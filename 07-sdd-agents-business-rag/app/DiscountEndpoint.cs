using System.Text.Json;

public static class DiscountEndpoint
{
    public const int MaxBodyBytes = 4096;

    // Public handler permits HTTP contract tests without network sockets.
    public static async Task Handle(HttpContext context)
    {
        try
        {
            if (context.Request.ContentLength > MaxBodyBytes)
                throw new ArgumentException("O corpo JSON deve ter no máximo 4096 bytes.");

            using var body = new MemoryStream();
            var buffer = new byte[1024];
            int read;
            while ((read = await context.Request.Body.ReadAsync(buffer, context.RequestAborted)) > 0)
            {
                if (body.Length + read > MaxBodyBytes)
                    throw new ArgumentException("O corpo JSON deve ter no máximo 4096 bytes.");
                await body.WriteAsync(buffer.AsMemory(0, read), context.RequestAborted);
            }
            using var document = JsonDocument.Parse(body.ToArray());
            var result = DiscountCalculator.Calculate(document.RootElement);
            await Respond(context, 200, result);
        }
        catch (JsonException)
        {
            await Respond(context, 400, new { error = "Envie um objeto JSON válido com amount e discount." });
        }
        catch (ArgumentException exception)
        {
            await Respond(context, 400, new { error = exception.Message });
        }
        catch (OverflowException)
        {
            await Respond(context, 400, new { error = "Os valores excedem a capacidade do simulador." });
        }
    }

    private static async Task Respond<T>(HttpContext context, int status, T value)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json; charset=utf-8";
        await JsonSerializer.SerializeAsync(context.Response.Body, value, cancellationToken: context.RequestAborted);
    }
}
