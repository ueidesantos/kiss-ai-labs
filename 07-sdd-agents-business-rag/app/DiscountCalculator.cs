using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

// REQ-4 / AC-4: strict JSON numbers and the documented business rules.
public static class DiscountCalculator
{
    public static DiscountResult Calculate(JsonElement input)
    {
        if (input.ValueKind != JsonValueKind.Object
            || input.EnumerateObject().Count() != 2
            || !input.TryGetProperty("amount", out var amountElement)
            || !input.TryGetProperty("discount", out var discountElement))
            throw new ArgumentException("Informe somente amount e discount.");

        var amount = Number(amountElement, "amount");
        var discount = Number(discountElement, "discount");
        if (amount <= 0)
            throw new ArgumentException("O valor do pedido deve ser maior que zero.");
        if (discount < 0 || discount > 20)
            throw new ArgumentException("O desconto deve estar entre 0 e 20%.");

        var original = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
        var total = decimal.Round(amount * (1m - discount / 100m), 2, MidpointRounding.AwayFromZero);
        return new DiscountResult(
            original.ToString("F2", CultureInfo.InvariantCulture),
            discount.ToString("0.############################", CultureInfo.InvariantCulture),
            total.ToString("F2", CultureInfo.InvariantCulture));
    }

    private static decimal Number(JsonElement element, string name)
    {
        if (element.ValueKind != JsonValueKind.Number || !element.TryGetDecimal(out var value))
            throw new ArgumentException($"{name} deve ser um número JSON representável como decimal.");
        return value;
    }
}

public sealed record DiscountResult(
    [property: JsonPropertyName("original")] string Original,
    [property: JsonPropertyName("discount")] string Discount,
    [property: JsonPropertyName("total")] string Total);
