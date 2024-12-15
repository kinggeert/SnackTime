using SnackTime.Models;

namespace SnackTime.ViewModels;

public class HomeViewModel
{
    public IEnumerable<ProductCategory> Categories { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public Basket? Basket { get; set; }
    public ProductCategory SelectedCategory { get; set; }
    public ProductCount ProductToAdd { get; set; }
    public Order.OrderType SelectedOrderType { get; set; }
    public uint? TableNumber { get; set; }
}