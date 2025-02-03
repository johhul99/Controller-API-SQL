using Controller_API_SQL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controller_API_SQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public ProductController(ApplicationDBContext Context)
        {
            context = Context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            if(product == null)
            {
                return BadRequest("Invalid JSON data");
            }

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id = product.Id}, product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product == null)
            {
                return NotFound($"Product with id {id} does not exist");
            }
            return Ok(product); 
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await context.Products.ToListAsync();
            if(products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]Product product, int id)
        {
            var productToUpdate = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (productToUpdate == null)
            {
                return NotFound($"Product with Id: {id} does not exist.");
            }

            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;

            await context.SaveChangesAsync();

            return Ok(productToUpdate);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound($"Product with Id: {id} does not exist.");
            }

            context.Products.Remove(product);
            await context.SaveChangesAsync();

            return Ok($"Product with ID {id} has been deleted.");
        }

    }
}
