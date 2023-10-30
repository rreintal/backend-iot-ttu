using App.DAL.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Integration;

public class TestStartup
{
    public IConfiguration Configuration { get; }

    public TestStartup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("TestDbConnection") ??
                               throw new InvalidOperationException("Connection string not found");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            //options.UseNpgsql("Server=localhost:5432;Database=iot-ttu-test;Username=postgres;Password=postgres;");
        });
    }
    
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Additional test-specific app configuration if needed
    }
}