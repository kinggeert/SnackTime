using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnackTime.Data;
using SnackTime.Models;

namespace SnackTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersApiController(DatabaseContext context) : ControllerBase
    {
        private readonly DatabaseContext _context = context;
        
        // GET: api/<OrdersApiController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            return _context.Orders
                .ToList();
        }

        // GET api/<OrdersApiController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
        {
            var order = _context.Orders
                .Include(e => e.Products)
                .ThenInclude(e => e.Product)
                .Include(e => e.Products)
                .ThenInclude(e => e.AddonsUsed)
                .FirstOrDefault(e => e.Identifier == id);
            if (order == null) return NotFound();
            return order;
        }
        
    }
}
