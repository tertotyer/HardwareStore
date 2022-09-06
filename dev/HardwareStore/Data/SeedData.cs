using HardwareStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HardwareStore.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context =
                new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            if (context == null || context.Entity == null || context.Roles == null)
            {
                throw new ArgumentNullException("Null context");
            }

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new IdentityRole { Name = "user", NormalizedName = "USER" },
                    new IdentityRole { Name = "admin", NormalizedName = "ADMIN" }
                    );
            }

            if (!context.Entity.Any())
            {
                context.Entity.AddRange(
                    new Entity { Name = "Кухонные вытяжки" },
                    new Entity { Name = "Духовые шкафы" },
                    new Entity { Name = "Варочные поверхности" },
                    new Entity { Name = "Микроволновые печи" },
                    new Entity { Name = "Водонагреватели" },
                    new Entity { Name = "Вентиляторы" },
                    new Entity { Name = "Посудомоечные машины" },
                    new Entity { Name = "Стиральные машины" },
                    new Entity { Name = "Посуда" },
                    new Entity { Name = "Холодильники" },
                    new Entity { Name = "Очистители воздуха" }
                    );
            }

            context.SaveChanges();
        }
    }
}
