using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnackTime.Data;
using SnackTime.Models;

namespace SnackTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesApiController(DatabaseContext context) : ControllerBase
    {
        private readonly DatabaseContext _context = context;
        
        // GET: api/<CategoriesApiController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> Get()
        {
            return _context.ProductCategories
                .Include(e => e.ProductsInCategory)
                .ToList();
        }

        // GET api/<CategoriesApiController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> Get(int id)
        {
            var productCategory = _context.ProductCategories
                .FirstOrDefault(e => e.Identifier == id);
            if (productCategory == null) return NotFound();
            return productCategory;
        }
        
    }
}
