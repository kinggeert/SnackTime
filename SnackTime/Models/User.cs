namespace SnackTime.Models;

public class User
{
    public uint Identifier { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<Order> Orders { get; set; }
    public Role Role { get; set; }
    public Basket Basket { get; set; }
    public uint? BasketIdentifier { get; set; }
}