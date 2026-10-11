var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});
builder.WebHost.UseUrls(builder.Configuration["urls"] ?? "http://127.0.0.1:8087");
var app = builder.Build();
app.MapGet("/", async () => Results.Content(
    await File.ReadAllTextAsync(Path.Combine(app.Environment.ContentRootPath, "index.html")),
    "text/html; charset=utf-8"));
app.MapPost("/api/discount", (HttpContext context) => DiscountEndpoint.Handle(context));
app.Run();
