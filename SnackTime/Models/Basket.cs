namespace SnackTime.Models;

public class Basket
{
    public uint Identifier { get; set; }
    public User Owner { get; set; }
    public ICollection<ProductCount> Products { get; set; } = new List<ProductCount>();

    public decimal GetTotalPrice()
    {
        decimal totalPrice = 0;
        foreach (var product in Products)
        {
            totalPrice += product.GetTotalPrice();
        }
        return totalPrice;
    }
}