using System.Net;
using System.Net.Http.Json;
using App.BLL.Services.ImageStorageService;
using App.DAL.EF;
using App.DAL.EF.Seeding;
using App.Domain;
using HtmlAgilityPack;
using Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Public.DTO.V1;
using TopicArea = Public.DTO.V1.TopicArea;


namespace NUnitTests.ImageServiceTests;

public class ImageServiceTests
{
    private CustomWebAppFactory<Program>? _factory;
    private const string VERSION = "v1";
    private string BASE_URL => $"api/{VERSION}/";
    private string NEWS_URL => BASE_URL + "News?test=true";
    private string DELETE_NEWS_URL(string id) => BASE_URL + $"News/{id}/?test=true";
    private string GET_NEWS_ALL_LANGUAGES(string id) => BASE_URL + $"News/Preview/{id}";
    
    private AppDbContext? _dbContext;
    private ImageExtractor? _imageExtractor;
    
    // ****
    // Image locations
    private string Image1 => GetImagePath("Image1.txt");
    private string Image2 => GetImagePath("Image2.txt");
    // ****
    

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = new CustomWebAppFactory<Program>();
        var scope = _factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _imageExtractor = new ImageExtractor();
    }
    
    [Test, Order(0)]
    public void TestImageAccess()
    {
        ReadFileAsString(Image1);
        ReadFileAsString(Image2);
    }
    

    [Test, Order(1)]
    public async Task AddNews_CorrectAmountOfImageResources()
    {
        var client = _factory!.CreateClient();

        var Image1Base64 = ReadFileAsString(Image1);
        
        var payload = new PostNewsDto()
        {
            Author = "Richard Reintal",
            Title = new List<ContentDto>()
            {
                new(value: "Title", culture: LanguageCulture.EST),
                new(value: "Title", culture: LanguageCulture.ENG)
            },
            Body = new List<ContentDto>()
            {
                new(value: MakeImageTag(Image1Base64), culture: LanguageCulture.EST),
                new(value: MakeImageTag(Image1Base64), culture: LanguageCulture.ENG)
            },
            TopicAreas = new List<TopicArea>()
            {
                new(id: Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID))
            },
            Image = Image1Base64
        };
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(NEWS_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
        Assert.NotNull(data);
        var imageResources = await _dbContext!.ImageResources.Where(e => e.NewsId == data!.Id).ToListAsync();
        Assert.That(imageResources.Count, Is.EqualTo(4));
    }
    
    [Test, Order(2)]
    public async Task UpdateNews_UpdatesImageResourcesCorrectly()
    {
        var client = _factory!.CreateClient();

        var Image1Base64 = ReadFileAsString(Image1);
        var Image2Base64 = ReadFileAsString(Image2);
        
        var payload = new PostNewsDto()
        {
            Author = "Richard Reintal",
            Title = new List<ContentDto>()
                {
                    new(value: "Title", culture: LanguageCulture.EST),
                    new(value: "Title", culture: LanguageCulture.ENG)
                },
            Body = new List<ContentDto>()
            {
                new(value: MakeImageTag(Image1Base64), culture: LanguageCulture.EST),
                new(value: MakeImageTag(Image1Base64), culture: LanguageCulture.ENG)
            },
            TopicAreas = new List<TopicArea>()
            {
                new(id: Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID))
            },
            Image = Image1Base64
        };
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(NEWS_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
        Assert.NotNull(data);
        var imageResources = await _dbContext!.ImageResources.Where(e => e.NewsId == data!.Id).ToListAsync();
        Assert.That(imageResources.Count, Is.EqualTo(4));
        
        var updatedPayload = new UpdateNews()
        {
            Id = data!.Id,
            Author = "Richard Reintal",
            Image = Image1Base64, 
            Title = new List<ContentDto>()
            {
                new(value: "Title", culture: LanguageCulture.EST),
                new(value: "Title", culture: LanguageCulture.ENG)
            },
            Body = new List<ContentDto>()
            {
                new(value: MakeImageTag(Image2Base64), culture: LanguageCulture.EST),
                new(value: MakeImageTag(Image2Base64), culture: LanguageCulture.ENG)
            },
            TopicAreas = new List<TopicArea>()
            {
                new(id: Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID))
            }
        };
        var updateResponse = await client.PutAsJsonAsync(NEWS_URL, updatedPayload);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var updatedImageResources =
            await _dbContext!.ImageResources.Where(e => e.NewsId == data!.Id).ToListAsync();
        Assert.That(updatedImageResources.Count, Is.EqualTo(4));

    }
    
    [Test, Order(3)]
    public async Task DeleteNews_DeletesAllImageResources()
    {
        var client = _factory!.CreateClient();

        var Image1Base64 = ReadFileAsString(Image1);
        
        var payload = new PostNewsDto()
        {
            Author = "Richard Reintal",
            Title = new List<ContentDto>()
            {
                new(value: "Title", culture: LanguageCulture.EST),
                new(value: "Title", culture: LanguageCulture.ENG)
            },
            Body = new List<ContentDto>()
            {
                new(value: MakeImageTag(Image1Base64), culture: LanguageCulture.EST),
                new(value: MakeImageTag(Image1Base64), culture: LanguageCulture.ENG)
            },
            TopicAreas = new List<TopicArea>()
            {
                new(id: Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID))
            },
            Image = Image1Base64
        };
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(NEWS_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
        Assert.NotNull(data);
        var imageResources = await _dbContext!.ImageResources.Where(e => e.NewsId == data!.Id).ToListAsync();
        Assert.That(imageResources.Count, Is.EqualTo(4));

        var deleteResponse = await client.DeleteAsync(DELETE_NEWS_URL(data!.Id.ToString()));
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var imageResourcesAfterDelete = await _dbContext!.ImageResources.Where(e => e.Id == Guid.NewGuid()).ToListAsync();
        Assert.That(imageResourcesAfterDelete.Count, Is.EqualTo(0));
    }

    [Test, Order(4)]
    public async Task AddNews_ReplacesImageBase64()
    {
        var client = _factory!.CreateClient();
        
        var Image2Base64 = ReadFileAsString(Image2);
        
        var payload = new PostNewsDto()
        {
            Author = "Richard Reintal",
            Title = new List<ContentDto>()
            {
                new(value: "Title", culture: LanguageCulture.EST),
                new(value: "Title", culture: LanguageCulture.ENG)
            },
            Body = new List<ContentDto>()
            {
                new(value: MakeImageTag(Image2Base64), culture: LanguageCulture.EST),
                new(value: MakeImageTag(Image2Base64), culture: LanguageCulture.ENG)
            },
            TopicAreas = new List<TopicArea>()
            {
                new(id: Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID))
            },
            Image = Image2Base64
        };
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(NEWS_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
        Assert.NotNull(data);

        var IsImageBase64 = _imageExtractor!.IsBase64String(data!.Image);
        Assert.False(IsImageBase64);
    }

    [Test, Order(5)]
    public async Task AddNews_ReplacesBodyBase64()
    {
        var client = _factory!.CreateClient();
        
        var Image2Base64 = ReadFileAsString(Image2);
        
        var payload = new PostNewsDto()
        {
            Author = "Richard Reintal",
            Title = new List<ContentDto>()
            {
                new(value: "Title", culture: LanguageCulture.EST),
                new(value: "Title", culture: LanguageCulture.ENG)
            },
            Body = new List<ContentDto>()
            {
                new(value: MakeImageTag(Image2Base64), culture: LanguageCulture.EST),
                new(value: MakeImageTag(Image2Base64), culture: LanguageCulture.ENG)
            },
            TopicAreas = new List<TopicArea>()
            {
                new(id: Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID))
            },
            Image = Image2Base64
        };
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(NEWS_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
        Assert.NotNull(data);

        var NewsId = data!.Id.ToString();
        var allLanguagesResponse = await client.GetAsync(GET_NEWS_ALL_LANGUAGES(NewsId));
        Assert.That(allLanguagesResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var allLanguagesData = await allLanguagesResponse.Content.ReadFromJsonAsync<NewsAllLangs>();
        Assert.NotNull(allLanguagesData);
        // Check if any img tag has base64
        var bodyContainsBase64 = CheckIfBodyImageTagsContainsBase64(allLanguagesData);
        Assert.False(bodyContainsBase64);
    }
    
    // *****
    // Helpers
    // *****
    public static string ReadFileAsString(string path)
    {
        try
        {
            string fileContents = File.ReadAllText(path);
            return fileContents;
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"File on path {path} not found!");
        }
    }

    public static bool CheckIfBodyImageTagsContainsBase64(NewsAllLangs news)
    {
        HtmlDocument document = new HtmlDocument();
        ImageExtractor imageExtractor = new ImageExtractor();
        foreach (var bodyHTML in news.Body)
        {
            document.LoadHtml(bodyHTML.Value);
            var imgElements = document.DocumentNode.Descendants("img").ToList();
            foreach (var imgElement in imgElements)
            {
                var defaultValue = "default value";
                var imgTagSrcValue = imgElement.GetAttributeValue("src", defaultValue);
                if (imageExtractor.IsBase64String(imgTagSrcValue) && imgTagSrcValue != defaultValue)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static string GetImagePath(string imageName)
    {
        string pathToBase = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", ".."));
        string relativePathToImage = Path.Combine(pathToBase, "ImageServiceTests", "Base64Images", imageName);
        return relativePathToImage;
    }

    private static string MakeImageTag(string base64)
    {
        return $"<img src=\"{base64}\"\\>";
    }
}