namespace SalesAPI.Domain.Entities
{
    public sealed class SalesOrder : IEntityBase
    {
        private SalesOrder() { }
        public Guid Id { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public bool Modified { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsDeleted { get; set; }
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Product Product { get; set; } 

       public static SalesOrder Create(Guid productId, Guid customerId, int quantity, decimal unitPrice) => new() 
       {
           ProductId = productId, CustomerId = customerId, Quantity = quantity, UnitPrice = unitPrice,
           Id = Guid.NewGuid(),
           Created = DateTime.UtcNow, IsDeleted = false
       };
    }

}
