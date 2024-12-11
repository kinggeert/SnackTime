namespace SnackTime.Models;

public class Addon
{
    public uint Identifier { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public List<Product> ApplicableProducts { get; set; }
    public List<Addon> UnavailableWith { get; set; }
}