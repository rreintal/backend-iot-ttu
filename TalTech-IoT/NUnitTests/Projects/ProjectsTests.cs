using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using App.DAL.EF.Seeding;
using App.Domain;
using Integration;
using Public.DTO.V1;
using Xunit.Sdk;
using TopicArea = Public.DTO.V1.TopicArea;

namespace NUnitTests.Projects;

public class ProjectsTests
{
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
        // TODO: äkki mingi helper objekt mille kaudu saan muuta neid prope?
        // TODO: aga iga objekti jaoks Helper?? ei tundu ok, küsi Edgarilt ka
        var data = new Public.DTO.V1.PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = "title in eng"
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = "title in estonian"
                }
            },
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = "body in estonian"
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = "body in english"
                }
            },
            TopicAreas = new List<TopicArea>()
            {
                new TopicArea()
                {
                    Id = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID)
                }
            }
        };
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync("/api/Project", data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(1)]
    public async Task AddProjects_MissingTopicArea_ReturnsOk()
    {
        var data = new Public.DTO.V1.PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = "title in eng"
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = "title in estonian"
                }
            },
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = "body in estonian"
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = "body in english"
                }
            },
        };
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync("/api/Project", data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(2)]
    public async Task AddProjects_MissingProjectManager_ReturnsBadRequest()
    {
        var data = new Public.DTO.V1.PostProjectDto()
        {
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = "title in eng"
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = "title in estonian"
                }
            },
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = "body in estonian"
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = "body in english"
                }
            },
            TopicAreas = new List<TopicArea>()
            {
                new TopicArea()
                {
                    Id = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID)
                }
            }
        };
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync("/api/Project", data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test, Order(3)]
    public async Task AddProjects_MissingProjectVolume_ReturnsBadRequest()
    {
        /*
        var data = new Public.DTO.V1.PostProjectDto()
        {
            ProjectManager = "super manager",
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = "title in eng"
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = "title in estonian"
                }
            },
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = "body in estonian"
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = "body in english"
                }
            },
            TopicAreas = new List<TopicArea>()
            {
                new TopicArea()
                {
                    Id = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID)
                }
            }
        };
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync("/api/Project", data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        */
        throw new NotImplementedException();
    }
    
    [Test, Order(4)]
    public async Task AddProjects_MissingProjectYear_ReturnsBadRequest()
    {
        throw new NotImplementedException();
    }
    
    [Test, Order(5)]
    public async Task AddProjects_En_ReturnsCorrectBody()
    {
        var bodyMessage = "this is body";
        var data = new Public.DTO.V1.PostProjectDto()
        {
            ProjectManager = "super manager",
            ProjectVolume = 2000.0,
            Title = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = "title in eng"
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = "title in estonian"
                }
            },
            Body = new List<ContentDto>()
            {
                new ContentDto()
                {
                    Culture = LanguageCulture.EST,
                    Value = "body in estonian"
                },
                new ContentDto()
                {
                    Culture = LanguageCulture.ENG,
                    Value = bodyMessage
                }
            },
            TopicAreas = new List<TopicArea>()
            {
                new TopicArea()
                {
                    Id = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID)
                }
            }
        };
        var client = _factory!.CreateClient();
        var response = await client.PostAsJsonAsync("/api/Project", data);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();

        Assert.NotNull(responseData);
        var getResponse = await client.GetAsync("/api/projects/" + responseData!.Id);
        
        var getResponseData = await getResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
        Assert.NotNull(getResponseData);
        Assert.Equals(getResponseData!.Body, Is.EqualTo(bodyMessage));
    }
    
    [Test, Order(6)]
    public async Task AddProjects_Et_ReturnsCorrectBody()
    {
        throw new NotImplementedException();
    }
    
    [Test, Order(7)]
    public async Task AddProjects_En_ReturnsCorrectTitle()
    {
        throw new NotImplementedException();
    }
    
    [Test, Order(8)]
    public async Task AddProjects_Et_ReturnsCorrectTitle()
    {
        throw new NotImplementedException();
    }




}