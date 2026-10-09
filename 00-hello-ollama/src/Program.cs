using System.Text;
using System.Text.Json;

var model = GetArg(args, "--model", "llama3.2");
var prompt = GetArg(args, "--prompt", "Olá!");
var baseUrl = GetArg(args, "--url", "http://localhost:11434").TrimEnd('/');

static string GetArg(string[] args, string key, string defaultValue)
{
    var idx = Array.IndexOf(args, key);
    return idx >= 0 && idx + 1 < args.Length ? args[idx + 1] : defaultValue;
}

Console.WriteLine($"> Modelo  : {model}");
Console.WriteLine($"> Prompt  : {prompt}");
Console.WriteLine($"> Endpoint: {baseUrl}");
Console.WriteLine(new string('-', 50));

using var http = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };

try
{
    var request = new
    {
        model,
        prompt,
        stream = true
    };

    var json = JsonSerializer.Serialize(request);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    using var response = await http.PostAsync($"{baseUrl}/api/generate", content);

    if (!response.IsSuccessStatusCode)
    {
        var errorBody = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound ||
            errorBody.Contains("no such file", StringComparison.OrdinalIgnoreCase) ||
            errorBody.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Erro: Modelo '{model}' não encontrado.");
            Console.WriteLine($"Dica: Execute 'ollama pull {model}' para baixar o modelo.");
        }
        else
        {
            Console.WriteLine($"Erro HTTP: {(int)response.StatusCode} {response.ReasonPhrase}");
            if (!string.IsNullOrWhiteSpace(errorBody))
                Console.WriteLine(errorBody);
        }
        Environment.Exit(1);
    }

    using var stream = await response.Content.ReadAsStreamAsync();
    using var reader = new StreamReader(stream);

    Console.Write("Resposta: ");

    string? line;
    while ((line = await reader.ReadLineAsync()) is not null)
    {
        if (string.IsNullOrWhiteSpace(line)) continue;

        try
        {
            using var doc = JsonDocument.Parse(line);
            if (doc.RootElement.TryGetProperty("response", out var resp))
            {
                Console.Write(resp.GetString());
            }
            if (doc.RootElement.TryGetProperty("done", out var done) && done.GetBoolean())
            {
                break;
            }
        }
        catch (JsonException)
        {
            Console.Write(line);
        }
    }

    Console.WriteLine();
    Console.WriteLine(new string('-', 50));
    Console.WriteLine("Concluído com sucesso.");
}
catch (HttpRequestException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
{
    Console.WriteLine($"Erro: Não foi possível conectar ao Ollama em {baseUrl}");
    Console.WriteLine("Dica: Verifique se o Ollama está rodando.");
    Console.WriteLine("      No Windows/Mac, abra o aplicativo Ollama.");
    Console.WriteLine("      No Linux, execute: ollama serve");
    Environment.Exit(1);
}
catch (TaskCanceledException)
{
    Console.WriteLine("Erro: Tempo esgotado aguardando resposta do modelo.");
    Environment.Exit(1);
}
catch (Exception ex)
{
    Console.WriteLine($"Erro inesperado: {ex.Message}");
    Environment.Exit(1);
}
