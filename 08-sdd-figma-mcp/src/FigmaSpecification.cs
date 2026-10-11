using System.Text;
using System.Text.Json;

public static class FigmaSpecification
{
    public static bool IsFixture(JsonElement capture) => capture.GetProperty("mode").GetString() == "offline-fixture";

    public static string Text(JsonElement capture)
    {
        var parts = new[] { "metadata", "designContext" }.Select(name =>
        {
            var value = capture.GetProperty(name);
            var texts = value.TryGetProperty("content", out var content)
                ? content.EnumerateArray().Where(block => block.TryGetProperty("type", out var type) && type.GetString() == "text")
                    .Select(block => block.GetProperty("text").GetString()).ToArray() : [];
            var text = string.Join("\n", texts);
            if (string.IsNullOrWhiteSpace(text) && value.TryGetProperty("structuredContent", out var structured)) text = structured.GetRawText();
            if (string.IsNullOrWhiteSpace(text)) throw new InvalidOperationException($"Contexto textual de {name} ausente.");
            return $"## {name}\n{text}";
        });
        return string.Join("\n\n", parts);
    }

    public static void ValidateCapture(JsonElement capture, bool allowFixture)
    {
        var mode = capture.GetProperty("mode").GetString();
        if (mode is not ("figma-mcp" or "offline-fixture") || (mode == "offline-fixture" && !allowFixture))
            throw new InvalidOperationException("Fonte não é uma captura MCP real; fixture só pode rodar em modo offline.");
        _ = Text(capture);
        if (mode == "figma-mcp")
        {
            if (string.IsNullOrWhiteSpace(capture.GetProperty("nodeId").GetString())) throw new InvalidOperationException("Node real ausente.");
            var screenshot = capture.GetProperty("screenshot");
            if (!screenshot.TryGetProperty("content", out var content) || !content.EnumerateArray().Any(IsImage))
                throw new InvalidOperationException("Screenshot real não recebido; aprovação visual bloqueada.");
            foreach (var image in content.EnumerateArray().Where(IsImage)) _ = ImageBytes(image);
        }
    }

    private static bool IsImage(JsonElement block) => block.TryGetProperty("type", out var type) && type.GetString() == "image"
        && block.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(data.GetString());

    private static byte[] ImageBytes(JsonElement block)
    {
        var bytes = Convert.FromBase64String(block.GetProperty("data").GetString()!);
        var mime = block.GetProperty("mimeType").GetString();
        var valid = mime switch {
            "image/png" => bytes.AsSpan().StartsWith(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }),
            "image/jpeg" => bytes.AsSpan().StartsWith(new byte[] { 255, 216, 255 }),
            "image/webp" => bytes.Length >= 12 && Encoding.ASCII.GetString(bytes, 0, 4) == "RIFF"
                            && Encoding.ASCII.GetString(bytes, 8, 4) == "WEBP",
            _ => false };
        if (!valid || bytes.Length > FigmaMcpClient.MaxResponseBytes) throw new InvalidOperationException("Screenshot inválido ou formato não suportado.");
        return bytes;
    }

    public static void WriteVisualSource(JsonElement capture, string output)
    {
        var fixture = IsFixture(capture);
        var imageNames = new List<string>();
        if (!fixture)
        {
            foreach (var block in capture.GetProperty("screenshot").GetProperty("content").EnumerateArray().Where(IsImage))
            {
                var extension = block.GetProperty("mimeType").GetString() switch {
                    "image/png" => "png", "image/jpeg" => "jpg", "image/webp" => "webp",
                    _ => throw new InvalidOperationException("Formato de screenshot não suportado.") };
                var bytes = ImageBytes(block);
                var name = $"figma-screenshot-{imageNames.Count + 1}.{extension}";
                File.WriteAllBytes(Path.Combine(output, name), bytes);
                imageNames.Add(name);
            }
        }
        var markdown = new StringBuilder("# Especificação visual — fonte MCP\n\n");
        markdown.AppendLine(fixture ? "**FIXTURE FICTÍCIA: não veio do Figma; não aprova implementação visual.**"
                                   : $"Fonte: Figma MCP. Node: {capture.GetProperty("nodeId").GetString()}. Revise a captura e o arquivo original.");
        markdown.AppendLine("\n" + Text(capture));
        foreach (var name in imageNames) markdown.AppendLine($"\n![Screenshot retornado pelo MCP]({name})");
        markdown.AppendLine("\n## Gate visual\n- [ ] Contexto de alta fidelidade e frames completos conferidos.\n" +
            "- [ ] Tokens, componentes, estados e responsividade conferidos no Figma.\n- [ ] Front renderizado comparado ao screenshot real.\n" +
            "- [ ] Regras de negócio e contrato validados separadamente.\n");
        File.WriteAllText(Path.Combine(output, "SPEC.visual-source.md"), markdown.ToString());
    }
}
