using Squad;
using System.Text.Json;
var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory });
builder.WebHost.UseUrls("http://127.0.0.1:8089");
builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 4096);
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
var policy = new Policy(10, 20);
var policyFile = Environment.GetEnvironmentVariable("SQUAD_POLICY") ?? Path.Combine(AppContext.BaseDirectory, "policy.json");
if (File.Exists(policyFile))
    policy = JsonSerializer.Deserialize<Policy>(File.ReadAllText(policyFile), jsonOptions) ?? throw new InvalidOperationException("Política inválida");
else if (Environment.GetEnvironmentVariable("SQUAD_POLICY") is not null) throw new FileNotFoundException("Política configurada não encontrada.");
Discount.Validate(policy);
var ui = new UiText("Simule um desconto", "Cliente Padrão: até 10%. VIP: até 20%. O perfil é informado apenas para esta simulação.", "Padrão · até 10%", "VIP · até 20%");
var frontFile = Environment.GetEnvironmentVariable("SQUAD_FRONT") ?? Path.Combine(AppContext.BaseDirectory, "front.json");
if (File.Exists(frontFile))
    ui = JsonSerializer.Deserialize<UiText>(File.ReadAllText(frontFile), jsonOptions) ?? throw new InvalidOperationException("Textos inválidos.");
else if (Environment.GetEnvironmentVariable("SQUAD_FRONT") is not null) throw new FileNotFoundException("Textos configurados não encontrados.");
if (new[] { ui.Title, ui.Help, ui.StandardLabel, ui.VipLabel }.Any(value => string.IsNullOrWhiteSpace(value) || value.Length > 500))
    throw new InvalidOperationException("Textos devem conter de 1 a 500 caracteres.");
app.MapGet("/api/ui", () => Results.Ok(ui));
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
app.MapGet("/api/run", (HttpContext context) =>
{
    context.Response.Headers.CacheControl = "no-store";
    var path = Environment.GetEnvironmentVariable("SQUAD_RUN");
    if (string.IsNullOrWhiteSpace(path)) return Results.NotFound(new { error = "Defina SQUAD_RUN com o caminho de report.json do runner." });
    if (Directory.Exists(path)) path = Path.Combine(path, "report.json");
    return File.Exists(path) ? Results.Text(File.ReadAllText(path), "application/json") : Results.NotFound(new { error = "Relatório ainda não disponível." });
});
app.Run();
record UiText(string Title, string Help, string StandardLabel, string VipLabel);
