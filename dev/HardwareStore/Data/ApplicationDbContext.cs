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
        public DbSet<HardwareStore.Models.Title> Title { get; set; }
        public DbSet<HardwareStore.Models.Entity> Entity { get; set; }
        public DbSet<HardwareStore.Models.Thing> Thing { get; set; }
        public DbSet<HardwareStore.Models.Image> Image { get; set; }
        public DbSet<HardwareStore.Models.Characteristic> Characteristic { get; set; }
    }
}