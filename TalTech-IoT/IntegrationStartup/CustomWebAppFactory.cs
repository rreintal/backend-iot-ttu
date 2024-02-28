using App.DAL.EF;
using App.DAL.EF.Seeding;
using Microsoft.AspNetCore.Builder;
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
    //private readonly IConfiguration _configuration;

    public CustomWebAppFactory()
    {
        // TODO: how to drop Db every time before running tests?
        
        /*
        var projectRootDirectory = AppContext.BaseDirectory;   
        var configuration = new ConfigurationBuilder()
            .SetBasePath(projectRootDirectory)
            .AddJsonFile("appsettings.json") // Use a test-specific configuration file
            .Build();
        _configuration = configuration;
        */
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        
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
                //var app = scopedServices.GetRequiredService<IApplicationBuilder>();
                //var env = scopedServices.GetRequiredService<IWebHostEnvironment>();
                var configuration = scopedServices.GetRequiredService<IConfiguration>();
                var db = scopedServices.GetRequiredService<AppDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebAppFactory<TStartup>>>();

                // Ensure the database is created
                db.Database.EnsureCreated();

                // Seed the database with test data
                try
                {
                    AppDataSeeding.SetupAppData(scopedServices, configuration).Wait();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB with test messages. Error: {Message}", ex.Message);
                }
            };
        });
        /*
        builder.ConfigureServices(services =>
        {
            // find DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));

            // if found - remove
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            var connectionString = _configuration.GetConnectionString("TestDbConnection") ??
                                   throw new InvalidOperationException("Connection string not found");
            // and new DbContext
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            // data seeding
            
            
            // create db and seed data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            var logger = scopedServices
                .GetRequiredService<ILogger<CustomWebAppFactory<TStartup>>>();
        });
        */
    }

}