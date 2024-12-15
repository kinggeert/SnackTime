using SnackTime.Models;

namespace SnackTime.ViewModels;

public class OrdersViewModel
{
    public ICollection<Order> Orders { get; set; }
    public uint? OrderToUpdateIdentifier { get; set; }
}