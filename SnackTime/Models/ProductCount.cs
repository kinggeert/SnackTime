namespace SnackTime.Models;

public class ProductCount
{
    public uint Identifier { get; set; }
    public Product Product { get; set; }
    public uint Count { get; set; }
    public ICollection<Addon> AddonsUsed { get; set; }

    public decimal GetTotalPrice()
    {
        decimal addonPrice = 0;
        foreach (var addon in AddonsUsed)
        {
            addonPrice += addon.Price;
        }
        return (Product.Price + addonPrice) * Count;
    }
}