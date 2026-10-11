using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SddAgents;

var lab = Options.Parse([]).Lab;
var temp = Path.Combine(lab, "runs", "tests-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(temp);
var passed = 0;
void Check(bool valid, string name)
{
    if (!valid) throw new InvalidOperationException("FAIL: " + name);
    passed++;
    Console.WriteLine("PASS: " + name);
}
void Reject(Action action, string name)
{
    try { action(); }
    catch (Exception error) when (error is ArgumentException or InvalidOperationException) { Check(true, name); return; }
    throw new InvalidOperationException("FAIL (accepted): " + name);
}
async Task<(int Status, JsonElement Json)> Send(string body, long? length = null)
{
    var context = new DefaultHttpContext();
    context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
    context.Request.ContentLength = length;
    context.Response.Body = new MemoryStream();
    await DiscountEndpoint.Handle(context);
    context.Response.Body.Position = 0;
    using var json = await JsonDocument.ParseAsync(context.Response.Body);
    Check(context.Response.ContentType!.StartsWith("application/json"), "HTTP JSON content type");
    return (context.Response.StatusCode, json.RootElement.Clone());
}
try
{
    foreach (var (input, total) in new[] {
        ("{\"amount\":100,\"discount\":10}", "90.00"),
        ("{\"amount\":100,\"discount\":0}", "100.00"),
        ("{\"amount\":100,\"discount\":20}", "80.00"),
        ("{\"amount\":10.005,\"discount\":0}", "10.01") })
    {
        var response = await Send(input);
        Check(response.Status == 200 && response.Json.GetProperty("total").GetString() == total, "AC-4 calculate " + total);
    }
    foreach (var input in new[] { "{}", "[]", "null", "invalid", "{\"amount\":0,\"discount\":10}",
        "{\"amount\":-1,\"discount\":10}", "{\"amount\":true,\"discount\":10}", "{\"amount\":\"100\",\"discount\":10}",
        "{\"amount\":100,\"discount\":-1}", "{\"amount\":100,\"discount\":21}",
        "{\"amount\":100,\"discount\":true}", "{\"amount\":1e999,\"discount\":10}",
        "{\"amount\":100,\"discount\":10,\"other\":1}" })
    {
        var response = await Send(input);
        Check(response.Status == 400 && !string.IsNullOrWhiteSpace(response.Json.GetProperty("error").GetString()), "AC-4 rejects " + input);
    }
    Check((await Send(new string(' ', 4097))).Status == 400, "body bound without Content-Length");
    Check((await Send("{}", 4097)).Status == 400, "body bound with Content-Length");

    var docs = Path.Combine(temp, "business");
    Directory.CreateDirectory(docs);
    File.WriteAllText(Path.Combine(docs, "a.md"), "Frete entrega transporte.");
    File.WriteAllText(Path.Combine(docs, "b.md"), "Desconto desconto percentual pedido.");
    var ranked = Retrieval.Search("desconto percentual", docs, 1);
    Check(ranked.Count == 1 && ranked[0].Path == "b.md" && ranked[0].Id == "S2", "AC-1 ranking and stable source ID");
    Check(Retrieval.Search("zzunknown", docs, 3).Count == 0, "AC-1 no unrelated context");
    File.WriteAllText(Path.Combine(docs, "b.md"), "Política de desconto.");
    Check(Retrieval.Search("politica", docs, 1)[0].Path == "b.md", "AC-1 Portuguese accent normalization");
    var emptyDocs = Path.Combine(temp, "empty-business");
    Directory.CreateDirectory(emptyDocs);
    Reject(() => Retrieval.Search("desconto", emptyDocs, 3), "AC-1 empty corpus rejected");
    Reject(() => Retrieval.Search("desconto", docs, 0), "invalid Top-K");
    foreach (var argsCase in new[] { new[] { "--top-k", "0" }, new[] { "--url", "bad" }, new[] { "--unknown" },
                                    new[] { "--model" }, new[] { "--cpu", "--cpu" } })
        Reject(() => Options.Parse(argsCase), "invalid CLI " + string.Join(' ', argsCase));

    var sources = Retrieval.Search(File.ReadAllText(Path.Combine(lab, "DEMAND.md")), Path.Combine(lab, "business"), 3);
    Check(sources.Count == 3, "AC-1 three business sources retrieved for demo");
    var plan = Workflow.ReferencePlan(sources);
    Workflow.ValidatePlan(plan, sources);
    Check(true, "AC-2 valid TL plan");
    Reject(() => Workflow.ValidatePlan(plan with { Contract = Workflow.Api with { Path = "/wrong" } }, sources), "AC-2 cannot alter API contract");
    Reject(() => Workflow.ValidatePlan(plan with { Requirements = [plan.Requirements[0] with { Sources = ["invented"] }, plan.Requirements[1]] }, sources), "AC-2 invented source");
    Reject(() => Workflow.ValidatePlan(plan with { Tasks = [plan.Tasks[0], plan.Tasks[0]] }, sources), "AC-2 duplicate dev role");
    Reject(() => Workflow.ValidatePlan(plan with { Tasks = [plan.Tasks[0] with { Requirements = ["REQ-4"] }, plan.Tasks[1]] }, sources), "AC-2 wrong delegation");
    Reject(() => Workflow.ValidatePlan(plan with { Acceptance = [plan.Acceptance[0], plan.Acceptance[0]] }, sources), "AC-2 duplicate acceptance IDs");
    Reject(() => Workflow.ValidatePlan(plan with { Requirements = [null!, plan.Requirements[1]] }, sources), "malformed model array");
    var task = plan.Tasks[0];
    Reject(() => Workflow.ValidateProposal(new("back", task.Requirements, [sources[0].Id], "code", ["check"]), task, sources), "AC-5 wrong dev reply");
    Reject(() => Workflow.ValidateProposal(new("front", task.Requirements, ["fake"], "code", ["check"]), task, sources), "AC-5 invented dev source");

    var offline = new Options(lab, Path.Combine(temp, "offline"), "http://localhost:11434", "unused", 3, true, true);
    var forbiddenHttp = new ModelStub(plan, sources, rejectCalls: true);
    await Workflow.RunAsync(offline, sources, forbiddenHttp);
    using var review = JsonDocument.Parse(File.ReadAllText(Path.Combine(offline.Output, "review.json")));
    Check(forbiddenHttp.Calls.Count == 0 && review.RootElement.GetProperty("mode").GetString() == "offline-replay", "AC-6 replay has zero model calls");
    Check(!review.RootElement.GetProperty("generatedCodeExecuted").GetBoolean(), "AC-6 never claim execution of proposals");
    Check(File.Exists(Path.Combine(offline.Output, "front.proposal.html")) && File.Exists(Path.Combine(offline.Output, "back.proposal.cs")), "AC-5 fixed output filenames");
    try { await Workflow.RunAsync(offline, sources); throw new Exception("overwrite accepted"); }
    catch (InvalidOperationException) { Check(true, "existing output is preserved"); }

    var model = new ModelStub(plan, sources);
    var live = offline with { Output = Path.Combine(temp, "mock-model"), Offline = false };
    await Workflow.RunAsync(live, sources, model);
    Check(model.Calls.Count == 3, "AC-5 one TL and two dev calls");
    Check(model.Calls[0].GetProperty("system").GetString()!.Contains("TL coordenador") &&
          model.Calls[1].GetProperty("system").GetString()!.Contains("Dev Front") &&
          model.Calls[2].GetProperty("system").GetString()!.Contains("Dev Back"), "AC-5 distinct agent roles");
    Check(!model.Calls[1].GetProperty("prompt").GetString()!.Contains(plan.Tasks[1].Instruction) &&
          !model.Calls[2].GetProperty("prompt").GetString()!.Contains(plan.Tasks[0].Instruction), "AC-5 each dev receives only its own task");
    Check(model.Calls.All(call => call.GetProperty("options").GetProperty("num_gpu").GetInt32() == 0), "CPU option passed to all agents");
    Console.WriteLine($"PASS: {passed} checks. Native .NET calculation/HTTP handler, RAG, delegation, offline and mock-model pipeline.");
}
catch (Exception error)
{
    Console.Error.WriteLine(error);
    Environment.ExitCode = 1;
}
finally
{
    var full = Path.GetFullPath(temp);
    var runsRoot = Path.GetFullPath(Path.Combine(lab, "runs")) + Path.DirectorySeparatorChar;
    if (full.StartsWith(runsRoot, StringComparison.OrdinalIgnoreCase) && Path.GetFileName(full).StartsWith("tests-"))
        Directory.Delete(full, recursive: true);
}

sealed class ModelStub(Plan plan, IReadOnlyList<Source> sources, bool rejectCalls = false) : HttpMessageHandler
{
    public List<JsonElement> Calls { get; } = [];
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (rejectCalls) throw new InvalidOperationException("Offline mode called a model.");
        using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync(cancellationToken));
        Calls.Add(body.RootElement.Clone());
        object payload = Calls.Count == 1 ? plan : new Proposal(Calls.Count == 2 ? "front" : "back",
            [Calls.Count == 2 ? "REQ-3" : "REQ-4"], [sources[0].Id], "// proposed code", ["proposed check"]);
        return new(HttpStatusCode.OK) { Content = new StringContent(JsonSerializer.Serialize(new {
            done = true, response = JsonSerializer.Serialize(payload, Workflow.Json)
        }), Encoding.UTF8, "application/json") };
    }
}
