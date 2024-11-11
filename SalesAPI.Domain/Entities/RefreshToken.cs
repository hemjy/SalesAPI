namespace SalesAPI.Domain.Entities
{
    public sealed class RefreshToken : IEntityBase
    {
        private RefreshToken() { }
        public string UserId { get; set; }  
        public User User { get; set; }  
        public string Token { get; set; }   
        public DateTime ExpirationDate { get; set; }
        public bool IsRevoked { get; set; } 
        public Guid Id { get ; set ; } = Guid.NewGuid();
        public string? CreatedBy { get ; set ; }
        public string? ModifiedBy { get ; set ; }
        public bool Modified { get ; set ; }
        public DateTime Created { get ; set ; } = DateTime.UtcNow;
        public DateTime? LastModified { get ; set ; }
        public bool IsDeleted { get ; set ; } = false;

        public static RefreshToken Create(string UserId, string refreshToken, DateTime expiration) => new() { 
            UserId = UserId,
            ExpirationDate = expiration,
            Token = refreshToken
        };
    }
}
