namespace SalesAPI.Application.DTOs.SalesOrders
{
    public class GetHighestSoldProductDto
    {
        public string HighestPricedProduct { get; set; } = string.Empty;
        public decimal? HighestPrice { get; set; } = 0;
        public string HighestSoldProduct { get; set; } = string.Empty ;
        public int? HighestSoldProductQuantity { get; set; } = 0;
    }
}
