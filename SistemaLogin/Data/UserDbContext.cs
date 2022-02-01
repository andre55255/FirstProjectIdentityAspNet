using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaLogin.Models;

namespace SistemaLogin.Data
{
    public class UserDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public DbSet<Log> Logs { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> opt) : base(opt)
        {
        }
    }
}
