using SddAgents;

try
{
    if (args.Contains("--help"))
    {
        Console.WriteLine("SDD + agents + RAG de negócio (.NET 11)\n" +
            "--offline  Replay de respostas preparadas, sem modelo\n" +
            "--cpu      Executar geração Ollama na CPU\n" +
            "--lab <pasta> --out <pasta-vazia> --model <nome> --url <endpoint> --top-k <n>\n" +
            "Padrão: lab descoberto a partir do executável, llama3.2, localhost:11434, K=3.");
        return;
    }
    var options = Options.Parse(args);
    var sources = Retrieval.Search(File.ReadAllText(Path.Combine(options.Lab, "DEMAND.md")),
        Path.Combine(options.Lab, "business"), options.TopK);
    if (sources.Count == 0) throw new InvalidOperationException("Nenhuma regra relacionada encontrada; reveja a demanda e os documentos.");
    var result = await Workflow.RunAsync(options, sources);
    Console.WriteLine($"Modo: {(options.Offline ? "REPLAY OFFLINE — sem chamadas de IA" : "Ollama — três chamadas de agents")}");
    foreach (var source in sources) Console.WriteLine($"[{source.Id}] {source.Path} — cosseno {source.Score:F3}");
    Console.WriteLine("TL → SPEC candidata + tarefas → Dev Front + Dev Back → propostas para revisão.");
    Console.WriteLine($"Artefatos: {result}");
    Console.WriteLine("Propostas não foram executadas. A aplicação de referência está em app/.");
}
catch (Exception error) when (error is ArgumentException or IOException or InvalidOperationException
                            or System.Text.Json.JsonException or HttpRequestException or TaskCanceledException
                            or UnauthorizedAccessException or KeyNotFoundException)
{
    Console.Error.WriteLine($"Erro: {error.Message}");
    Environment.ExitCode = 1;
}
