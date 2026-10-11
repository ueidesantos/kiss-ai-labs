using System.Text.Json;
using SddAgents;

try
{
    if (args.Length == 0 || args.Contains("--help"))
    {
        Console.WriteLine("Figma MCP + SDD + agents (.NET 11)\n" +
            "capture --node <ID-real> [--mcp-url <endpoint>] [--out <pasta-vazia>]\n" +
            "spec --capture <capture.json> [--offline] [--cpu] [--model <nome>] [--url <Ollama>] [--out <pasta-vazia>]\n" +
            "demo [--out <pasta-vazia>]  Fixture fictícia + replay; não acessa Figma nem modelo.\n" +
            "--lab <pasta> define o lab 08 quando não for encontrado a partir do executável.");
        return;
    }
    var command = args[0];
    if (command is not ("capture" or "spec" or "demo")) throw new ArgumentException("Use capture, spec ou demo.");
    var values = new Dictionary<string, string>();
    var flags = new HashSet<string>();
    var allowed = new[] { "--node", "--mcp-url", "--capture", "--out", "--lab", "--model", "--url" };
    for (var i = 1; i < args.Length; i++)
    {
        var key = args[i];
        if (key is "--offline" or "--cpu")
        {
            if (!flags.Add(key)) throw new ArgumentException($"Opção repetida: {key}");
            continue;
        }
        if (!allowed.Contains(key) || i + 1 >= args.Length || args[i + 1].StartsWith("--") || string.IsNullOrWhiteSpace(args[i + 1]))
            throw new ArgumentException($"Opção inválida ou sem valor: {key}");
        if (!values.TryAdd(key, args[++i])) throw new ArgumentException($"Opção repetida: {key}");
    }
    string Get(string key, string fallback) => values.GetValueOrDefault(key, fallback);
    var lab = Path.GetFullPath(values.TryGetValue("--lab", out var suppliedLab) ? suppliedLab : FindLab());
    var output = Path.GetFullPath(Get("--out", Path.Combine(lab, "runs", DateTime.UtcNow.ToString("yyyyMMddTHHmmssfffffffZ"))));
    if (Directory.Exists(output) && Directory.EnumerateFileSystemEntries(output).Any())
        throw new InvalidOperationException("Saída não está vazia; escolha outra --out.");
    if (command == "capture")
    {
        if (flags.Contains("--offline")) throw new ArgumentException("capture exige Figma real. Use demo para fixture.");
        if (!values.TryGetValue("--node", out var node)) throw new ArgumentException("Informe --node com o ID do frame real.");
        using var mcp = new FigmaMcpClient(Get("--mcp-url", FigmaMcpClient.DefaultEndpoint));
        var capture = await mcp.CaptureAsync(node);
        FigmaSpecification.ValidateCapture(capture, allowFixture: false);
        Directory.CreateDirectory(output);
        File.WriteAllText(Path.Combine(output, "capture.json"), capture.GetRawText());
        FigmaSpecification.WriteVisualSource(capture, output);
        Console.WriteLine($"Captura MCP real registrada em {output}. Revise os frames antes de implementar.");
    }
    else
    {
        var offline = command == "demo" || flags.Contains("--offline");
        var capturePath = command == "demo" ? Path.Combine(lab, "fixtures", "design.sample.json")
            : values.TryGetValue("--capture", out var path) ? Path.GetFullPath(path)
            : throw new ArgumentException("Informe --capture com a evidência MCP.");
        using var document = JsonDocument.Parse(File.ReadAllText(capturePath));
        var capture = document.RootElement.Clone();
        FigmaSpecification.ValidateCapture(capture, allowFixture: offline);
        var baseLab = Path.GetFullPath(Path.Combine(lab, "../07-sdd-agents-business-rag"));
        var business = Retrieval.Search(File.ReadAllText(Path.Combine(lab, "DEMAND.md")), Path.Combine(baseLab, "business"), 3);
        var sourceId = FigmaSpecification.IsFixture(capture) ? "FIGMA-FIXTURE" : "FIGMA";
        var sources = business.Append(new Source(sourceId, "Figma MCP design context", FigmaSpecification.Text(capture), 1)).ToArray();
        var options = Options.Parse(["--lab", baseLab, "--out", output, "--url", Get("--url", "http://localhost:11434"),
            "--model", Get("--model", "llama3.2"), .. (offline ? new[] { "--offline" } : Array.Empty<string>()),
            .. (flags.Contains("--cpu") ? new[] { "--cpu" } : Array.Empty<string>())]);
        await Workflow.RunAsync(options, sources);
        var plan = JsonSerializer.Deserialize<Plan>(File.ReadAllText(Path.Combine(output, "plan.json")), Workflow.Json)!;
        var proposals = JsonSerializer.Deserialize<Proposal[]>(File.ReadAllText(Path.Combine(output, "handoffs.json")), Workflow.Json)!;
        var referenced = plan.Requirements.Single(requirement => requirement.Id == "REQ-3").Sources.Contains(sourceId)
            && proposals.Single(proposal => proposal.Role == "front").Sources.Contains(sourceId);
        File.WriteAllText(Path.Combine(output, "capture.json"), capture.GetRawText());
        FigmaSpecification.WriteVisualSource(capture, output);
        File.WriteAllText(Path.Combine(output, "figma-review.json"), JsonSerializer.Serialize(new {
            sourceMode = capture.GetProperty("mode").GetString(), designSourceId = sourceId,
            frontendReferencesDesign = referenced, visualMatchVerified = false,
            figmaFileCreatedByThisRun = false, status = "needs-design-and-human-review"
        }, Workflow.Json));
        if (!referenced) throw new InvalidOperationException("TL/Front não referenciaram o design; propostas bloqueadas para revisão.");
        Console.WriteLine($"Modo: {(offline ? "REPLAY OFFLINE" : "Ollama + captura MCP")}; fonte: {sourceId}");
        Console.WriteLine($"SPEC candidata do TL e fonte visual: {output}");
        Console.WriteLine("Conformidade visual e criação do arquivo Figma não são aprovadas por esta execução.");
    }
}
catch (Exception error) when (error is ArgumentException or InvalidOperationException or IOException or UnauthorizedAccessException
                            or JsonException or KeyNotFoundException or HttpRequestException or TaskCanceledException or FormatException)
{
    Console.Error.WriteLine($"Erro: {error.Message}");
    Environment.ExitCode = 1;
}

static string FindLab()
{
    for (var folder = new DirectoryInfo(AppContext.BaseDirectory); folder != null; folder = folder.Parent)
        if (File.Exists(Path.Combine(folder.FullName, "fixtures", "design.sample.json"))) return folder.FullName;
    throw new ArgumentException("Lab 08 não encontrado; use --lab.");
}
