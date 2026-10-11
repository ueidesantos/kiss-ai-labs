using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

// F-1..F-3: a small read-only MCP client; returned code is data, never executed.
public sealed class FigmaMcpClient : IDisposable
{
    public const int MaxResponseBytes = 4 * 1024 * 1024;
    public const string DefaultEndpoint = "http://127.0.0.1:3845/mcp";
    private readonly HttpClient http;
    private readonly bool ownsHttp;
    private readonly Uri endpoint;
    private string? session;
    private string? protocol;
    private int nextId;

    public FigmaMcpClient(string endpoint = DefaultEndpoint, HttpClient? http = null)
    {
        this.endpoint = new Uri(endpoint, UriKind.Absolute);
        if (this.endpoint.Scheme is not ("http" or "https") || !string.IsNullOrEmpty(this.endpoint.UserInfo))
            throw new ArgumentException("Use um endpoint HTTP/HTTPS sem credenciais na URL.");
        this.http = http ?? new HttpClient();
        ownsHttp = http is null;
    }

    public async Task<JsonElement> CaptureAsync(string nodeId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nodeId)) throw new ArgumentException("Informe o nodeId real do Figma.");
        session = null;
        protocol = null;
        var initialized = await Request("initialize", new
        {
            protocolVersion = "2025-06-18",
            capabilities = new { },
            clientInfo = new { name = "kiss-ai-lab-08", version = "1.0.0" }
        }, cancellationToken);
        if (!initialized.TryGetProperty("protocolVersion", out var version)
            || version.ValueKind != JsonValueKind.String
            || version.GetString() is not ("2025-06-18" or "2025-03-26"))
            throw new InvalidOperationException("Versão MCP não suportada pelo lab.");
        protocol = version.GetString();
        await NotifyInitialized(cancellationToken);

        var tools = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        string? cursor = null;
        for (var page = 0; page < 20; page++)
        {
            var listed = await Request("tools/list", cursor is null ? new { } : (object)new { cursor }, cancellationToken);
            if (!listed.TryGetProperty("tools", out var items) || items.ValueKind != JsonValueKind.Array)
                throw new InvalidOperationException("tools/list não retornou ferramentas.");
            foreach (var tool in items.EnumerateArray())
                tools[tool.GetProperty("name").GetString() ?? ""] = tool.Clone();
            cursor = listed.TryGetProperty("nextCursor", out var next) ? next.GetString() : null;
            if (string.IsNullOrEmpty(cursor)) break;
            if (page == 19) throw new InvalidOperationException("tools/list excedeu o limite de páginas.");
        }

        var names = new[] { "get_metadata", "get_design_context", "get_screenshot" };
        // Validate all schemas before any tool call, including the required screenshot.
        var arguments = names.ToDictionary(name => name, name =>
            tools.TryGetValue(name, out var tool) ? Arguments(tool, nodeId)
                : throw new InvalidOperationException($"Ferramenta obrigatória ausente: {name}."));
        var results = new Dictionary<string, JsonElement>();
        foreach (var name in names)
        {
            var result = await Request("tools/call", new { name, arguments = arguments[name] }, cancellationToken);
            if (result.TryGetProperty("isError", out var error) && error.ValueKind == JsonValueKind.True)
                throw new InvalidOperationException($"Ferramenta {name} retornou isError; captura recusada.");
            var hasContent = result.TryGetProperty("content", out var content)
                && content.ValueKind == JsonValueKind.Array && content.GetArrayLength() > 0;
            if (!hasContent && !result.TryGetProperty("structuredContent", out _))
                throw new InvalidOperationException($"Ferramenta {name} retornou resultado vazio.");
            results[name] = result;
        }
        return JsonSerializer.SerializeToElement(new
        {
            mode = "figma-mcp", endpoint = endpoint.ToString(), nodeId,
            time = DateTimeOffset.UtcNow.ToString("O"), protocolVersion = protocol,
            metadata = results["get_metadata"], designContext = results["get_design_context"],
            screenshot = results["get_screenshot"]
        });
    }

    private static Dictionary<string, string> Arguments(JsonElement tool, string nodeId)
    {
        if (!tool.TryGetProperty("inputSchema", out var schema)
            || !schema.TryGetProperty("properties", out var properties)
            || properties.ValueKind != JsonValueKind.Object || !properties.TryGetProperty("nodeId", out _))
            throw new InvalidOperationException("A ferramenta não declara o argumento nodeId.");
        var known = new Dictionary<string, string>
        {
            ["nodeId"] = nodeId, ["clientLanguages"] = "csharp,html,css,javascript",
            ["clientFrameworks"] = "aspnetcore"
        };
        var arguments = new Dictionary<string, string>();
        foreach (var property in properties.EnumerateObject())
        {
            if (!known.TryGetValue(property.Name, out var value)) continue;
            if (property.Value.TryGetProperty("type", out var type)
                && !(type.ValueKind == JsonValueKind.String && type.GetString() == "string")
                && !(type.ValueKind == JsonValueKind.Array && type.EnumerateArray().Any(t => t.GetString() == "string")))
                throw new InvalidOperationException($"Tipo de argumento não suportado: {property.Name}.");
            if (property.Value.TryGetProperty("enum", out var choices)
                && !choices.EnumerateArray().Any(choice => choice.ValueKind == JsonValueKind.String && choice.GetString() == value))
                throw new InvalidOperationException($"Valor não aceito pelo schema: {property.Name}.");
            arguments[property.Name] = value;
        }
        if (schema.TryGetProperty("required", out var required))
            foreach (var item in required.EnumerateArray())
                if (!arguments.ContainsKey(item.GetString() ?? ""))
                    throw new InvalidOperationException($"Argumento obrigatório desconhecido: {item.GetString()}.");
        return arguments;
    }

    private HttpRequestMessage Message(object payload)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };
        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        if (session is not null) message.Headers.Add("Mcp-Session-Id", session);
        if (protocol is not null) message.Headers.Add("MCP-Protocol-Version", protocol);
        return message;
    }

    private async Task NotifyInitialized(CancellationToken cancellationToken)
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(30));
        using var message = Message(new { jsonrpc = "2.0", method = "notifications/initialized" });
        using var response = await http.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, timeout.Token);
        response.EnsureSuccessStatusCode();
    }

    private async Task<JsonElement> Request(string method, object parameters, CancellationToken cancellationToken)
    {
        var id = ++nextId;
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(30));
        using var message = Message(new { jsonrpc = "2.0", id, method, @params = parameters });
        using var response = await http.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, timeout.Token);
        response.EnsureSuccessStatusCode();
        if (response.Headers.TryGetValues("Mcp-Session-Id", out var values)) session = values.Single();
        if (response.Content.Headers.ContentLength > MaxResponseBytes)
            throw new InvalidOperationException("Resposta MCP excede 4 MiB, incluindo imagens base64.");
        await using var stream = await response.Content.ReadAsStreamAsync(timeout.Token);
        if (response.Content.Headers.ContentType?.MediaType == "text/event-stream")
            return await ReadSse(stream, id, timeout.Token);
        if (response.Content.Headers.ContentType?.MediaType != "application/json")
            throw new InvalidOperationException("Content-Type MCP deve ser JSON ou SSE.");
        using var body = new MemoryStream();
        var buffer = new byte[8192];
        int read;
        while ((read = await stream.ReadAsync(buffer, timeout.Token)) > 0)
        {
            if (body.Length + read > MaxResponseBytes) throw new InvalidOperationException("Resposta MCP excede 4 MiB.");
            body.Write(buffer, 0, read);
        }
        using var document = JsonDocument.Parse(body.ToArray());
        return Result(document.RootElement, id);
    }

    private static async Task<JsonElement> ReadSse(Stream stream, int id, CancellationToken token)
    {
        using var reader = new StreamReader(new LimitedReadStream(stream), Encoding.UTF8);
        var data = new StringBuilder();
        var size = 0;
        while (await reader.ReadLineAsync(token) is { } line)
        {
            size += Encoding.UTF8.GetByteCount(line) + 1;
            if (size > MaxResponseBytes) throw new InvalidOperationException("Resposta SSE excede 4 MiB.");
            if (line.StartsWith("data:", StringComparison.Ordinal))
                data.AppendLine(line[5..].TrimStart(' '));
            if (line.Length != 0 || data.Length == 0) continue;
            using var document = JsonDocument.Parse(data.ToString());
            data.Clear();
            if (HasId(document.RootElement, id)) return Result(document.RootElement, id);
        }
        throw new InvalidOperationException("SSE terminou sem resposta para o id solicitado.");
    }

    private static bool HasId(JsonElement value, int id) => value.TryGetProperty("id", out var returned)
        && returned.ValueKind == JsonValueKind.Number && returned.TryGetInt32(out var number) && number == id;

    private static JsonElement Result(JsonElement value, int id)
    {
        if (!HasId(value, id)) throw new InvalidOperationException("Resposta JSON-RPC com id divergente.");
        if (!value.TryGetProperty("jsonrpc", out var rpc) || rpc.GetString() != "2.0")
            throw new InvalidOperationException("Resposta JSON-RPC inválida.");
        if (value.TryGetProperty("error", out var error))
            throw new InvalidOperationException($"Erro JSON-RPC: {error.GetRawText()}.");
        if (!value.TryGetProperty("result", out var result)) throw new InvalidOperationException("Resposta sem result.");
        return result.Clone();
    }

    public void Dispose() { if (ownsHttp) http.Dispose(); }

    // Bound SSE bytes before ReadLine can allocate an arbitrarily large line.
    private sealed class LimitedReadStream(Stream inner) : Stream
    {
        private long readTotal;
        private int Count(int count)
        {
            readTotal += count;
            if (readTotal > MaxResponseBytes) throw new InvalidOperationException("Resposta SSE excede 4 MiB.");
            return count;
        }
        public override int Read(byte[] buffer, int offset, int count) => Count(inner.Read(buffer, offset, Math.Min(count, 8192)));
        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken token = default)
            => Count(await inner.ReadAsync(buffer[..Math.Min(buffer.Length, 8192)], token));
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
