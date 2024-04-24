using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    // identity user will add things like password and password confirm automatically so we just need to inherit it
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios {get;set;} = new List<Portfolio>();
    }
}