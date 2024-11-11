using Microsoft.AspNetCore.Identity;

namespace SalesAPI.Domain.Entities
{
    public class User : IdentityUser
    {
        
        public string Email { get; set; }
    }
}
