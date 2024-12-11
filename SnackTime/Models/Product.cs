namespace SnackTime.Models;

public class Product
{
    public uint Identifier { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string ImagePath { get; set; }
    public Discount? Discount { get; set; }
    public List<Addon> AvailableAddons { get; set; }
}