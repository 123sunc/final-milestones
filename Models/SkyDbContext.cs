using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SKYResturant.Models
{
    public class SkyDbContext : IdentityDbContext<IdentityUser>
    {
        public SkyDbContext(DbContextOptions<SkyDbContext> options) : base(options)
        {
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<CheckoutCustomer> CheckoutCustomer { get; set; }
        public DbSet<BasketItem> BasketItem { get; set; }
        public DbSet<Basket> Basket { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BasketItem>().HasKey(t => new { t.StockID, t.BasketID });
        }
    }
}
