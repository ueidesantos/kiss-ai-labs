using Squad;
using System.Text.Json;
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://127.0.0.1:8089");
builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 4096);
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
var policy = new Policy(10, 20);
if (Environment.GetEnvironmentVariable("SQUAD_POLICY") is { } file)
{
    policy = JsonSerializer.Deserialize<Policy>(File.ReadAllText(file), new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? throw new InvalidOperationException("Política inválida");
    Discount.Validate(policy);
}
app.MapPost("/api/discount", async (HttpContext context) =>
{
    try
    {
        using var json = await JsonDocument.ParseAsync(context.Request.Body, cancellationToken: context.RequestAborted);
        return Results.Ok(Discount.Calculate(json.RootElement, policy));
    }
    catch (Exception ex) when (ex is JsonException or ArgumentException or OverflowException)
    { return Results.BadRequest(new { error = ex is JsonException ? "JSON inválido." : ex.Message }); }
});
app.MapGet("/api/run", () =>
{
    var path = Environment.GetEnvironmentVariable("SQUAD_RUN");
    if (string.IsNullOrWhiteSpace(path)) return Results.NotFound(new { error = "Defina SQUAD_RUN com o caminho de report.json do runner." });
    if (Directory.Exists(path)) path = Path.Combine(path, "report.json");
    return File.Exists(path) ? Results.Text(File.ReadAllText(path), "application/json") : Results.NotFound(new { error = "Relatório ainda não disponível." });
});
app.Run();
