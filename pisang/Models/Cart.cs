namespace pisang.Models;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PisangId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}