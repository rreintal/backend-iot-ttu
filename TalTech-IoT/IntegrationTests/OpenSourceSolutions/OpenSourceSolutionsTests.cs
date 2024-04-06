using System.Net;
using System.Net.Http.Json;
using App.Domain;
using Integration;
using Public.DTO.V1;
using Public.DTO.V1.OpenSourceSolution;
using OpenSourceSolution = Public.DTO.V1.OpenSourceSolution.OpenSourceSolution;

namespace NUnitTests.OpenSourceSolutions;

public class OpenSourceSolutionsTests
{
    private const string BASE_URL = $"api/{VERSION}OpenSourceSolution";
    private const string VERSION = "v1/";

    private const string PREVIEW = "/Preview";
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
    public async Task AddOSS_ValidData_ReturnsOk()
    {
        var payload = CreateOpenSourceSolution();
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(1)]
    public async Task AddOSS_MissingLink_ReturnsBadRequest()
    {
        var payload = CreateOpenSourceSolution(link: null);
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test, Order(2)]
    public async Task DeleteOSS_ValidId_ReturnsOK()
    {
        var payload = CreateOpenSourceSolution();
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var postResponse = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.NotNull(postResponse);
        Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await postResponse.Content.ReadFromJsonAsync<OpenSourceSolution>();
        Assert.NotNull(data);
        var deleteResponse = await client.DeleteAsync($"{BASE_URL}/{data!.Id}");
        Assert.NotNull(deleteResponse);
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(3)]
    public async Task DeleteOSS_invalidId_ReturnsNotFound()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var deleteResponse = await client.DeleteAsync($"{BASE_URL}/{Guid.NewGuid()}");
        Assert.NotNull(deleteResponse);
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
    [Test, Order(4)]
    public async Task PreviewOSS_invalidId_ReturnsNotFound()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var previewResponse = await client.GetAsync($"{BASE_URL}{PREVIEW}/{Guid.NewGuid()}");
        Assert.NotNull(previewResponse);
        Assert.That(previewResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
    [Test, Order(5)]
    public async Task PreviewOSS_ValidId_ReturnsCorrectData()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var titleEt = "estonian title";
        var titleEn = "english title";
        var bodyEt = "estonian body";
        var bodyEn = "english body";
        var link = "www.google.com";
        var isPrivate = false;
        var payload = CreateOpenSourceSolution(
            link: link, 
            estonianTitle: titleEt,
            estonianBody: bodyEt,
            englishTitle: titleEn,
            englishBody: bodyEn,
            Private: isPrivate);

        var postResponse = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.NotNull(postResponse);
        Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var postData = await postResponse.Content.ReadFromJsonAsync<OpenSourceSolution>();
        Assert.NotNull(postData);
        var previewResponse = await client.GetAsync($"{BASE_URL}{PREVIEW}/{postData!.Id}");
        Assert.NotNull(previewResponse);
        Assert.That(previewResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var previewData = await previewResponse.Content.ReadFromJsonAsync<OpenSourceSolution>();
        Assert.NotNull(previewData);
        var responseBodyEt = previewData!.Body.FirstOrDefault(x => x.Culture == LanguageCulture.EST);
        var responseBodyEn = previewData.Body.FirstOrDefault(x => x.Culture == LanguageCulture.ENG);
        var responseTitleEt = previewData.Title.FirstOrDefault(x => x.Culture == LanguageCulture.EST);
        var responseTitleEn = previewData.Title.FirstOrDefault(x => x.Culture == LanguageCulture.ENG);
        Assert.NotNull(responseBodyEt);
        Assert.NotNull(responseBodyEn);
        Assert.NotNull(responseTitleEt);
        Assert.NotNull(responseTitleEn);
        
        Assert.That(previewData.Link, Is.EqualTo(link));
        Assert.That(previewData.Private, Is.EqualTo(isPrivate));
        Assert.That(responseBodyEt!.Value, Is.EqualTo(bodyEt));
        Assert.That(responseBodyEn!.Value, Is.EqualTo(bodyEn));
        Assert.That(responseTitleEn!.Value, Is.EqualTo(titleEn));
        Assert.That(responseTitleEt!.Value, Is.EqualTo(titleEt));
    }
    
    [Test, Order(6)]
    public async Task GetOSS_Et_ReturnsCorrectTitle()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.EST;
        var estonianTitle = "this is really cool title in estonian";
        var payload = CreateOpenSourceSolution(estonianTitle: estonianTitle);
        
        var postResponse = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.NotNull(postResponse);
        Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await postResponse.Content.ReadFromJsonAsync<OpenSourceSolution>();
        Assert.NotNull(data);

        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{data!.Id}");
        var getResponseData = await getResponse.Content.ReadFromJsonAsync<GetOpenSourceSolution>();
        Assert.NotNull(getResponse);
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        Assert.NotNull(getResponseData);
        Assert.That(getResponseData!.Title, Is.EqualTo(estonianTitle));
    }
    
    [Test, Order(7)]
    public async Task GetOSS_Et_ReturnsCorrectBody()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.EST;
        var estonianBody = "this is really cool body in estonian";
        var payload = CreateOpenSourceSolution(estonianBody: estonianBody);
        
