namespace SnackTime.Models;

public class User
{
    public uint Identifier { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public List<Order> Orders { get; set; }
    public Role Role { get; set; }
}