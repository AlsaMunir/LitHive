using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace WebApplication6.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserProfile>
    {
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Books> Books { get; set; }
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
