using System.Net;
using System.Net.Http.Json;
using App.Domain;
using Integration;
using Public.DTO.V1;

namespace NUnitTests.ContactPerson;

public class ContactPersonTests
{
    private const string BASE_URL = $"api/{VERSION}/ContactPerson";
    private const string VERSION = "v1";
    private CustomWebAppFactory<Program>? _factory;

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

    [Test, Order(0)]
    public async Task AddContactPerson_ValidData_ReturnsOK()
    {
        var data = GetContactPerson();
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(1)]
    public async Task AddContactPerson_MissingName_ReturnsBadRequest()
    {
        var data = new Public.DTO.V1.ContactPerson()
        {
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Value = "this is body value in english",
                    Culture = LanguageCulture.ENG
                },
                new ContentDto()
                {
                    Value = "this is body value in english",
                    Culture = LanguageCulture.EST
                }
            }
        };
        
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test, Order(2)]
    public async Task GetContactPerson_Et_ReturnsCorrectData()
    {
        var Name = "First Last";
        var BodyInEnglish = "This is body in english";
        var BodyInEstonain = "This is body in estonian";
        var data = GetContactPerson(Name, BodyInEnglish, BodyInEstonain);
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.ContactPerson>();
        Assert.NotNull(responseData);

        var getResponse = await client.GetAsync($"{BASE_URL}/{LanguageCulture.EST}/{responseData!.Id}");
        Assert.NotNull(getResponse);
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var getResponseData = await getResponse.Content.ReadFromJsonAsync<Public.DTO.V1.GetContactPerson>();
        Assert.NotNull(getResponseData);
        Assert.That(getResponseData!.Name, Is.EqualTo(Name));
        Assert.That(getResponseData!.Body, Is.EqualTo(BodyInEstonain));
    }
    
    [Test, Order(3)]
    public async Task GetContactPerson_En_ReturnsCorrectData()
    {
        var Name = "First Last";
        var BodyInEnglish = "This is body in english";
        var BodyInEstonain = "This is body in estonian";
        var data = GetContactPerson(Name, BodyInEnglish, BodyInEstonain);
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.ContactPerson>();
        Assert.NotNull(responseData);

        var getResponse = await client.GetAsync($"{BASE_URL}/{LanguageCulture.ENG}/{responseData!.Id}");
        Assert.NotNull(getResponse);
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var getResponseData = await getResponse.Content.ReadFromJsonAsync<Public.DTO.V1.GetContactPerson>();
        Assert.NotNull(getResponseData);
        Assert.That(getResponseData!.Name, Is.EqualTo(Name));
        Assert.That(getResponseData!.Body, Is.EqualTo(BodyInEnglish));
    }
    
    [Test, Order(4)]
    public async Task GetContactPerson_InvalidId_ReturnsNotFound()
    {
        var client = _factory!.CreateClient();
        var response = await client.GetAsync($"{BASE_URL}/{LanguageCulture.ENG}/{Guid.NewGuid()}");
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private Public.DTO.V1.ContactPerson GetContactPerson(string Name = "Default name", string BodyInEnglish = "Body in english",
        string BodyInEstonian = "Body in estonian")
    {
        return new Public.DTO.V1.ContactPerson()
        {
            Name = Name,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = BodyInEnglish
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = BodyInEstonian
                }
            }
        };
    }
    
    
}