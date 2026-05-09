using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockFlow.API.Data;
using StockFlow.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace StockFlow.API.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly AppDbContext _context;

    public SuppliersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Suppliers.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        return supplier is null ? NotFound() : Ok(supplier);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Supplier updated)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier is null) return NotFound();

        supplier.Name = updated.Name;
        supplier.Email = updated.Email;
        supplier.Phone = updated.Phone;
        supplier.Address = updated.Address;
        await _context.SaveChangesAsync();
        return Ok(supplier);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier is null) return NotFound();

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}