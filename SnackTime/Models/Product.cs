namespace SnackTime.Models;

public class Product
{
    public uint Identifier { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string ImagePath { get; set; }
    public ICollection<Discount> Discounts { get; set; }
    public ICollection<Addon> AvailableAddons { get; set; }
    public ProductCategory ProductCategory { get; set; }
}