using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App.DAL.EF;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        // this is needed for creation and migrations
        // Dev db connection
        //builder.UseNpgsql("Server=localhost:5432;Database=iot-ttu;Username=postgres;Password=postgres;");
        // Docker-compose connection
        builder.UseNpgsql("Server=iot-ttu-db:5432;Database=iot-ttu;Username=postgres;Password=postgres;");
        return new AppDbContext(builder.Options);
    }
}