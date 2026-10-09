using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var accessToken = Environment.GetEnvironmentVariable("SPOTIFY_ACCESS_TOKEN");

if (string.IsNullOrWhiteSpace(accessToken))
{
    Error("SPOTIFY_ACCESS_TOKEN não configurado.");
    Info("Defina a variável de ambiente (terminal atual):");
    Info("  PowerShell: $env:SPOTIFY_ACCESS_TOKEN = 'seu-token'");
    Info("  CMD:        set SPOTIFY_ACCESS_TOKEN=seu-token");
    Info("  Bash:       export SPOTIFY_ACCESS_TOKEN='seu-token'");
    Environment.Exit(1);
}

using var http = new HttpClient();
http.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", accessToken);

var serializerOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

await RunMcpStdioLoopAsync();

async Task RunMcpStdioLoopAsync()
{
    using var stdin = Console.OpenStandardInput();
    using var reader = new StreamReader(stdin, Encoding.UTF8);

    string? line;
    while ((line = await reader.ReadLineAsync()) is not null)
    {
        if (string.IsNullOrWhiteSpace(line)) continue;

        JsonDocument? requestDoc = null;
        object? id = null;
        string? method = null;
        JsonElement? parameters = null;

        try
        {
            requestDoc = JsonDocument.Parse(line);
            var root = requestDoc.RootElement;

            if (root.TryGetProperty("id", out var idEl))
                id = idEl.ValueKind == JsonValueKind.Number ? (object)idEl.GetInt32() : idEl.GetString();

            method = root.TryGetProperty("method", out var mEl) ? mEl.GetString() : null;

            if (root.TryGetProperty("params", out var pEl))
                parameters = pEl;
        }
        catch (Exception ex)
        {
            await WriteResponseAsync(new { jsonrpc = "2.0", error = new { code = -32700, message = $"Parse error: {ex.Message}" } });
            requestDoc?.Dispose();
            continue;
        }

        object? result = null;
        object? error = null;

        try
        {
            switch (method)
            {
                case "initialize":
                    result = new
                    {
                        protocolVersion = "2024-11-05",
                        capabilities = new
                        {
                            tools = new { }
                        },
                        serverInfo = new
                        {
                            name = "mcp-spotify-server",
                            version = "1.0.0"
                        }
                    };
                    break;

                case "notifications/initialized":
                    requestDoc?.Dispose();
                    continue;

                case "tools/list":
                    result = new
                    {
                        tools = new object[]
                        {
                            new
                            {
                                name = "search_track",
                                description = "Busca faixas (tracks) no Spotify por palavra-chave (nome, artista, álbum).",
                                inputSchema = new
                                {
                                    type = "object",
                                    properties = new
                                    {
                                        query = new
                                        {
                                            type = "string",
                                            description = "Termo de busca (ex: 'The Beatles' ou 'Yesterday')"
                                        }
                                    },
                                    required = new[] { "query" }
                                }
                            },
                            new
                            {
                                name = "get_currently_playing",
                                description = "Retorna a música que está tocando agora na conta do usuário. Requer escopo user-read-currently-playing.",
                                inputSchema = new
                                {
                                    type = "object",
                                    properties = new { }
                                }
                            }
                        }
                    };
                    break;

                case "tools/call":
                    if (!parameters.HasValue)
                        throw new InvalidOperationException("Parâmetros ausentes.");

                    var p = parameters.Value;
                    var toolName = p.TryGetProperty("name", out var tnEl) ? tnEl.GetString() : null;
                    var argsEl = p.TryGetProperty("arguments", out var aEl) ? aEl : default;

                    result = toolName switch
                    {
                        "search_track" => await SearchTrackAsync(argsEl),
                        "get_currently_playing" => await GetCurrentlyPlayingAsync(),
                        _ => throw new InvalidOperationException($"Tool desconhecida: {toolName}")
                    };
                    break;

                default:
                    error = new { code = -32601, message = $"Método não encontrado: {method}" };
                    break;
            }
        }
        catch (HttpRequestException hrex) when ((int?)hrex.StatusCode == 401)
        {
            error = new { code = 401, message = "Token Spotify inválido ou expirado (401 Unauthorized)." };
        }
        catch (HttpRequestException hrex)
        {
            error = new { code = (int?)(hrex.StatusCode ?? 0) ?? -1, message = $"Erro Spotify HTTP: {hrex.Message}" };
        }
        catch (Exception ex)
        {
            error = new { code = -32000, message = ex.Message };
        }

        if (id is not null)
        {
            if (error is not null)
                await WriteResponseAsync(new { jsonrpc = "2.0", id, error });
            else
                await WriteResponseAsync(new { jsonrpc = "2.0", id, result });
        }

        requestDoc?.Dispose();
    }
}

