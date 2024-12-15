namespace SnackTime.Models;

public class Order : Basket
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

    
    public DateTime? OrderTime { get; set; }
    public OrderStatus Status { get; set; }
    public OrderType Type { get; set; }
    public uint? TableNumber { get; set; }
}