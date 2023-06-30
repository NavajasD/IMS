using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IMS.WebApp.Identity
{
    public class AccountDbContext : IdentityDbContext
    {
        public AccountDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
