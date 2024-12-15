namespace SnackTime.Models;

public class Order
{
    public enum OrderStatus
    {
        Pending,    // Order submitted but not yet prepared
        Preparing,  // Order is being prepared
        Ready,      // Order is ready for pickup/delivery
        Closed      // Order is completed or closed
    }
    
    public enum OrderType
    {
        Takeout,    // Order is for takeout
        InStore     // Order is for eating in the store
    }

    
    public uint Identifier { get; set; }
    public User Owner { get; set; }
    public ICollection<ProductCount> Products { get; set; } = new List<ProductCount>();
    public DateTime? OrderTime { get; set; }
    public OrderStatus Status { get; set; }
    public OrderType Type { get; set; }
    public uint? TableNumber { get; set; }
    public uint OwnerIdentifier { get; set; }
    
    public decimal GetTotalPrice()
    {
        decimal totalPrice = 0;
        foreach (var product in Products)
        {
            totalPrice += product.GetTotalPrice();
        }
        return totalPrice;
    }
}