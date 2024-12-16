using SnackTime.Models;

namespace SnackTime.ViewModels;

public class CreateProductViewModel
{
    public Product? Product { get; set; }
    public ICollection<ProductCategory>? AvailableCategories { get; set; }
}