async Task WriteResponseAsync(object response)
{
    var json = JsonSerializer.Serialize(response, serializerOptions);
    await Console.Out.WriteLineAsync(json);
    await Console.Out.FlushAsync();
}

async Task<object> SearchTrackAsync(JsonElement argsEl)
{
    var query = argsEl.TryGetProperty("query", out var qEl) ? qEl.GetString() : null;
    if (string.IsNullOrWhiteSpace(query))
        throw new InvalidOperationException("Argumento 'query' é obrigatório.");

    var url = $"https://api.spotify.com/v1/search?type=track&q={Uri.EscapeDataString(query)}&limit=5";

    using var resp = await http.GetAsync(url);
    resp.EnsureSuccessStatusCode();

    using var doc = await JsonDocument.ParseAsync(await resp.Content.ReadAsStreamAsync());

    var tracks = doc.RootElement
        .GetProperty("tracks")
        .GetProperty("items");

    var results = new List<object>();
    foreach (var t in tracks.EnumerateArray())
    {
        var artist = t.GetProperty("artists").EnumerateArray().FirstOrDefault();
        results.Add(new
        {
            name = t.GetProperty("name").GetString(),
            artist = artist.GetProperty("name").GetString(),
            album = t.GetProperty("album").GetProperty("name").GetString(),
            uri = t.GetProperty("uri").GetString(),
            external_url = t.GetProperty("external_urls").GetProperty("spotify").GetString()
        });
    }

    return new { content = new[] { new { type = "text", text = JsonSerializer.Serialize(results, serializerOptions) } } };
}

async Task<object> GetCurrentlyPlayingAsync()
{
    const string url = "https://api.spotify.com/v1/me/player/currently-playing";

    using var resp = await http.GetAsync(url);
    resp.EnsureSuccessStatusCode();

    if (resp.StatusCode == System.Net.HttpStatusCode.NoContent)
    {
        return new
        {
            content = new[]
            {
                new { type = "text", text = JsonSerializer.Serialize(new { is_playing = false, message = "Nenhuma música tocando no momento." }, serializerOptions) }
            }
        };
    }

    using var doc = await JsonDocument.ParseAsync(await resp.Content.ReadAsStreamAsync());

    var isPlaying = doc.RootElement.TryGetProperty("is_playing", out var ipEl) && ipEl.GetBoolean();
    var progressMs = doc.RootElement.TryGetProperty("progress_ms", out var pEl) ? pEl.GetInt32() : 0;

    JsonElement itemEl = default;
    if (doc.RootElement.TryGetProperty("item", out var iEl))
        itemEl = iEl;

    if (itemEl.ValueKind == JsonValueKind.Object)
    {
        var artist = itemEl.GetProperty("artists").EnumerateArray().FirstOrDefault();
        var durationMs = itemEl.TryGetProperty("duration_ms", out var dEl) ? dEl.GetInt32() : 0;

        var nowPlaying = new
        {
            is_playing = isPlaying,
            name = itemEl.GetProperty("name").GetString(),
            artist = artist.GetProperty("name").GetString(),
            album = itemEl.GetProperty("album").GetProperty("name").GetString(),
            progress_ms = progressMs,
            duration_ms = durationMs
        };

        return new { content = new[] { new { type = "text", text = JsonSerializer.Serialize(nowPlaying, serializerOptions) } } };
    }

    return new
    {
        content = new[]
        {
            new { type = "text", text = JsonSerializer.Serialize(new { is_playing = isPlaying, message = "Reprodutor vazio ou desconhecido." }, serializerOptions) }
        }
    };
}

static void Error(string msg) => Console.Error.WriteLine($"[ERRO] {msg}");
static void Info(string msg) => Console.Error.WriteLine($"[INFO] {msg}");
