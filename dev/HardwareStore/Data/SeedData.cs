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


            if (context == null || context.Title == null || context.Roles == null)
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


            if (context.Title.Any())
            {
                return;
            }
            else
            {
                context.Title.AddRange(
                    new Title { Name = "Кухонные вытяжки" },
                    new Title { Name = "Духовые шкафы" },
                    new Title { Name = "Варочные поверхности" },
                    new Title { Name = "Микроволновые печи" },
                    new Title { Name = "Водонагреватели" },
                    new Title { Name = "Вентиляторы" },
                    new Title { Name = "Посудомоечные машины" },
                    new Title { Name = "Стиральные машины" },
                    new Title { Name = "Посуда" },
                    new Title { Name = "Холодильники" },
                    new Title { Name = "Очистители воздуха" }
                    );
            }

            context.SaveChanges();
        }
    }
}
