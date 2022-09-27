using Microsoft.AspNetCore.Identity;

namespace ETradingSystem1.WebApi.Authentication
{
    public class ApplicationRole : IdentityRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        
    }
}
