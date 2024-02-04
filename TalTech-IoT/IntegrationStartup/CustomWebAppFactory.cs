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
    private readonly IConfiguration _configuration;

    public CustomWebAppFactory()
    {
        // TODO: how to drop Db every time before running tests?
        
        var projectRootDirectory = AppContext.BaseDirectory;   
        var configuration = new ConfigurationBuilder()
            .SetBasePath(projectRootDirectory)
            .AddJsonFile("appsettings.json") // Use a test-specific configuration file
            .Build();
        _configuration = configuration;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
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
    }

}