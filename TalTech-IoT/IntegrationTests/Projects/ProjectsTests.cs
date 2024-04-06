using System.Net;
using System.Net.Http.Json;
using App.Domain;
using Integration;
using Public.DTO.V1;

namespace NUnitTests.Projects;

public class ProjectsTests
{
    private const string BASE_URL = $"api/{VERSION}/Project";
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
    public async Task AddProjects_ValidData_ReturnsOk()
    {
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            IsOngoing = true,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: "title in eng"),
                new(culture: LanguageCulture.EST, value: "title in estonian")
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: "body in estonian"),
                new(culture: LanguageCulture.ENG, value: "body in english")
            }
        };
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(1)]
    public async Task AddProjects_MissingTopicArea_ReturnsOk()
    {
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: "title in eng"),
                new(culture: LanguageCulture.EST, value: "title in estonian")
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: "body in estonian"),
                new(culture: LanguageCulture.ENG, value: "body in english")
            },
        };
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(2)]
    public async Task AddProjects_MissingProjectManager_ReturnsBadRequest()
    {
        var data = new PostProjectDto()
        {
            ProjectVolume = 2000.0,
            IsOngoing = true,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: "title in eng"),
                new(culture: LanguageCulture.EST, value: "title in estonian")
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: "body in estonian"),
                new(culture: LanguageCulture.ENG, value: "body in english")
            }
        };
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test, Order(3)]
    public async Task AddProjects_MissingProjectVolume_ReturnsBadRequest()
    {
        
        var data = new PostProjectDto()
        {
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: "title in eng"),
                new(culture: LanguageCulture.EST, value: "title in estonian")
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: "body in estonian"),
                new(culture: LanguageCulture.ENG, value: "body in english")
            }
        };
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test, Order(5)]
    public async Task AddProjects_En_ReturnsCorrectBody()
    {
        var languageCulture = LanguageCulture.ENG;
        var bodyMessage = "this is body";
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            IsOngoing = true,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: "title in eng"),
                new(culture: LanguageCulture.EST, value: "title in estonian")
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: "body in estonian"),
                new(culture: LanguageCulture.ENG, value: bodyMessage)
            }
            
        };
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        
        Assert.NotNull(responseData);
        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{responseData!.Id}");
        
        var getResponseData = await getResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(getResponseData);
        Assert.That(getResponseData!.Body, Is.EqualTo(bodyMessage));
    }
    
    [Test, Order(6)]
    public async Task AddProjects_Et_ReturnsCorrectBody()
    {
        var client = _factory!.CreateClient();
        var languageCulture = LanguageCulture.EST;
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEt),
                new(culture: LanguageCulture.EST, value: titleEn)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        
        Assert.NotNull(responseData);
        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{responseData!.Id}");
        
        var getResponseData = await getResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(getResponseData);
        Assert.That(getResponseData!.Body, Is.EqualTo(bodyEt));
    }
    
    [Test, Order(7)]
    public async Task AddProjects_En_ReturnsCorrectTitle()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.ENG;
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        
        var data = new PostProjectDto
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };

        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        
        Assert.NotNull(responseData);
        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{responseData!.Id}");
        
        var getResponseData = await getResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(getResponseData);
        Assert.That(getResponseData!.Title, Is.EqualTo(titleEn));
    }
    
    [Test, Order(8)]
    public async Task AddProjects_Et_ReturnsCorrectTitle()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.EST;
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        
        Assert.NotNull(responseData);
        var getResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{responseData!.Id}");
        
        var getResponseData = await getResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(getResponseData);
        Assert.That(getResponseData!.Title, Is.EqualTo(titleEt));
    }

    [Test, Order(8)]
    public async Task UpdateProjects_ValidData_ReturnsOk()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        Assert.NotNull(responseData);

        var updatedBodyEt = "abcdef";
        var projectId = responseData!.Id;
        var updatedData = new UpdateProject()
        {
            Id = projectId,
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: updatedBodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(9)]
    public async Task UpdateProjects_InvalidValidData_ReturnsBadRequest()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        Assert.NotNull(responseData);

        var projectId = responseData!.Id;
        var updatedData = new UpdateProject()
        {
            Id = projectId,
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test, Order(9)]
    public async Task UpdateProjects_Et_ReturnsCorrectTitle()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.EST;
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        Assert.NotNull(responseData);

        var updatedTitleEt = "see on uus pealkiri!";
        var projectId = responseData!.Id;
        var updatedData = new UpdateProject()
        {
            Id = projectId,
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: updatedTitleEt),
                
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedNewsResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{projectId}");
        var updatedProject = await getUpdatedNewsResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(updatedProject);
        Assert.That(updatedProject!.Title, Is.EqualTo(updatedTitleEt));
    }
    
    [Test, Order(9)]
    public async Task UpdateProjects_En_ReturnsCorrectTitle()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.ENG;
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        Assert.NotNull(responseData);

        var updatedTitleEn = "new title in english";
        var projectId = responseData!.Id;
        var updatedData = new UpdateProject()
        {
            Id = projectId,
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: updatedTitleEn),
                new(culture: LanguageCulture.EST, value: titleEt),
                
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedNewsResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{projectId}");
        var updatedProject = await getUpdatedNewsResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(updatedProject);
        Assert.That(updatedProject!.Title, Is.EqualTo(updatedTitleEn));
    }
    
    [Test, Order(9)]
    public async Task UpdateProjects_Et_ReturnsCorrectBody()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.EST;
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        Assert.NotNull(responseData);

        var updatedbodyEt = "siin on uus sisu";
        var projectId = responseData!.Id;
        var updatedData = new UpdateProject()
        {
            Id = projectId,
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt),
                
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: updatedbodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedNewsResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{projectId}");
        var updatedProject = await getUpdatedNewsResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(updatedProject);
        Assert.That(updatedProject!.Body, Is.EqualTo(updatedbodyEt));
    }
    
    [Test, Order(9)]
    public async Task UpdateProjects_En_ReturnsCorrectBody()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.ENG;
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        Assert.NotNull(responseData);

        var updatedbodyEn = "this is new body";
        var projectId = responseData!.Id;
        var updatedData = new UpdateProject()
        {
            Id = projectId,
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt),
                
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: updatedbodyEn)
            }
        };
        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedNewsResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{projectId}");
        var updatedProject = await getUpdatedNewsResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(updatedProject);
        Assert.That(updatedProject!.Body, Is.EqualTo(updatedbodyEn));
    }
    

    [Test, Order(9)]
    public async Task UpdateProjects_Et_ProjectVolume_ReturnsCorrectProjectVolume()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.EST;
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        var projectVolume = 2000.0;
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = projectVolume,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        Assert.NotNull(responseData);

        var updateProjectVolume = 2500;
        var projectId = responseData!.Id;
        var updatedData = new UpdateProject()
        {
            Id = projectId,
            ProjectManager = "super manager",
            ProjectVolume = updateProjectVolume,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt),
                
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedNewsResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{projectId}");
        var updatedProject = await getUpdatedNewsResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(updatedProject);
        Assert.That(updatedProject!.ProjectVolume, Is.EqualTo(updateProjectVolume));
    }
    
    [Test, Order(9)]
    public async Task UpdateProjects_En_ProjectVolume_ReturnsCorrectProjectVolume()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.ENG;
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        var projectVolume = 2000.0;
        var data = new PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = projectVolume,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        Assert.NotNull(responseData);

        var updateProjectVolume = 2500;
        var projectId = responseData!.Id;
        var updatedData = new UpdateProject()
        {
            Id = projectId,
            ProjectManager = "super manager",
            ProjectVolume = updateProjectVolume,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt),
                
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedNewsResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{projectId}");
        var updatedProject = await getUpdatedNewsResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(updatedProject);
        Assert.That(updatedProject!.ProjectVolume, Is.EqualTo(updateProjectVolume));
    }

    [Test, Order(9)]
    public async Task UpdateProjects_En_ProjectManager_ReturnsCorrectProjectManager()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.ENG;
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        var projectManager = "not cool manager";
        var data = new PostProjectDto()
        {
            ProjectManager = projectManager,
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        Assert.NotNull(responseData);

        var updatedProjectManager = "cool manager";
        var projectId = responseData!.Id;
        var updatedData = new UpdateProject()
        {
            Id = projectId,
            ProjectManager = updatedProjectManager,
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt),
                
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedNewsResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{projectId}");
        var updatedProject = await getUpdatedNewsResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(updatedProject);
        Assert.That(updatedProject!.ProjectManager, Is.EqualTo(updatedProjectManager));
    }
    
    [Test, Order(9)]
    public async Task UpdateProjects_Et_ProjectManager_ReturnsCorrectProjectManager()
    {
        var client = _factory!.CreateClient();
        await TestHelpers.Authenticate(client, TestHelpers.MakeAdminLoginModel());
        var languageCulture = LanguageCulture.EST;
        var titleEt = "pealkiri eesti keeles";
        var titleEn = "title in english";
        var bodyEn = "body in english";
        var bodyEt = "this is body";
        var projectManager = "not cool manager";
        var data = new PostProjectDto()
        {
            ProjectManager = projectManager,
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt)
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        
        var response = await client.PostAsJsonAsync(BASE_URL, data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseData = await response.Content.ReadFromJsonAsync<PostProjectSuccessDto>();
        Assert.NotNull(responseData);

        var updatedProjectManager = "cool manager";
        var projectId = responseData!.Id;
        var updatedData = new UpdateProject()
        {
            Id = projectId,
            ProjectManager = updatedProjectManager,
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new(culture: LanguageCulture.ENG, value: titleEn),
                new(culture: LanguageCulture.EST, value: titleEt),
                
            },
            Body = new List<ContentDto>()
            {
                new(culture: LanguageCulture.EST, value: bodyEt),
                new(culture: LanguageCulture.ENG, value: bodyEn)
            }
        };
        var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedData);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUpdatedNewsResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{projectId}");
        var updatedProject = await getUpdatedNewsResponse.Content.ReadFromJsonAsync<GetProject>();
        Assert.NotNull(updatedProject);
        Assert.That(updatedProject!.ProjectManager, Is.EqualTo(updatedProjectManager));
    }


}