using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnackTime.Data;
using SnackTime.Models;
using SnackTime.ViewModels;

namespace SnackTime.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DatabaseContext _context;

    public HomeController(ILogger<HomeController> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index(uint? id)
    {
        Basket basket = null;
        if (id == null) id = 1;
        var products = _context.Products
            .Include(e => e.AvailableAddons)
            .ToList();
        var categories = _context.ProductCategories.ToList();
        
        var claimIdentifier = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claimIdentifier != null)
        {
            var userIdentifier = uint.Parse(claimIdentifier.Value);
            var user = _context.Users.FirstOrDefault(e => e.Identifier == userIdentifier);
            if (user == null) return NotFound();
            basket = _context.Baskets
                .Include(e => e.Products)
                .ThenInclude(e => e.Product)
                .Include(e => e.Products)
                .ThenInclude(e => e.AddonsUsed)
                .FirstOrDefault(e => e.Identifier == user.BasketIdentifier);
            if (basket == null) return NotFound();
        }
        
        var selectedCategory =
            _context.ProductCategories
                .Include(e => e.ProductsInCategory)
                .FirstOrDefault(e => e.Identifier == id);
        
        var homeViewModel = new HomeViewModel
        {
            Products = products,
            Categories = categories,
            Basket = basket,
            SelectedCategory = selectedCategory
        };
        return View(homeViewModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddProductToBasket(HomeViewModel model)
    {
        if (model.Basket ==null) return BadRequest();
        var selectedAddons = new List<Addon>();
        foreach (uint addonIdentifier in model.SelectedAddons)
        {
            var addon = _context.Addons.FirstOrDefault(e => e.Identifier == addonIdentifier);
            if (addon != null) selectedAddons.Add(addon);
        }
        var product = _context.Products.FirstOrDefault(e => e.Identifier == model.ProductToAdd.Product.Identifier);
        if (product == null) return NotFound();
        var productCount = new ProductCount
        {
            AddonsUsed = selectedAddons,
            Product = product,
            Count = model.ProductToAdd.Count,
        };
        
        var basket = _context.Baskets.First(e => e.Identifier == model.Basket.Identifier);
        basket.Products.Add(productCount);
        
        _context.Baskets.Update(basket);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SubmitOrder(HomeViewModel model)
    {
        if (model.Basket == null) return BadRequest();

        var basket = await _context.Baskets
            .Include(b => b.Products)
            .ThenInclude(pc => pc.Product)
            .Include(b => b.Products)
            .ThenInclude(pc => pc.AddonsUsed)
            .Include(b => b.Owner)
            .FirstOrDefaultAsync(b => b.Identifier == model.Basket.Identifier);

        if (basket == null)
        {
            return BadRequest("Basket not found.");
        }

        // Create new ProductCount entities for the order
        var products = basket.Products.Select(pc => new ProductCount
        {
            Product = pc.Product,
            Count = pc.Count,
            AddonsUsed = pc.AddonsUsed
        }).ToList();

        var order = new Order
        {
            Products = products,
            Owner = basket.Owner,
            Status = Order.OrderStatus.Pending,
            OrderTime = DateTime.Now,
            TableNumber = model.TableNumber
        };
        
        basket.Products.Clear();

        _context.Orders.Add(order);
        _context.Baskets.Update(basket);

        // Commit the transaction
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}