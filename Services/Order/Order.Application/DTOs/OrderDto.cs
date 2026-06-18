namespace Order.Application.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; } = string.Empty;
}