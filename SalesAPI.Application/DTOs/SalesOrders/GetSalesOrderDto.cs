

namespace SalesAPI.Application.DTOs.SalesOrders
{
    public class GetSalesOrderDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount => UnitPrice * Quantity;
        public DateTime OrderDate { get; set; }
    }
}
