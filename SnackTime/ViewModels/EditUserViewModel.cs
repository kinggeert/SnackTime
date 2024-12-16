using SnackTime.Models;

namespace SnackTime.ViewModels;

public class EditUserViewModel
{
    public string? Email { get; set; }
    public ICollection<Role> AvailableRoles { get; set; }
    public string? SelectedRoleName { get; set; }
}