using System;

namespace pisang.Models;

public class OrderItems
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int PisangId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}