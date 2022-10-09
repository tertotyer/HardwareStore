using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HardwareStore.Models;

namespace HardwareStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HardwareStore.Models.Entity> Entity { get; set; }
        public DbSet<HardwareStore.Models.Category> Category { get; set; }
        public DbSet<HardwareStore.Models.Thing> Thing { get; set; }
        public DbSet<HardwareStore.Models.Image> Image { get; set; }
        public DbSet<HardwareStore.Models.Characteristic> Characteristic { get; set; }
        public DbSet<HardwareStore.Models.Order> Order { get; set; }
        public DbSet<HardwareStore.Models.CartItem> OrderItem { get; set; }
    }
}