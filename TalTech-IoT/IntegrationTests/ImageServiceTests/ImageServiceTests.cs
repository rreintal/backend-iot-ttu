using App.DAL.EF;
using Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NUnitTests.ImageServiceTests;

public class ImageServiceTests
{
    private CustomWebAppFactory<Program>? _factory;
    private const string VERSION = "v1";
    private string BASE_URL => $"api/{VERSION}/Users";
    private AppDbContext? _dbContext;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = new CustomWebAppFactory<Program>();
        
        var scope = _factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }
    

    [Test, Order(1)]
    public async Task AddNews_ReplacesAllBase64WithLinks()
    {
    }
    
    [Test, Order(2)]
    public async Task UpdateNews_ReplacesAllBase64WithLinks()
    {
    }
    
    [Test, Order(3)]
    public async Task DeleteNews_DeletesAllImageResources()
    {
        var items = await _dbContext!.ImageResources.Where(e => e.Id == Guid.NewGuid()).ToListAsync();
        Assert.That(items.Count, Is.EqualTo(3));
    }
}