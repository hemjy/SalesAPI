namespace SalesAPI.Application.DTOs.Products
{
    public class GetProductDto
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; } 
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
