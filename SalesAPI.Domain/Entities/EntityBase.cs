namespace SalesAPI.Domain.Entities
{
    public interface IEntityBase
    {
        public Guid Id { get; set; } 
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public bool Modified { get; set; }
        public DateTime Created { get; set; } 
        public DateTime? LastModified { get; set; }
        public bool IsDeleted { get; set; }
    }
}
