using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.DAL.EF.Seeding;

public static class AppDataSeeding
{
    public const string CONTENT_TYPE_BODY_ID = "60c865f7-ceef-4c91-8da6-22e483c6354d";
    public const string CONTENT_TYPE_TITLE_ID = "6dd748e4-bac4-4d5b-aa6c-54641b719938";
    public const string TOPIC_AREA_ROBOTICS_ID = "23e18f8b-0f97-496c-99a3-774f66b8c43d";
    public const string TOPIC_AREA_TECHNOLOGY_ID = "daf06b3e-26f0-4d0e-b68a-6b236ce6a84c";
    
    public static async Task SetupAppData(IApplicationBuilder app, IWebHostEnvironment environment, IConfiguration configuration)
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
                    Id = GetContentTypeId(ContentTypes.BODY),
                    Name = ContentTypes.BODY
                };
                var t2 = new ContentType()
                {
                    Id = GetContentTypeId(ContentTypes.TITLE),
                    Name = ContentTypes.TITLE
                };
                context.ContentTypes.Add(t1);
                context.ContentTypes.Add(t2);
            }

            var areasCount = context.TopicAreas.ToList().Count;

            if (areasCount == 0)
            {
                var t1 = DomainFactory
                    .TopicArea()
                    .SetValues("Tehnoloogia", "Technology", Guid.Parse(TOPIC_AREA_TECHNOLOGY_ID));
                var t2 = DomainFactory
                    .TopicArea()
                    .SetValues("Robootika", "Robotics", Guid.Parse(TOPIC_AREA_ROBOTICS_ID));
                var t3 = DomainFactory
                    .TopicArea()
                    .SetValues("Arvutivõrgud", "Networking");
                var t3Child = DomainFactory
                    .TopicArea()
                    .SetValues("4G", "4G")
                    .SetParent(t3);
                var t3child2 = DomainFactory
                    .TopicArea()
                    .SetValues("5G", "5G")
                    .SetParent(t3);
                context.TopicAreas.AddRangeAsync(new List<TopicArea>() { t1, t2, t3, t3child2, t3Child });
            }

            using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();
            
            var roles = new List<AppRole>()
            {
                new AppRole()
                {
                    Name = "ADMIN"
                },
                new AppRole()
                {
                    Name = "USER"
                }
            };
            foreach (var role in roles)
            {
                // TODO: logger if roleManager is null!
                if (!await roleManager!.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }

            var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>()!;
            var usersCount = (await userManager.Users.ToListAsync()).Count;
            if (usersCount == 0)
            {
                var adminUser = new AppUser()
                {
                    Firstname = "AdminFN",
                    Lastname = "AdminLN",
                    Email = "admin@email.ee",
                    UserName = "admin",
                };

                await userManager.CreateAsync(adminUser, "admin");
                await userManager.AddToRoleAsync(adminUser, "ADMIN");   
            }

            await context.SaveChangesAsync();
        }
    }
    
    public static Guid GetContentTypeId(string contentType)
    {
        switch (contentType)
        {
            case ContentTypes.BODY:
                return Guid.Parse(CONTENT_TYPE_BODY_ID);
            case ContentTypes.TITLE:
                return Guid.Parse(CONTENT_TYPE_TITLE_ID);
            default:
                throw new InvalidOperationException("AppDataSeeding:GetContentTypeId - Invalid content type!");
        }
    }
}