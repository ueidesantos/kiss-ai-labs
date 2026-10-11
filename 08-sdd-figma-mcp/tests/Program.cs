using System.Net;
using System.Text;
using System.Text.Json;

// F-6: fictitious MCP transport fixtures, never a real Figma connection.
var passed = 0;
async Task Check(string name, Func<Task> test)
{
    await test();
    Console.WriteLine($"PASS {name}");
    passed++;
}
void Assert(bool condition, string message) { if (!condition) throw new Exception(message); }
async Task Reject(string scenario, string expected)
{
    using var http = new HttpClient(new MockMcp(scenario));
    using var client = new FigmaMcpClient(http: http);
    try { await client.CaptureAsync("MOCK:1"); }
    catch (InvalidOperationException exception) when (exception.Message.Contains(expected)) { return; }
    throw new Exception($"Scenario {scenario} should fail with {expected}");
}

foreach (var mode in new[] { "json", "sse", "old-version" })
    await Check($"sequence-session-version-{mode}", async () =>
    {
        var mock = new MockMcp(mode);
        using var http = new HttpClient(mock);
        using var client = new FigmaMcpClient(http: http);
        var capture = await client.CaptureAsync("MOCK:1");
        Assert(mock.Methods.SequenceEqual(new[] { "initialize", "notifications/initialized", "tools/list", "tools/call", "tools/call", "tools/call" }), "Sequence");
        Assert(capture.GetProperty("mode").GetString() == "figma-mcp", "Capture transport label");
        Assert(capture.GetProperty("metadata").GetProperty("content")[0].GetProperty("text").GetString() == "MOCK ONLY", "Text preserved");
        Assert(capture.GetProperty("screenshot").GetProperty("content")[0].GetProperty("type").GetString() == "image", "Image preserved");
    });
await Check("wrong-response-id", () => Reject("wrong-id", "id divergente"));
await Check("json-rpc-error", () => Reject("rpc-error", "Erro JSON-RPC"));
await Check("missing-screenshot", () => Reject("missing-tool", "get_screenshot"));
await Check("unknown-required-argument", () => Reject("required", "obrigatório desconhecido"));
await Check("unsupported-schema-type", () => Reject("type", "Tipo de argumento"));
await Check("schema-rejects-value", () => Reject("enum", "Valor não aceito"));
await Check("tool-isError", () => Reject("is-error", "isError"));
await Check("unsupported-version", () => Reject("version", "Versão MCP"));
await Check("oversized-response", () => Reject("large", "4 MiB"));
await Check("oversized-sse-without-length", () => Reject("large-sse", "4 MiB"));
await Check("sse-wrong-response-id", () => Reject("sse-wrong-id", "id solicitado"));
await Check("sse-json-rpc-error", () => Reject("sse-rpc-error", "Erro JSON-RPC"));
await Check("empty-tool-result", () => Reject("empty", "vazio"));
await Check("node-required", async () =>
{
    using var client = new FigmaMcpClient(http: new HttpClient(new MockMcp("json")));
    try { await client.CaptureAsync(" "); }
    catch (ArgumentException) { return; }
    throw new Exception("Missing node should fail");
});
await Check("fixture-never-approves-real-design", () =>
{
    var fixture = JsonSerializer.SerializeToElement(new {
        mode = "offline-fixture", metadata = new { content = new[] { new { type = "text", text = "MOCK ONLY" } } },
        designContext = new { content = new[] { new { type = "text", text = "MOCK ONLY" } } }, screenshot = new { content = Array.Empty<object>() }
    });
    FigmaSpecification.ValidateCapture(fixture, allowFixture: true);
    try { FigmaSpecification.ValidateCapture(fixture, allowFixture: false); }
    catch (InvalidOperationException) { return Task.CompletedTask; }
    throw new Exception("Fixture approved as real design");
});
await Check("invalid-image-signature-blocks-visual-source", async () =>
{
    using var http = new HttpClient(new MockMcp("json"));
    using var client = new FigmaMcpClient(http: http);
    var capture = await client.CaptureAsync("MOCK:1");
    try { FigmaSpecification.ValidateCapture(capture, allowFixture: false); }
    catch (InvalidOperationException exception) when (exception.Message.Contains("Screenshot inválido")) { return; }
    throw new Exception("MOCK ONLY bytes were accepted as a PNG screenshot");
});
Console.WriteLine($"{passed} tests passed; all transport fixtures are fictitious.");

