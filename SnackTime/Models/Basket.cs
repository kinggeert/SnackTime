namespace SnackTime.Models;

public class Basket
{
    public User Owner { get; set; }
    public List<ProductCount> Products { get; set; }
}