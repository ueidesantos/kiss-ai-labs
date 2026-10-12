using System.Globalization;
using System.Text.Json;
namespace Squad;

public sealed record Policy(decimal StandardMax, decimal VipMax);
public sealed record DiscountResult(string Original, string Discount, string Total, string CustomerType, decimal MaxDiscount);
public sealed record Check(string Name, bool Passed, string Detail);

public static class Discount
{
    public static void Validate(Policy policy)
    {
        if (policy.StandardMax < 0 || policy.StandardMax > 100 || policy.VipMax < 0 || policy.VipMax > 100)
            throw new ArgumentException("Limites devem estar entre 0 e 100.");
    }
    public static DiscountResult Calculate(JsonElement input, Policy policy)
    {
        Validate(policy);
        if (input.ValueKind != JsonValueKind.Object || input.EnumerateObject().Count() != 3
            || !input.TryGetProperty("amount", out var a) || !input.TryGetProperty("discount", out var d)
            || !input.TryGetProperty("customerType", out var c)) throw new ArgumentException("Informe amount, discount e customerType.");
        if (a.ValueKind != JsonValueKind.Number || !a.TryGetDecimal(out var amount)
            || d.ValueKind != JsonValueKind.Number || !d.TryGetDecimal(out var percent)) throw new ArgumentException("Valores devem ser números JSON.");
        if (amount <= 0 || amount > 1_000_000_000m) throw new ArgumentException("Valor deve ser positivo e no máximo 1 bilhão.");
        if (c.ValueKind != JsonValueKind.String || c.GetString() is not ("standard" or "vip")) throw new ArgumentException("Perfil deve ser standard ou vip.");
        var type = c.GetString()!;
        var max = type == "standard" ? policy.StandardMax : policy.VipMax;
        if (percent < 0 || percent > max) throw new ArgumentException($"Desconto permitido para {type}: 0 a {max}%.");
        string Money(decimal value) => decimal.Round(value, 2, MidpointRounding.AwayFromZero).ToString("F2", CultureInfo.InvariantCulture);
        return new(Money(amount), percent.ToString(CultureInfo.InvariantCulture), Money(amount * (1 - percent / 100)), type, max);
    }
    // Immutable acceptance oracle; models cannot modify these cases.
    public static List<Check> Verify(Policy policy)
    {
        var results = new List<Check> { new("Limite Padrão exato", policy.StandardMax == 10m, "Esperado: 10%"), new("Limite VIP exato", policy.VipMax == 20m, "Esperado: 20%") };
        void Case(string name, string json, string? expected)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var actual = Calculate(doc.RootElement, policy);
                results.Add(new(name, expected != null && actual.Total == expected, $"total={actual.Total}; expected={expected ?? "rejection"}"));
            }
            catch (ArgumentException) { results.Add(new(name, expected == null, "Entrada rejeitada")); }
        }
        Case("Padrão 10%", "{\"amount\":100,\"discount\":10,\"customerType\":\"standard\"}", "90.00");
        Case("Padrão rejeita 15%", "{\"amount\":100,\"discount\":15,\"customerType\":\"standard\"}", null);
        Case("Padrão rejeita 10.01%", "{\"amount\":100,\"discount\":10.01,\"customerType\":\"standard\"}", null);
        Case("VIP 15%", "{\"amount\":100,\"discount\":15,\"customerType\":\"vip\"}", "85.00");
        Case("VIP 20%", "{\"amount\":100,\"discount\":20,\"customerType\":\"vip\"}", "80.00");
        Case("VIP rejeita 20.01%", "{\"amount\":100,\"discount\":20.01,\"customerType\":\"vip\"}", null);
        Case("Perfil desconhecido", "{\"amount\":100,\"discount\":10,\"customerType\":\"admin\"}", null);
        Case("Valor zero", "{\"amount\":0,\"discount\":10,\"customerType\":\"vip\"}", null);
        Case("Desconto negativo", "{\"amount\":100,\"discount\":-1,\"customerType\":\"vip\"}", null);
        Case("Arredondamento", "{\"amount\":1.005,\"discount\":0,\"customerType\":\"vip\"}", "1.01");
        Case("Número como texto", "{\"amount\":\"100\",\"discount\":1,\"customerType\":\"vip\"}", null);
        Case("Perfil ausente", "{\"amount\":100,\"discount\":1}", null);
        Case("Campo extra", "{\"amount\":100,\"discount\":1,\"customerType\":\"vip\",\"admin\":true}", null);
        Case("Limite de valor", "{\"amount\":1000000001,\"discount\":1,\"customerType\":\"vip\"}", null);
        return results;
    }
}