sealed class MockMcp(string mode) : HttpMessageHandler
{
    public List<string> Methods { get; } = [];
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
    {
        using var document = JsonDocument.Parse(await request.Content!.ReadAsStringAsync(token));
        var root = document.RootElement;
        var method = root.GetProperty("method").GetString()!;
        Methods.Add(method);
        if (!request.Headers.Accept.Any(v => v.MediaType == "application/json") || !request.Headers.Accept.Any(v => v.MediaType == "text/event-stream")) throw new Exception("Accept headers");
        var version = mode == "old-version" ? "2025-03-26" : "2025-06-18";
        if (method != "initialize")
        {
            if (request.Headers.GetValues("Mcp-Session-Id").Single() != "MOCK_SESSION") throw new Exception("Session header");
            if (request.Headers.GetValues("MCP-Protocol-Version").Single() != version) throw new Exception("Protocol header");
        }
        if (method == "notifications/initialized")
        {
            if (root.TryGetProperty("id", out _)) throw new Exception("Notification must not have id");
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
        var id = root.GetProperty("id").GetInt32();
        object result;
        if (method == "initialize") result = new { protocolVersion = mode == "version" ? "BAD" : version, capabilities = new { tools = new { } }, serverInfo = new { name = "MOCK ONLY", version = "1" } };
        else if (method == "tools/list")
        {
            var names = mode == "missing-tool" ? new[] { "get_metadata", "get_design_context" } : new[] { "get_metadata", "get_design_context", "get_screenshot" };
            result = new { tools = names.Select(name => new { name, inputSchema = new { type = "object", properties = new Dictionary<string, object> { ["nodeId"] = mode == "enum" ? (object)new { type = "string", @enum = new[] { "REJECTED" } } : new { type = mode == "type" ? "number" : "string" }, ["clientLanguages"] = new { type = "string" } }, required = mode == "required" ? new[] { "nodeId", "unknown" } : new[] { "nodeId" } } }).ToArray() };
        }
        else
        {
            var args = root.GetProperty("params").GetProperty("arguments");
            if (args.GetProperty("nodeId").GetString() != "MOCK:1" || args.GetProperty("clientLanguages").GetString() != "csharp,html,css,javascript" || args.TryGetProperty("clientFrameworks", out _)) throw new Exception("Schema-compatible arguments");
            var image = root.GetProperty("params").GetProperty("name").GetString() == "get_screenshot";
            result = new { isError = mode == "is-error", content = mode == "empty" ? Array.Empty<object>() : image ? new object[] { new { type = "image", mimeType = "image/png", data = "TU9DSyBPTkxZ" } } : new object[] { new { type = "text", text = "MOCK ONLY" } } };
        }
        var payload = mode.EndsWith("rpc-error") ? JsonSerializer.Serialize(new { jsonrpc = "2.0", id, error = new { code = -32602, message = "MOCK failure" } })
            : JsonSerializer.Serialize(new { jsonrpc = "2.0", id = mode.EndsWith("wrong-id") ? id + 100 : id, result });
        if (mode == "large") payload = new string(' ', FigmaMcpClient.MaxResponseBytes + 1);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Headers.Add("Mcp-Session-Id", "MOCK_SESSION");
        response.Content = mode.StartsWith("sse")
            ? new StringContent($"event: message\ndata: {{\"jsonrpc\":\"2.0\",\"method\":\"notifications/progress\"}}\n\nevent: message\ndata: {payload}\n\n", Encoding.UTF8, "text/event-stream")
            : new StringContent(payload, Encoding.UTF8, "application/json");
        if (mode == "large-sse") response.Content = new UnknownLengthSse();
        return response;
    }
}

sealed class UnknownLengthSse : HttpContent
{
    public UnknownLengthSse() { Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/event-stream"); }
    protected override bool TryComputeLength(out long length) { length = 0; return false; }
    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        => stream.WriteAsync(Encoding.UTF8.GetBytes("data: " + new string(' ', FigmaMcpClient.MaxResponseBytes + 1))).AsTask();
}
