using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnackTime.Data;
using SnackTime.Models;
using SnackTime.ViewModels;

namespace SnackTime.Controllers;

public class OrdersController(DatabaseContext context) : Controller
{
    private readonly DatabaseContext _context = context;

    [Authorize(Roles = "cook, admin")]
    public IActionResult Index()
    {
        var orders = _context.Orders
            .Include(e => e.Products)
            .ThenInclude(e => e.Product)
            .Include(e => e.Products)
            .ThenInclude(e => e.AddonsUsed)
            .Include(e => e.Owner)
            .OrderBy(e => e.OrderTime)
            .Where(e => e.Status != Order.OrderStatus.Closed)
            .ToList();
        var viewModel = new OrdersViewModel
        {
            Orders = orders
        };
        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "cook, admin")]
    public async Task<IActionResult> SetPreparing(OrdersViewModel model)
    {
        if (model.OrderToUpdateIdentifier == null) return BadRequest();
        var order = _context.Orders.First(e => e.Identifier == model.OrderToUpdateIdentifier);
        if (order.Status == Order.OrderStatus.Preparing)
        {
            order.Status = Order.OrderStatus.Pending;
        }
        else
        {
            order.Status = Order.OrderStatus.Preparing;
        }
        _context.Entry(order).State = EntityState.Modified;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    [Authorize(Roles = "cook, admin")]
    public async Task<IActionResult> SetReady(OrdersViewModel model)
    {
        if (model.OrderToUpdateIdentifier == null) return BadRequest();
        var order = _context.Orders.First(e => e.Identifier == model.OrderToUpdateIdentifier);
        if (order.Status == Order.OrderStatus.Ready)
        {
            order.Status = Order.OrderStatus.Pending;
        }
        else
        {
            order.Status = Order.OrderStatus.Ready;
        }
        _context.Entry(order).State = EntityState.Modified;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    [Authorize(Roles = "cook, admin")]
    public async Task<IActionResult> SetClosed(OrdersViewModel model)
    {
        if (model.OrderToUpdateIdentifier == null) return BadRequest();
        var order = _context.Orders.First(e => e.Identifier == model.OrderToUpdateIdentifier);
        order.Status = Order.OrderStatus.Closed;
        _context.Entry(order).State = EntityState.Modified;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult History()
    {
        var claimIdentifier = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claimIdentifier == null) return NotFound();
        uint userIdentifier = uint.Parse(claimIdentifier.Value);
        var viewModel = new HistoryViewModel
        {
            Orders = _context.Orders
                .Where(e => e.Owner.Identifier == userIdentifier)
                .Include(e => e.Products)
                .ThenInclude(e => e.Product)
                .Include(e => e.Products)
                .ThenInclude(e => e.AddonsUsed)
                .ToList()
        };
        return View(viewModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddOrderToBasket(HistoryViewModel model)
    {
        if (model.OrderToAddToBasketIdentifier != null)
        {
            var claimIdentifier = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claimIdentifier == null) return NotFound();
            
            var basket = _context.Baskets
                .Include(e => e.Products)
                .First(e => e.Owner.Identifier == uint.Parse(claimIdentifier.Value));
            var order = _context.Orders
                .Include(e => e.Products)
                .First(e => e.Identifier == model.OrderToAddToBasketIdentifier);
            
            basket.Products = order.Products;
            
            _context.Entry(basket).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return RedirectToAction("History");
    }
}