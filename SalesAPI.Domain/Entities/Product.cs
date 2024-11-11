namespace SalesAPI.Domain.Entities
{
    public sealed class Product : IEntityBase
    {
        private Product() { }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? LastModified { get; set; }
        public bool Modified { get; set; } = false;
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public static Product Create(string name, decimal price) => new() { Name = name, Price = price };
    }

}
