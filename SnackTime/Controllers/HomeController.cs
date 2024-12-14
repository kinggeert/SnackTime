using System.Diagnostics;
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
        if (id == null) id = 1;
        var products = _context.Products.ToList();
        var categories = _context.ProductCategories.ToList();
        //temp
        var basket = new Basket
        {
            Products = new List<ProductCount>()
        };
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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}