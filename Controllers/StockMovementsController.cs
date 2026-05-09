using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockFlow.API.Data;
using StockFlow.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace StockFlow.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StockMovementsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StockMovementsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.StockMovements
            .Include(m => m.Product)
            .Include(m => m.User)
            .ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var movement = await _context.StockMovements
            .Include(m => m.Product)
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        return movement is null ? NotFound() : Ok(movement);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StockMovement movement)
    {
        var product = await _context.Products.FindAsync(movement.ProductId);
        if (product is null) return NotFound("Product not found");

        if (movement.Type == "OUT" && product.Stock < movement.Quantity)
            return BadRequest("Stock insuficiente");

        product.Stock += movement.Type == "IN" ? movement.Quantity : -movement.Quantity;

        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = movement.Id }, movement);
    }
}