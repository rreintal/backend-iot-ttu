using System.Net;
using System.Text;
using App.DAL.EF;
using App.DAL.EF.Seeding;
using App.Domain;
using Factory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Integration;

public class NewsTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public NewsTests(ITestOutputHelper testOutputHelper, CustomWebAppFactory<Program> factory)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
    }
    
    
    // TODO : Populate db before running tests!!
    // maybe add a sequence to run tests or smth?!
    [Fact]
    public async void AddNews()
    {
        var topicArea = PublicFactory
            .TopicArea(Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID));

        var newsDto = PublicFactory
            .CreatePostNews()
            .SetAuthor("Richard Reintal")
            .SetContent(ContentTypes.TITLE, new Dictionary<string, string>()
            {
                { "eesti123 title", LanguageCulture.EST },
                { "english title", LanguageCulture.ENG },
            })
            .SetContent(ContentTypes.BODY, new Dictionary<string, string>()
            {
                { "eesti body", LanguageCulture.EST },
                { "english body", LanguageCulture.ENG },
            })
            .SetTopicArea(topicArea)
            .SetImage("empty");

        var jsonNewsDto = JsonSerializer.Serialize(newsDto);

        var client = _factory.CreateClient();
        
        var content = new StringContent(jsonNewsDto, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync("/api/news", content);
        
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async void AddNewsWithoutTopicArea()
    {
        var newsDto = PublicFactory
            .CreatePostNews()
            .SetAuthor("Richard Reintal")
            .SetContent(ContentTypes.TITLE, new Dictionary<string, string>()
            {
                { "eesti title", LanguageCulture.EST },
                { "english title", LanguageCulture.ENG },
            })
            .SetContent(ContentTypes.BODY, new Dictionary<string, string>()
            {
                { "eesti body", LanguageCulture.EST },
                { "english body", LanguageCulture.ENG },
            })
            .SetImage("empty");
        
        var jsonNewsDto = JsonSerializer.Serialize(newsDto);
        _testOutputHelper.WriteLine(jsonNewsDto);
        
        var client = _factory.CreateClient();
        
        var content = new StringContent(jsonNewsDto, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync("/api/news", content);
        
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async void AddNewsWithoutAuthor()
    {
        var newsDto = PublicFactory
            .CreatePostNews()
            .SetContent(ContentTypes.TITLE, new Dictionary<string, string>()
            {
                { "eesti title", LanguageCulture.EST },
                { "english title", LanguageCulture.ENG },
            })
            .SetContent(ContentTypes.BODY, new Dictionary<string, string>()
            {
                { "eesti body", LanguageCulture.EST },
                { "english body", LanguageCulture.ENG },
            })
            .SetImage("empty");
        
        var jsonNewsDto = JsonSerializer.Serialize(newsDto);
        _testOutputHelper.WriteLine(jsonNewsDto);
        
        var client = _factory.CreateClient();
        
        var content = new StringContent(jsonNewsDto, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync("/api/news", content);
        
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async void GetAllNews()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync("api/et/news/");
        // Assert
        response.EnsureSuccessStatusCode(); // Ensure that the response has a 2xx status code.
    }
    
    [Fact]
    public async void GetAllProjects()
    {
        // TODO - maybe create another db just for testing?!
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync("api/et/project/");
        // Assert
        response.EnsureSuccessStatusCode(); // Ensure that the response has a 2xx status code.
    }
    
    [Fact]
    public async void GetAllTopicAreas()
    {
        // TODO - maybe create another db just for testing?!
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync("api/et/TopicAreas/");
        // Assert
        response.EnsureSuccessStatusCode(); // Ensure that the response has a 2xx status code.
    }
}