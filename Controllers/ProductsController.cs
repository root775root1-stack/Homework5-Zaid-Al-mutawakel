
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Models;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound(new { message = "Product not found." });

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> Create(ProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product
        );
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductRequest request)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound(new { message = "Product not found." });

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound(new { message = "Product not found." });

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Product deleted successfully." });
    }
}

public class ProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}
