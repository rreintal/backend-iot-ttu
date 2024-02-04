using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using App.Domain;
using Integration;
using Public.DTO.V1;

namespace NUnitTests.PageContent;

public class PageContentTests
{
    private CustomWebAppFactory<Program>? _factory;
    private const string BASE_URL = "api" + VERSION + "/PageContent";
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
    public async Task GetPageContent_ReturnsCorrectData()
    {
        var pageIdentifier = Guid.NewGuid().ToString();
        var bodyET = "sisu eesti keeles";
        var bodyEN = "body in english";
        var titleET = "pealkiri eesti keeles";
        var titleEN = "title in english";
        var payload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                }
            }
        };

        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var data = await response.Content.ReadFromJsonAsync<Public.DTO.V1.PageContent>();
        Assert.NotNull(data);
        var responseBodyET = data!.Body.First(content => content.Culture == LanguageCulture.EST).Value;
        var responseBodyEN = data.Body.First(content => content.Culture == LanguageCulture.ENG).Value;
        
        var responseTitleET = data.Title.First(content => content.Culture == LanguageCulture.EST).Value;
        var responseTitleEN = data.Title.First(content => content.Culture == LanguageCulture.ENG).Value;

        Assert.That(responseTitleEN, Is.EqualTo(titleEN));
        Assert.That(responseTitleET, Is.EqualTo(titleET));
        Assert.That(responseBodyEN, Is.EqualTo(bodyEN));
        Assert.That(responseBodyET, Is.EqualTo(bodyET));
    }
    
    [Test, Order(2)]
    public async Task GetPageContent_Et_ReturnsCorrectTitle()
    {
        var languageCulture = LanguageCulture.EST;
        var pageIdentifier = Guid.NewGuid().ToString();
        var bodyET = "sisu eesti keeles";
        var bodyEN = "body in english";
        var titleET = "pealkiri eesti keeles";
        var titleEN = "title in english";
        var payload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                }
            }
        };

        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        
        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{pageIdentifier}");
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await getResponse.Content.ReadFromJsonAsync<Public.DTO.V1.GetPageContent>();
        Assert.NotNull(data);
        Assert.That(data!.Title, Is.EqualTo(titleET));
        
    }
    
    [Test, Order(3)]
    public async Task GetPageContent_En_ReturnsCorrectTitle()
    {
        var languageCulture = LanguageCulture.ENG;
        var pageIdentifier = Guid.NewGuid().ToString();
        var bodyET = "sisu eesti keeles";
        var bodyEN = "body in english";
        var titleET = "pealkiri eesti keeles";
        var titleEN = "title in english";
        var payload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                }
            }
        };

        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{pageIdentifier}");
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await getResponse.Content.ReadFromJsonAsync<Public.DTO.V1.GetPageContent>();
        Assert.NotNull(getResponse);
        Assert.NotNull(data);
        
        Assert.That(data!.Title, Is.EqualTo(titleEN));
    }
    
    [Test, Order(4)]
    public async Task GetPageContent_Et_ReturnsCorrectBody()
    {
        var languageCulture = LanguageCulture.EST;
        var pageIdentifier = Guid.NewGuid().ToString();
        var bodyET = "sisu eesti keeles";
        var bodyEN = "body in english";
        var titleET = "pealkiri eesti keeles";
        var titleEN = "title in english";
        var payload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                }
            }
        };

        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{pageIdentifier}");
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await getResponse.Content.ReadFromJsonAsync<Public.DTO.V1.GetPageContent>();
        Assert.NotNull(data);
        
        Assert.That(data!.Body, Is.EqualTo(bodyET));
    }
    
    [Test, Order(5)]
    public async Task GetPageContent_En_ReturnsCorrectBody()
    {
        var languageCulture = LanguageCulture.ENG;
        var pageIdentifier = Guid.NewGuid().ToString();
        var bodyET = "sisu eesti keeles";
        var bodyEN = "body in english";
        var titleET = "pealkiri eesti keeles";
        var titleEN = "title in english";
        var payload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                }
            }
        };

        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{pageIdentifier}");
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await getResponse.Content.ReadFromJsonAsync<Public.DTO.V1.GetPageContent>();
        Assert.NotNull(data);
        
        Assert.That(data!.Body, Is.EqualTo(bodyEN));
    }

    [Test, Order(6)]
    public async Task UpdatePageContent_En_ReturnsCorrectBody()
    {
        var languageCulture = LanguageCulture.ENG;
        var pageIdentifier = Guid.NewGuid().ToString();
        var bodyET = "sisu eesti keeles";
        var bodyEN = "body in english";
        var titleET = "pealkiri eesti keeles";
        var titleEN = "title in english";
        var payload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                }
            }
        };

        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var updatedEnBody = "this is new body";
        var updatedPayload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = updatedEnBody
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                }
            }
        };
        
        var updateResponse = await client.PutAsJsonAsync($"{BASE_URL}/{pageIdentifier}", updatedPayload);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{pageIdentifier}");
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await getUpdatedResponse.Content.ReadFromJsonAsync<Public.DTO.V1.GetPageContent>();
        Assert.NotNull(data);
        Assert.That(data!.Body, Is.EqualTo(updatedEnBody));
        
    }
    
    [Test, Order(7)]
    public async Task UpdatePageContent_Et_ReturnsCorrectBody()
    {
        var languageCulture = LanguageCulture.EST;
        var pageIdentifier = Guid.NewGuid().ToString();
        var bodyET = "sisu eesti keeles";
        var bodyEN = "body in english";
        var titleET = "pealkiri eesti keeles";
        var titleEN = "title in english";
        var payload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                }
            }
        };

        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var updatedEtBody = "see on uus sisu";
        var updatedPayload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = updatedEtBody
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                }
            }
        };
        
        var updateResponse = await client.PutAsJsonAsync($"{BASE_URL}/{pageIdentifier}", updatedPayload);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{pageIdentifier}");
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await getUpdatedResponse.Content.ReadFromJsonAsync<Public.DTO.V1.GetPageContent>();
        Assert.NotNull(data);
        Assert.That(data!.Body, Is.EqualTo(updatedEtBody));
        
    }
    
    [Test, Order(8)]
    public async Task UpdatePageContent_En_ReturnsCorrectTitle()
    {
        var languageCulture = LanguageCulture.ENG;
        var pageIdentifier = Guid.NewGuid().ToString();
        var bodyET = "sisu eesti keeles";
        var bodyEN = "body in english";
        var titleET = "pealkiri eesti keeles";
        var titleEN = "title in english";
        var payload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                }
            }
        };

        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var updatedEnTitle = "this is new title";
        var updatedPayload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = updatedEnTitle
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                }
            }
        };
        
        var updateResponse = await client.PutAsJsonAsync($"{BASE_URL}/{pageIdentifier}", updatedPayload);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{pageIdentifier}");
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await getUpdatedResponse.Content.ReadFromJsonAsync<Public.DTO.V1.GetPageContent>();
        Assert.NotNull(data);
        Assert.That(data!.Title, Is.EqualTo(updatedEnTitle));
        
    }
    
    [Test, Order(9)]
    public async Task UpdatePageContent_Et_ReturnsCorrectTitle()
    {
        var languageCulture = LanguageCulture.EST;
        var pageIdentifier = Guid.NewGuid().ToString();
        var bodyET = "sisu eesti keeles";
        var bodyEN = "body in english";
        var titleET = "pealkiri eesti keeles";
        var titleEN = "title in english";
        var payload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = titleET
                }
            }
        };

        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync(BASE_URL, payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var updatedEtTitle = "see on uus pealkiri!";
        var updatedPayload = new Public.DTO.V1.PageContent()
        {
            PageIdentifier = pageIdentifier,
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = bodyET
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyEN
                }
            },
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = titleEN
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = updatedEtTitle
                }
            }
        };
        
        var updateResponse = await client.PutAsJsonAsync($"{BASE_URL}/{pageIdentifier}", updatedPayload);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{pageIdentifier}");
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await getUpdatedResponse.Content.ReadFromJsonAsync<Public.DTO.V1.GetPageContent>();
        Assert.NotNull(data);
        Assert.That(data!.Title, Is.EqualTo(updatedEtTitle));
        
    }
}