        var postResponse = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.NotNull(postResponse);
        Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await postResponse.Content.ReadFromJsonAsync<OpenSourceSolution>();
        Assert.NotNull(data);

        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{data!.Id}");
        var getResponseData = await getResponse.Content.ReadFromJsonAsync<GetOpenSourceSolution>();
        Assert.NotNull(getResponse);
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        Assert.NotNull(getResponseData);
        Assert.That(getResponseData!.Body, Is.EqualTo(estonianBody));
    }
    
    [Test, Order(8)]
    public async Task GetOSS_En_ReturnsCorrectTitle()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.ENG;
        var englishTitle = "this is really cool title in english";
        var payload = CreateOpenSourceSolution(englishTitle: englishTitle);
        
        var postResponse = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.NotNull(postResponse);
        Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await postResponse.Content.ReadFromJsonAsync<OpenSourceSolution>();
        Assert.NotNull(data);

        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{data!.Id}");
        var getResponseData = await getResponse.Content.ReadFromJsonAsync<GetOpenSourceSolution>();
        Assert.NotNull(getResponse);
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        Assert.NotNull(getResponseData);
        Assert.That(getResponseData!.Title, Is.EqualTo(englishTitle));
    }
    
    [Test, Order(9)]
    public async Task GetOSS_En_ReturnsCorrectBody()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.ENG;
        var englishBody = "this is really cool body in english";
        var payload = CreateOpenSourceSolution(englishBody: englishBody);
        
        var postResponse = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.NotNull(postResponse);
        Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await postResponse.Content.ReadFromJsonAsync<OpenSourceSolution>();
        Assert.NotNull(data);

        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{data!.Id}");
        var getResponseData = await getResponse.Content.ReadFromJsonAsync<GetOpenSourceSolution>();
        Assert.NotNull(getResponse);
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        Assert.NotNull(getResponseData);
        Assert.That(getResponseData!.Body, Is.EqualTo(englishBody));
    }
    
    [Test, Order(10)]
    public async Task UpdateOSS_Et_ReturnsCorrectData()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.EST;
        var titleEt = "this is title in et";
        var bodyEt = "this is body in et";
        var link = "www.link.ee";
        var isPrivate = false;
        var payload = CreateOpenSourceSolution();
        var postResponse = await client.PostAsJsonAsync(BASE_URL, payload);
        
        Assert.NotNull(postResponse);
        Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var data = await postResponse.Content.ReadFromJsonAsync<OpenSourceSolution>();
        Assert.NotNull(data);

        var updateData =
            CreateOpenSourceSolution(link: link, estonianTitle: titleEt, estonianBody: bodyEt, Private: isPrivate);
        updateData.Id = data!.Id;

        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updateData);
        Assert.NotNull(updateResponse);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedDataResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{updateData.Id}");
        Assert.NotNull(getUpdatedDataResponse);
        Assert.That(getUpdatedDataResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var updatedData = await getUpdatedDataResponse.Content.ReadFromJsonAsync<GetOpenSourceSolution>();
        Assert.NotNull(updateData);
        
        Assert.That(titleEt, Is.EqualTo(updatedData!.Title));
        Assert.That(bodyEt, Is.EqualTo(updatedData.Body));
        Assert.That(link, Is.EqualTo(updatedData.Link));
        Assert.That(isPrivate, Is.EqualTo(updatedData.Private));
    }
    
    [Test, Order(11)]
    public async Task UpdateOSS_En_ReturnsCorrectData()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.ENG;
        var titleEn = "this is title in en";
        var bodyEn = "this is body in en";
        var link = "www.link-en.ee";
        var isPrivate = false;
        var payload = CreateOpenSourceSolution();
        var postResponse = await client.PostAsJsonAsync(BASE_URL, payload);
        
        Assert.NotNull(postResponse);
        Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var data = await postResponse.Content.ReadFromJsonAsync<OpenSourceSolution>();
        Assert.NotNull(data);

        var updateData =
            CreateOpenSourceSolution(link: link, englishTitle: titleEn, englishBody: bodyEn, Private: isPrivate);
        updateData.Id = data!.Id;

        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updateData);
        Assert.NotNull(updateResponse);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedDataResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{updateData.Id}");
        Assert.NotNull(getUpdatedDataResponse);
        Assert.That(getUpdatedDataResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var updatedData = await getUpdatedDataResponse.Content.ReadFromJsonAsync<GetOpenSourceSolution>();
        Assert.NotNull(updateData);
        
        Assert.That(titleEn, Is.EqualTo(updatedData!.Title));
        Assert.That(bodyEn, Is.EqualTo(updatedData.Body));
        Assert.That(link, Is.EqualTo(updatedData.Link));
        Assert.That(isPrivate, Is.EqualTo(updatedData.Private));
    }

    private OpenSourceSolution CreateOpenSourceSolution(
        string? link = "www.cool-link.com",
        string? estonianTitle = "title in estonian",
        string? estonianBody = "body in estonian",
        string? englishTitle = "title in english",
        string? englishBody = "body in english",
        bool? Private = true
        )
    {
        return new OpenSourceSolution()
        {
            Private = Private!.Value,
            Link = link!,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: estonianTitle!),
                new(culture: LanguageCulture.ENG, value: englishTitle!),
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: estonianBody!),
                new(culture: LanguageCulture.ENG, value: englishBody!),
            },
        };
    }
    
    
}