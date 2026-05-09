namespace StockFlow.API.Models;

public class StockMovement
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty; // IN / OUT
    public int Quantity { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int UserId { get; set; } 
    public User? User { get; set; }
}