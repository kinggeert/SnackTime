using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnackTime.Data;
using SnackTime.Models;

namespace SnackTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController(DatabaseContext context) : ControllerBase
    {
        private readonly DatabaseContext _context = context;

        // GET: api/<ProductsApiController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
             return await _context.Products.ToListAsync();
        }

        // GET api/<ProductsApiController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = _context.Products
                .Include(e => e.ProductCategory)
                .Include(e => e.AvailableAddons)
                .First(e => e.Identifier == id);
            return product;
        }
    }
}
