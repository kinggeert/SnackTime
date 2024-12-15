using SnackTime.Models;

namespace SnackTime.ViewModels;

public class HistoryViewModel
{
    public ICollection<Order> Orders { get; set; }
    public uint? OrderToAddToBasketIdentifier { get; set; }
}