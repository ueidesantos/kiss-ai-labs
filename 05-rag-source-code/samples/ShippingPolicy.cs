namespace SampleShop;

public static class ShippingPolicy
{
    // Frete grátis a partir de 200 reais; abaixo disso, custa 15 reais.
    public static decimal GetShippingCost(decimal total) => total >= 200 ? 0 : 15;
}
