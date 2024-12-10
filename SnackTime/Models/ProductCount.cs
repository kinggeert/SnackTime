namespace SnackTime.Models;

public class ProductCount
{
    public Product Product { get; set; }
    public uint Count { get; set; }

    public decimal GetTotalPrice()
    {
        return Product.Price * Count;
    }
}