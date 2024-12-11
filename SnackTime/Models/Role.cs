namespace SnackTime.Models;

public class Role
{
    public string Name { get; set; }
    public List<User> UsersWithRole { get; set; }
}