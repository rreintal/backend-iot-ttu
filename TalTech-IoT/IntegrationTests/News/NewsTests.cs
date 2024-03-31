using App.DAL.EF.Seeding;
using App.Domain;
using System.Net;
using System.Net.Http.Json;
using Integration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Public.DTO.V1;
using TopicArea = Public.DTO.V1.TopicArea;

namespace NUnitTests.News
{
    // TODO: Test viewCount
    public class NewsTests
    {
        private const string BASE_URL = $"api/{VERSION}News";
        private const string VERSION = "v1/";
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
        public async Task AddNews_ValidData_ReturnsOk()
        {

            var payload = new PostNewsDto()
            {
                Author = "Richard Reintal",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = "eesti pealkiri",
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = "english title",
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = "eesti sisu",
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = "english body",
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID)
                    }
                },
                Image = "image stuff"
            };

            var client = _factory!.CreateClient();
            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var str = await response.Content.ReadAsStringAsync();
            Console.WriteLine(str);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test, Order(1)]
        public async Task GetAllNews_OneExists_ReturnsOk()
        {
            var client = _factory!.CreateClient();
            var response = await client.GetAsync("/api/v1/News/et");
            var data = await response.Content.ReadFromJsonAsync<List<App.Domain.News>>();
            Assert.That(data!.Count, Is.EqualTo(1));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        

        [Test, Order(2)]
        public async Task AddNews_MissingTopicArea_ReturnsBadRequest()
        {
            var client = _factory!.CreateClient();
            var payload = new PostNewsDto()
            {
                Image = "image stuff",
                Author = "Richard Reintal",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = "eesti pealkiri",
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = "english title",
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = "eesti sisu",
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = "english body",
                        Culture = LanguageCulture.ENG
                    }
                }
            };

            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        
        [Test, Order(3)]
        public async Task AddNews_Et_ReturnsCorrectBody()
        {
            var client = _factory!.CreateClient();
            var bodyString = Guid.NewGuid().ToString();
            var languageCulture = LanguageCulture.EST;
            var payload = new PostNewsDto()
            {
                Image = "image stuff",
                Author = "Richard Reintal",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = "eesti pealkiri",
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = "english title",
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyString,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = "english body",
                        Culture = LanguageCulture.ENG
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

            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var newsId = responseData!.Id;
            var getNewsResponse = await client.GetAsync($"/api/v1/news/{languageCulture}/" + newsId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var getNewsResponseData = await getNewsResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(getNewsResponseData);
            Assert.That(getNewsResponseData!.Body, Is.EqualTo(bodyString));
        }
        
        [Test, Order(4)]
        public async Task AddNews_En_ReturnsCorrectBody()
        {
            var client = _factory!.CreateClient();
            var bodyString = Guid.NewGuid().ToString();
            var languageCulture = LanguageCulture.ENG;
            var payload = new PostNewsDto()
            {
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = "eesti pealkiri",
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = "english title",
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = "eesti sisu",
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = bodyString,
                        Culture = LanguageCulture.ENG
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

            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var newsId = responseData!.Id;
            var getNewsResponse = await client.GetAsync($"/api/v1/news/{languageCulture}/" + newsId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var getNewsResponseData = await getNewsResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(getNewsResponseData);
            Assert.That(getNewsResponseData!.Body, Is.EqualTo(bodyString));
        }
        
        [Test, Order(5)]
        public async Task AddNews_Et_ReturnsCorrectTitle()
        {
            var client = _factory!.CreateClient();
            var titleString = Guid.NewGuid().ToString();
            var languageCulture = LanguageCulture.EST;
            var payload = new PostNewsDto()
            {
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleString,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = "english title",
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = "eesti sisu",
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = "english body",
                        Culture = LanguageCulture.ENG
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
            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var newsId = responseData!.Id;
            var getNewsResponse = await client.GetAsync($"/api/v1/news/{languageCulture}/" + newsId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var getNewsResponseData = await getNewsResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(getNewsResponseData);
            Assert.That(getNewsResponseData!.Title, Is.EqualTo(titleString));
        }
        
        [Test, Order(6)]
        public async Task AddNews_En_ReturnsCorrectTitle()
        {
            var client = _factory!.CreateClient();
            var titleString = Guid.NewGuid().ToString();
            var languageCulture = LanguageCulture.ENG;
            var payload = new PostNewsDto()
            {
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = "eesti pealkiri",
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleString,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = "eesti sisu",
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = "english body",
                        Culture = LanguageCulture.ENG
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

            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var newsId = responseData!.Id;
            var getNewsResponse = await client.GetAsync($"/api/v1/news/{languageCulture}/" + newsId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var getNewsResponseData = await getNewsResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(getNewsResponseData);
            Assert.That(getNewsResponseData!.Title, Is.EqualTo(titleString));
        }

        [Test, Order(7)]
        public async Task UpdateNews_Et_ReturnsCorrectTitle()
        {
            var client = _factory!.CreateClient();
            var languageCulture = LanguageCulture.EST;
            var titleET = "pealkiri eesti keeles";
            var titleEN = "title in english";
            var bodyET = "sisu eesti keeles";
            var bodyEN = "body in english";
            var topicAreaId = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);
            var updatedTitleET = "see on uus pealkiri!";
            var payload = new PostNewsDto()
            {
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = bodyEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };
            
            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var newsId = responseData!.Id;
            
            var updatedPayload = new UpdateNews()
            {
                Id = newsId,
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = updatedTitleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = bodyEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };

            var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedPayload);
            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var getUpdatedResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{newsId}");
            var data = await getUpdatedResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            
            Assert.NotNull(data);
            Assert.That(data!.Title, Is.EqualTo(updatedTitleET));
        }
        
        
        [Test, Order(8)]
        public async Task UpdateNews_En_ReturnsCorrectTitle()
        {
            var client = _factory!.CreateClient();
            var languageCulture = LanguageCulture.ENG;
            var titleET = "pealkiri eesti keeles";
            var titleEN = "title in english";
            var bodyET = "sisu eesti keeles";
            var bodyEN = "body in english";
            var topicAreaId = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);
            var updatetTitleEN = "this is new title";
            var payload = new PostNewsDto()
            {
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = bodyEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };
            
            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var newsId = responseData!.Id;
            
            var updatedPayload = new UpdateNews()
            {
                Id = newsId,
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = updatetTitleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = bodyEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };

            var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedPayload);
            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var getUpdatedResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{newsId}");
            var data = await getUpdatedResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            
            Assert.NotNull(data);
            Assert.That(data!.Title, Is.EqualTo(updatetTitleEN));
        }
        
        [Test, Order(9)]
        public async Task UpdateNews_Et_ReturnsCorrectBody()
        {
            var client = _factory!.CreateClient();
            var languageCulture = LanguageCulture.EST;
            var titleET = "pealkiri eesti keeles";
            var titleEN = "title in english";
            var bodyET = "sisu eesti keeles";
            var bodyEN = "body in english";
            var topicAreaId = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);
            var updatedBodyEt = "see on uus sisu";
            var payload = new PostNewsDto()
            {
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = bodyEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };
            
            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var newsId = responseData!.Id;
            
            var updatedPayload = new UpdateNews()
            {
                Id = newsId,
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = updatedBodyEt,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = bodyEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };

            var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedPayload);
            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var getUpdatedResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{newsId}");
            var data = await getUpdatedResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            
            Assert.NotNull(data);
            Assert.That(data!.Body, Is.EqualTo(updatedBodyEt));
        }
        
        [Test, Order(10)]
        public async Task UpdateNews_En_ReturnsCorrectBody()
        {
            var client = _factory!.CreateClient();
            var languageCulture = LanguageCulture.ENG;
            var titleET = "pealkiri eesti keeles";
            var titleEN = "title in english";
            var bodyET = "sisu eesti keeles";
            var bodyEN = "body in english";
            var topicAreaId = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);
            var updatedBodyEn = "this is new body";
            var payload = new PostNewsDto()
            {
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = bodyEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };
            
            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var newsId = responseData!.Id;
            
            var updatedPayload = new UpdateNews()
            {
                Id = newsId,
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = updatedBodyEn,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };

            var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedPayload);
            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var getUpdatedResponse = await client.GetAsync($"{BASE_URL}/{languageCulture}/{newsId}");
            var data = await getUpdatedResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            
            Assert.NotNull(data);
            Assert.That(data!.Body, Is.EqualTo(updatedBodyEn));
        }
        
        [Test, Order(11)]
        public async Task UpdateNews_InvalidId_ReturnsNotFound()
        {
            var client = _factory!.CreateClient();
            var languageCulture = LanguageCulture.ENG;
            var titleET = "pealkiri eesti keeles";
            var titleEN = "title in english";
            var bodyET = "sisu eesti keeles";
            var bodyEN = "body in english";
            var topicAreaId = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);
            var updatedBodyEn = "this is new body";
            var payload = new PostNewsDto()
            {
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = bodyEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };
            
            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var newsId = Guid.NewGuid();
            
            var updatedPayload = new UpdateNews()
            {
                Id = newsId,
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = updatedBodyEn,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };

            var updateResponse = await client.PutAsJsonAsync(BASE_URL, updatedPayload);
            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        
        [Test, Order(12)]
        public async Task DeleteNews_CorrectId_ReturnsOk()
        {
            var client = _factory!.CreateClient();
            var languageCulture = LanguageCulture.ENG;
            var titleET = "pealkiri eesti keeles";
            var titleEN = "title in english";
            var bodyET = "sisu eesti keeles";
            var bodyEN = "body in english";
            var topicAreaId = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);
            var payload = new PostNewsDto()
            {
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = bodyEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };
            
            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var newsId = responseData!.Id;

            var deleteResponse = await client.DeleteAsync($"{BASE_URL}/{newsId}");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        
        [Test, Order(13)]
        public async Task DeleteNews_InvalidId_ReturnsNotFound()
        {
            var client = _factory!.CreateClient();
            var languageCulture = LanguageCulture.ENG;
            var titleET = "pealkiri eesti keeles";
            var titleEN = "title in english";
            var bodyET = "sisu eesti keeles";
            var bodyEN = "body in english";
            var topicAreaId = Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);
            var payload = new PostNewsDto()
            {
                Author = "Richard Reintal",
                Image = "image stuff",
                Title = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = titleET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = titleEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                Body = new List<ContentDto>()
                {
                    new ContentDto()
                    {
                        Value = bodyET,
                        Culture = LanguageCulture.EST
                    },
                    new ContentDto()
                    {
                        Value = bodyEN,
                        Culture = LanguageCulture.ENG
                    }
                },
                TopicAreas = new List<TopicArea>()
                {
                    new TopicArea()
                    {
                        Id = topicAreaId
                    }
                }
            };
            
            var response = await client.PostAsJsonAsync("/api/v1/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var newsId = Guid.NewGuid();

            var deleteResponse = await client.DeleteAsync($"{BASE_URL}/{newsId}");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        
        
        
    }
}
