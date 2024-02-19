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

    [Test, Order(5)]
    public async Task UpdateContactPerson_Et_ReturnsCorrectBody()
    {
        var LanguageCulture = App.Domain.LanguageCulture.EST;
        var Name = "First Last";
        var BodyInEnglish = "This is body in english";
        var BodyInEstonain = "This is body in estonian";
        var data = GetContactPerson(Name, BodyInEnglish, BodyInEstonain);
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.ContactPerson>();
        Assert.NotNull(responseData);

        var UpdatedBodyInEstonian = "this is the new updated body in estonian";
        
        var updatedData = GetContactPerson(Name, BodyInEnglish, UpdatedBodyInEstonian);
        updatedData.Id = responseData!.Id;

        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.NotNull(updateResponse);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var getUpdatedData = await client.GetAsync($"{BASE_URL}/{LanguageCulture}/{updatedData.Id}");
        Assert.NotNull(getUpdatedData);
        Assert.That(getUpdatedData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var getUpdateResponseData = await getUpdatedData.Content.ReadFromJsonAsync<Public.DTO.V1.GetContactPerson>();
        Assert.That(getUpdateResponseData!.Body, Is.EqualTo(UpdatedBodyInEstonian));
        
    }
    
    [Test, Order(5)]
    public async Task UpdateContactPerson_En_ReturnsCorrectBody()
    {
        var LanguageCulture = App.Domain.LanguageCulture.ENG;
        var Name = "First Last";
        var BodyInEnglish = "This is body in english";
        var BodyInEstonain = "This is body in estonian";
        var data = GetContactPerson(Name, BodyInEnglish, BodyInEstonain);
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.ContactPerson>();
        Assert.NotNull(responseData);

        var UpdatedBodyInEnglish = "this is the new updated body in english";
        
        var updatedData = GetContactPerson(Name, UpdatedBodyInEnglish, BodyInEstonain);
        updatedData.Id = responseData!.Id;

        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.NotNull(updateResponse);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var getUpdatedData = await client.GetAsync($"{BASE_URL}/{LanguageCulture}/{updatedData.Id}");
        Assert.NotNull(getUpdatedData);
        Assert.That(getUpdatedData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var getUpdateResponseData = await getUpdatedData.Content.ReadFromJsonAsync<Public.DTO.V1.GetContactPerson>();
        Assert.That(getUpdateResponseData!.Body, Is.EqualTo(UpdatedBodyInEnglish));
    }
    
    [Test, Order(5)]
    public async Task UpdateContactPerson_ReturnsCorrectName()
    {
        var LanguageCulture = App.Domain.LanguageCulture.ENG;
        var Name = "First Last";
        var BodyInEnglish = "This is body in english";
        var BodyInEstonain = "This is body in estonian";
        var data = GetContactPerson(Name, BodyInEnglish, BodyInEstonain);
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.ContactPerson>();
        Assert.NotNull(responseData);
        var UpdatedName = "Last First";
        var updatedData = GetContactPerson(UpdatedName, BodyInEnglish, BodyInEstonain);
        updatedData.Id = responseData!.Id;

        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.NotNull(updateResponse);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var getUpdatedData = await client.GetAsync($"{BASE_URL}/{LanguageCulture}/{updatedData.Id}");
        Assert.NotNull(getUpdatedData);
        Assert.That(getUpdatedData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var getUpdateResponseData = await getUpdatedData.Content.ReadFromJsonAsync<Public.DTO.V1.GetContactPerson>();
        Assert.That(getUpdateResponseData!.Name, Is.EqualTo(UpdatedName));
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