namespace SampleShop;

public sealed class OrderService
{
    // A regra de desconto é aplicada antes de calcular o total do pedido.
    public decimal CalculateTotal(decimal subtotal, decimal discountPercent)
    {
        if (subtotal < 0) throw new ArgumentOutOfRangeException(nameof(subtotal));
        DiscountPolicy.Validate(discountPercent);
        return subtotal * (1 - discountPercent / 100);
    }
}
