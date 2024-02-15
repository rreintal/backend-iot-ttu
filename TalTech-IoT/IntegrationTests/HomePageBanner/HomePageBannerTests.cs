using System.Net;
using System.Net.Http.Json;
using App.Domain;
using Integration;
using Public.DTO.V1;

namespace NUnitTests.HomePageBanner;

public class HomePageBannerTests
{
    private CustomWebAppFactory<Program>? _factory;
    private const string BASE_URL = "api" + VERSION + "/HomePageBanner";
    private const string VERSION = "/v1";

    [OneTimeSetUp]
    public void Setup()
    {
        _factory = new CustomWebAppFactory<Program>();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _factory!.Dispose();
    }

    [Test, Order(1)]
    public async Task AddHomePageBanner_ValidData_ReturnsOK()
    {
        var client = _factory!.CreateClient();
        var data = CreateHomePageBanner();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(2)]
    public async Task AddHomePageBanner_InvalidData_ReturnsBadRequest()
    {
        var data = new Public.DTO.V1.HomePageBanner()
        {
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Value = "aaa",
                    Culture = "ooo"
                }
            }
        };
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test, Order(3)]
    public async Task DeleteHomePageBanner_ValidData_ReturnsOK()
    {
        var client = _factory!.CreateClient();
        var data = CreateHomePageBanner();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.HomePageBanner>();
        Assert.NotNull(responseData);

        var deleteResponse = await client.DeleteAsync($"{BASE_URL}/{responseData!.Id}");
        Assert.NotNull(deleteResponse);
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(4)]
    public async Task DeleteHomePageBanner_InvalidId_ReturnsNotFound()
    {
        var client = _factory!.CreateClient();
        var deleteResponse = await client.DeleteAsync($"{BASE_URL}/{Guid.NewGuid()}");
        Assert.NotNull(deleteResponse);
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
    [Test, Order(5)]
    public async Task UpdateHomePageBanner_Et_ReturnsCorrectTitle()
    {
        var TitleInEstonian = "updated title in estonian";
        var client = _factory!.CreateClient();
        var data = CreateHomePageBanner();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.HomePageBanner>();
        Assert.NotNull(responseData);
        
        var updateData = CreateHomePageBanner(TitleInEstonian: TitleInEstonian);
        updateData.Id = responseData!.Id;

        var updateResponse = await client.PutAsJsonAsync($"{BASE_URL}", updateData);
        Assert.NotNull(updateResponse);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedData = await client.GetAsync($"{BASE_URL}/Preview/{updateData.Id}");
        Assert.NotNull(getUpdatedData);
        Assert.That(getUpdatedData.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var updatedDataTitle = updateData.Title.FirstOrDefault(content => content.Culture == LanguageCulture.EST);
        Assert.NotNull(updatedDataTitle);
        Assert.That(updatedDataTitle!.Value, Is.EqualTo(TitleInEstonian));
    }
    [Test, Order(6)]
    public async Task UpdateHomePageBanner_Et_ReturnsCorrectBody()
    {
        var BodyInEstonain = "updated body in estonian";
        var client = _factory!.CreateClient();
        var data = CreateHomePageBanner();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.HomePageBanner>();
        Assert.NotNull(responseData);
        
        var updateData = CreateHomePageBanner(BodyInEstonian: BodyInEstonain);
        updateData.Id = responseData!.Id;

        var updateResponse = await client.PutAsJsonAsync($"{BASE_URL}", updateData);
        Assert.NotNull(updateResponse);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedData = await client.GetAsync($"{BASE_URL}/Preview/{updateData.Id}");
        Assert.NotNull(getUpdatedData);
        Assert.That(getUpdatedData.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var updatedDataTitle = updateData.Body.FirstOrDefault(content => content.Culture == LanguageCulture.EST);
        Assert.NotNull(updatedDataTitle);
        Assert.That(updatedDataTitle!.Value, Is.EqualTo(BodyInEstonain));
    }
    
    [Test, Order(7)]
    public async Task UpdateHomePageBanner_En_ReturnsCorrectTitle()
    {
        var TitleInEnglish = "updated title in english";
        var client = _factory!.CreateClient();
        var data = CreateHomePageBanner();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.HomePageBanner>();
        Assert.NotNull(responseData);
        
        var updateData = CreateHomePageBanner(TitleInEnglish: TitleInEnglish);
        updateData.Id = responseData!.Id;

        var updateResponse = await client.PutAsJsonAsync($"{BASE_URL}", updateData);
        Assert.NotNull(updateResponse);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedData = await client.GetAsync($"{BASE_URL}/Preview/{updateData.Id}");
        Assert.NotNull(getUpdatedData);
        Assert.That(getUpdatedData.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var updatedDataTitle = updateData.Title.FirstOrDefault(content => content.Culture == LanguageCulture.ENG);
        Assert.NotNull(updatedDataTitle);
        Assert.That(updatedDataTitle!.Value, Is.EqualTo(TitleInEnglish));
    }
    
    [Test, Order(8)]
    public async Task UpdateHomePageBanner_En_ReturnsCorrectBody()
    {
        var BodyInEnglish = "updated body in english";
        var client = _factory!.CreateClient();
        var data = CreateHomePageBanner();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.HomePageBanner>();
        Assert.NotNull(responseData);
        
        var updateData = CreateHomePageBanner(BodyInEnglish: BodyInEnglish);
        updateData.Id = responseData!.Id;

        var updateResponse = await client.PutAsJsonAsync($"{BASE_URL}", updateData);
        Assert.NotNull(updateResponse);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedData = await client.GetAsync($"{BASE_URL}/Preview/{updateData.Id}");
        Assert.NotNull(getUpdatedData);
        Assert.That(getUpdatedData.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var updatedDataTitle = updateData.Body.FirstOrDefault(content => content.Culture == LanguageCulture.ENG);
        Assert.NotNull(updatedDataTitle);
        Assert.That(updatedDataTitle!.Value, Is.EqualTo(BodyInEnglish));
    }

    private Public.DTO.V1.HomePageBanner CreateHomePageBanner(
        string TitleInEstonian = "title in estonian",
        string BodyInEstonian = "body in estonian",
        string TitleInEnglish = "title in english",
        string BodyInEnglish = "body in english",
        string Image = "invalid image string"
    )
    {
        return new Public.DTO.V1.HomePageBanner()
        {
            Image = Image,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Value = BodyInEstonian,
                    Culture = LanguageCulture.EST
                },
                new ContentDto()
                {
                    Value = BodyInEnglish,
                    Culture = LanguageCulture.ENG
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Value = TitleInEstonian,
                    Culture = LanguageCulture.EST
                },
                new ContentDto()
                {
                    Value = TitleInEnglish,
                    Culture = LanguageCulture.ENG
                }
            }
        };
    }
    
}