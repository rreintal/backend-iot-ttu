using App.Domain;
using App.Domain.Constants;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp;

namespace App.DAL.EF.Seeding;

public static class AppDataSeeding
{
    public const string CONTENT_TYPE_BODY_ID = "60c865f7-ceef-4c91-8da6-22e483c6354d";
    public const string CONTENT_TYPE_TITLE_ID = "6dd748e4-bac4-4d5b-aa6c-54641b719938";
    public const string TOPIC_AREA_ROBOTICS_ID = "23e18f8b-0f97-496c-99a3-774f66b8c43d";
    public const string TOPIC_AREA_TECHNOLOGY_ID = "daf06b3e-26f0-4d0e-b68a-6b236ce6a84c";
    
    public static async Task SetupAppData(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        Console.WriteLine("SEEDING PRODUCTION DATA!");
        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<AppDbContext>();
        if (Environment.GetEnvironmentVariable(EnvironmentVariableConstants.DROP_DB) == "true")
        {
            await context!.Database.EnsureDeletedAsync();
        }
        if (Environment.GetEnvironmentVariable(EnvironmentVariableConstants.MIGRATE_DB) == "true")
        {
            if (context.Database.IsRelational()) // This is because, when running tests, in memory db can't migrate :(
            {
                context.Database.Migrate();
            }
        }
        if (Environment.GetEnvironmentVariable(EnvironmentVariableConstants.SEED_DB) == "true")
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
                    .SetValues("4G", "4G");
                var t3child2 = DomainFactory
                    .TopicArea()
                    .SetValues("5G", "5G");
                context.TopicAreas.AddRangeAsync(new List<TopicArea>() { t1, t2, t3, t3child2, t3Child });
            }

            var feedpageCount = context.FeedPages.ToList().Count();
            if (feedpageCount == 0)
            {
                var hardware = new FeedPage()
                {
                    FeedPageName = "HARDWARE"
                };
                var research = new FeedPage()
                {
                    FeedPageName = "RESEARCH"
                };

                context.AddRange(new List<FeedPage>() { hardware, research });
            }

            using var roleManager = scopedServices.GetService<RoleManager<AppRole>>();
            
            var roles = new List<AppRole>()
            {
                new AppRole()
                {
                    Name = IdentityRolesConstants.ROLE_ADMIN
                },
                new AppRole()
                {
                    Name = IdentityRolesConstants.ROLE_MODERATOR
                }
            };
            foreach (var role in roles)
            {
                if (!await roleManager!.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }

            var userManager = scopedServices.GetService<UserManager<AppUser>>()!;
            var usersCount = (await userManager.Users.ToListAsync()).Count;
            if (usersCount == 0)
            {
                
                var adminUser = new AppUser()
                {
                    Firstname = "AdminFn",
                    Lastname = "AdminLn",
                    Email = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.ADMIN_EMAIL),
                    UserName = "AppAdmin"
                };
                await userManager.CreateAsync(adminUser, Environment.GetEnvironmentVariable(EnvironmentVariableConstants.ADMIN_PASSWORD)!);
                await userManager.AddToRoleAsync(adminUser, IdentityRolesConstants.ROLE_ADMIN);

    
                var recipent = new EmailRecipents()
                {
                    Email = "reintalrichard@gmail.com"
                };
                var recipent2 = new EmailRecipents()
                {
                    Email = "edgar.vildt@gmail.com"
                };
                context.EmailRecipents.Add(recipent);
                context.EmailRecipents.Add(recipent2);
            }

            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedTestUsers(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<AppDbContext>();
        
        
        // Content Types
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


        // Topic Areas
        var ta1 = DomainFactory
            .TopicArea()
            .SetValues("Tehnoloogia", "Technology", Guid.Parse(TOPIC_AREA_TECHNOLOGY_ID));
        var ta2 = DomainFactory
            .TopicArea()
            .SetValues("Robootika", "Robotics", Guid.Parse(TOPIC_AREA_ROBOTICS_ID));
        var t3 = DomainFactory
            .TopicArea()
            .SetValues("Arvutivõrgud", "Networking");
        var t3Child = DomainFactory
            .TopicArea()
            .SetValues("4G", "4G");
        var t3child2 = DomainFactory
            .TopicArea()
            .SetValues("5G", "5G");
        await context.TopicAreas.AddRangeAsync(new List<TopicArea>() { ta1, ta2, t3, t3child2, t3Child });

        // Feed pages

        var hardware = new FeedPage()
        {
            FeedPageName = "HARDWARE"
        };
        var research = new FeedPage()
        {
            FeedPageName = "RESEARCH"
        };

        context.AddRange(new List<FeedPage>() { hardware, research });

        // Identity

        using var roleManager = scopedServices.GetService<RoleManager<AppRole>>();

        var roles = new List<AppRole>()
        {
            new(name: IdentityRolesConstants.ROLE_ADMIN),
            new(name: IdentityRolesConstants.ROLE_MODERATOR)
        };

        foreach (var role in roles)
        {
            await roleManager!.CreateAsync(role);
        }

        await context.SaveChangesAsync();

        var userManager = scopedServices.GetService<UserManager<AppUser>>()!;
        var usersCount = (await userManager.Users.ToListAsync()).Count;
        if (usersCount == 0)
        {
            var testAdminUser = new AppUser()
            {
                Id = Guid.NewGuid(), // Some really weird InMemoryDb stuff, that the ID needs to be present
                Firstname = "TestAdminFirstName",
                Lastname = "TestAdminLastName",
                Email = TestConstants.TestAdminEmail,
                UserName = "testAdmin",
            };

            await userManager.CreateAsync(testAdminUser, TestConstants.TestAdminPassword);
            await userManager.AddToRoleAsync(testAdminUser, IdentityRolesConstants.ROLE_ADMIN);

            var testModeratorUser = new AppUser()
            {
                Id = Guid.NewGuid(), // Some really weird InMemoryDb stuff, that the ID needs to be present
                Firstname = "TestModeratorFirstName",
                Lastname = "TestModeratorLastName",
                Email = TestConstants.TestModeratorEmail,
                UserName = "testUser"
            };
            await userManager.CreateAsync(testModeratorUser, TestConstants.TestModeratorPassword);
            await userManager.AddToRoleAsync(testModeratorUser, IdentityRolesConstants.ROLE_MODERATOR);
        }
        
        await context.SaveChangesAsync();
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