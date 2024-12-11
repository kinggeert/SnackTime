namespace SnackTime.Models;

public class Discount
{
    public uint Identifier { get; set; }
    public Product Product { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? DiscountCode { get; set; }
    public List<Product> ApplicableProducts { get; set; }
}