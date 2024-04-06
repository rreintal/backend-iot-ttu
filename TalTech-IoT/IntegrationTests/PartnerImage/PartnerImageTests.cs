using System.Net;
using System.Net.Http.Json;
using Integration;

namespace NUnitTests.PartnerImage;

public class PartnerImageTests
{
    private CustomWebAppFactory<Program>? _factory;
    private const string BASE_URL = "api" + VERSION + "/PartnerImage";
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
    public async Task AddPartnerImage_ValidData_ReturnsOk()
    {
        var data = new Public.DTO.V1.PartnerImage()
        {
            Image = "cool image src",
            Link = "cool link"
        };
        
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test, Order(2)]
    public async Task AddPartnerImage_MissingImage_ReturnsBadRequest()
    {
        var data = new Public.DTO.V1.PartnerImage()
        {
            Link = "cool link"
        };
        
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test, Order(3)]
    public async Task AddPartnerImage_MissingLink_ReturnsOK()
    {
        var data = new Public.DTO.V1.PartnerImage()
        {
            Image = "image without link"
        };
        
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(6)]
    public async Task DeletePartnerImage_InvalidId_ReturnsNotFound()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var deleteResponse = await client.DeleteAsync($"{BASE_URL}/{Guid.NewGuid()}");
        Assert.NotNull(deleteResponse);
        Assert.That(deleteResponse!.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
    [Test, Order(7)]
    public async Task DeletePartnerImage_ValidId_ReturnsOk()
    {
        var data = new Public.DTO.V1.PartnerImage()
        {
            Image = "cool image src",
            Link = "cool link"
        };
        
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.PartnerImage>();
        Assert.NotNull(responseData);
        var deleteResponse = await client.DeleteAsync($"{BASE_URL}/{responseData!.Id}");
        Assert.NotNull(deleteResponse);
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }




}