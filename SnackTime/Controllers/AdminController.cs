using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnackTime.Data;
using SnackTime.Models;
using SnackTime.ViewModels;

namespace SnackTime.Controllers;

public class AdminController(DatabaseContext context) : Controller
{
    private readonly DatabaseContext _context = context;

    [Authorize(Roles = "Admin")]
    public IActionResult Users()
    {
        var viewModel = new EditUserViewModel
        {
            AvailableRoles = _context.Roles.ToList()
        };
        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {
        if (model.Email != null && model.SelectedRoleName != null)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null) return RedirectToAction("Users");
            var role = _context.Roles.FirstOrDefault(r => r.Name == model.SelectedRoleName);
            if (role == null) return RedirectToAction("Users");
            user.Role = role;
            _context.Entry(user).State = EntityState.Modified;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Users");
    }
}