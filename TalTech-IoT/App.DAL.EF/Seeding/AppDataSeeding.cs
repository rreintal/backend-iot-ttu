using App.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.DAL.EF.Seeding;

public class AppDataSeeding
{
    static void SetupAppData(IApplicationBuilder app, IWebHostEnvironment environment, IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
        if (configuration.GetValue<bool>("DataInit:DropDatabase"))
        {
            context!.Database.EnsureDeleted();
        }
        if (configuration.GetValue<bool>("DataInit:Migrate"))
        {
            context!.Database.Migrate();
        }
        if (configuration.GetValue<bool>("DataInit:Seed"))
        {
            var count = context!.ContentTypes.ToList().Count;
            if (count == 0)
            {
                var t1 = new ContentType()
                {
                    Id = Guid.NewGuid(),
                    Name = "BODY"
                };
                var t2 = new ContentType()
                {
                    Id = Guid.NewGuid(),
                    Name = "TITLE"
                };
                context!.ContentTypes.Add(t1);
                context!.ContentTypes.Add(t2);
            }

            var areasCount = context.TopicAreas.ToList().Count;

            context.SaveChanges();
        }
    }
}