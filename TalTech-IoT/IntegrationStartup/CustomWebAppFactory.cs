using App.DAL.EF;
using App.DAL.EF.Seeding;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Integration;

public class CustomWebAppFactory<TStartup> : WebApplicationFactory<TStartup>
where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("testing");
        builder.ConfigureServices(services =>
        {
            // Find the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));

            // If found, remove it
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Register the in-memory database context
            services.AddDbContext<AppDbContext>(options =>
            {
                // Use the in-memory database
                options.UseInMemoryDatabase("InMemoryAppDb");    

                
            });

            // Optionally, you can seed the in-memory database here if needed
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                var configuration = scopedServices.GetRequiredService<IConfiguration>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebAppFactory<TStartup>>>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                
                try
                {
                    logger.LogInformation("Starting to seed test data...");
                    AppDataSeeding.SeedTestUsers(scopedServices).Wait();
                    logger.LogInformation("Seeding test data completed successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB with test data. Error: {Message}", ex.Message);
                }
            };
            
        });
    }

}