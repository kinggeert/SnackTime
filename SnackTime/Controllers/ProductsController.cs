using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SnackTime.Data;
using SnackTime.Models;
using SnackTime.ViewModels;

namespace SnackTime.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private readonly DatabaseContext _context;

        public ProductsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(e => e.ProductCategory).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Identifier == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var categories = _context.ProductCategories.ToList();
            var viewModel = new CreateProductViewModel
            {
                AvailableCategories = categories
            };
            return View(viewModel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (model.Product != null)
            {
                var category = _context.ProductCategories.Find(model.Product.ProductCategory.Identifier); 
                model.Product.ProductCategory = category;
                _context.Add(model.Product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var categories = _context.ProductCategories.ToList();
            model.AvailableCategories = categories;
            return View(model);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            
            var categories = _context.ProductCategories.ToList();
            var viewModel = new CreateProductViewModel
            {
                AvailableCategories = categories,
                Product = product
            };
            return View(viewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, CreateProductViewModel model)
        {
            if (model.Product == null || id != model.Product.Identifier)
            {
                return NotFound();
            }

            if (model.Product != null)
            {
                try
                {
                    var category = _context.ProductCategories.Find(model.Product.ProductCategory.Identifier); 
                    model.Product.ProductCategory = category;
                    var product = _context.Products.Find(id);
                    product.ProductCategory = category;
                    product.Name = model.Product.Name;
                    product.Price = model.Product.Price;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(model.Product.Identifier))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var categories = _context.ProductCategories.ToList();
            model.AvailableCategories = categories;
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Identifier == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(uint id)
        {
            return _context.Products.Any(e => e.Identifier == id);
        }
    }
}
