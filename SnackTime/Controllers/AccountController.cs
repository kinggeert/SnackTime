using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnackTime.Data;
using SnackTime.Models;
using SnackTime.ViewModels;

namespace SnackTime.Controllers;

public class AccountController : Controller
{
    private readonly DatabaseContext _context;

    public AccountController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users.Include(e => e.Role)
                .SingleOrDefaultAsync(e => e.Email == model.Email);
            if (user != null && user.PasswordHash == model.Password)
            {
                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Identifier.ToString())
                };

                // Create the user's claimsPrincipal
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return RedirectToAction("Index", "Home");
            }
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var role = _context.Roles.FirstOrDefault(e => e.Name == "customer");
            if (role == null) return NotFound();
            var user = new User
            {
                Email = model.Email,
                Name = model.Name,
                PasswordHash = model.Password,
                Role = role,
                Basket = new Basket()
            };

            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
        }

        return View(model);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Profile()
    {
        var claimIdentifier = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claimIdentifier == null) return NotFound();
        var userIdentifier = uint.Parse(claimIdentifier.Value);
        var user = _context.Users.FirstOrDefault(e => e.Identifier == userIdentifier);
        var model = new ProfileViewModel
        {
            UserIdentifier = user.Identifier,
            Email = user.Email,
            Name = user.Name,
            Password = user.PasswordHash
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _context.Users.First(e => e.Identifier == model.UserIdentifier);
            
            user.Email = model.Email;
            user.Name = model.Name;
            user.PasswordHash = model.Password;
            
            _context.Attach(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

}