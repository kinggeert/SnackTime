using SnackTime.Models;

namespace SnackTime.ViewModels;

public class HomeViewModel
{
    public IEnumerable<ProductCategory> Categories { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public Basket Basket { get; set; }
}