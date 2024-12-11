namespace SnackTime.Models;

public class Role
{
    public string Name { get; set; }
    public ICollection<User> UsersWithRole { get; set; }
}