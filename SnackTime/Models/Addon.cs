namespace SnackTime.Models;

public class Addon
{
    public uint Identifier { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ICollection<Product> ApplicableProducts { get; set; }
    public ICollection<Addon> UnavailableWith { get; set; }
    public ICollection<ProductCount> UsedInProductCounts { get; set; }
}