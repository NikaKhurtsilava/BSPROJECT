using FirstProjectTest.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstProjectTest.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

        public DbSet<Wallet> Wallet { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Wallet>()
                .Property(w => w.CurrentBalance)
                .HasColumnType("decimal(18,2)");
        }

    }
}