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

    
    public DateTime? OrderTime { get; set; }
    public OrderStatus Status { get; set; }
}