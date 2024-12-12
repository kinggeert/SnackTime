namespace SnackTime.Models;

public class ProductCategory
{
    public uint Identifier { get; set; }
    public string Name { get; set; }
    public ICollection<Product> ProductsInCategory { get; set; }
}