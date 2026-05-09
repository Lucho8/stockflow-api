using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockFlow.API.Data;
using StockFlow.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace StockFlow.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock() =>
        Ok(await _context.Products
            .Where(p => p.Stock <= p.MinStock)
            .Include(p => p.Category)
            .ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product updated)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return NotFound();

        product.Name = updated.Name;
        product.SKU = updated.SKU;
        product.Description = updated.Description;
        product.Price = updated.Price;
        product.Stock = updated.Stock;
        product.MinStock = updated.MinStock;
        product.CategoryId = updated.CategoryId;
        product.SupplierId = updated.SupplierId;
        await _context.SaveChangesAsync();
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}