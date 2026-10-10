namespace SampleShop;

// Regra de negócio: desconto permitido de zero até 20 por cento.
public static class DiscountPolicy
{
    public static void Validate(decimal discountPercent)
    {
        if (discountPercent < 0 || discountPercent > 20)
            throw new ArgumentOutOfRangeException(nameof(discountPercent),
                "O desconto deve estar entre 0 e 20 por cento.");
    }
}
