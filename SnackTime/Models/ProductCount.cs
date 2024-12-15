namespace SnackTime.Models;

public class ProductCount
{
    public uint Identifier { get; set; }
    public Product Product { get; set; }
    public uint Count { get; set; }
    public ICollection<Addon> AddonsUsed { get; set; }
    public Basket Basket { get; set; }
    public uint BasketIdentifier { get; set; }

    public decimal GetTotalPrice()
    {
        return Product.Price * Count;
    }
}