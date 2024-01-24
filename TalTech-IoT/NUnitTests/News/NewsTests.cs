using App.DAL.EF.Seeding;
using App.Domain;
using Factory;
using System.Net;
using System.Net.Http.Json;
using Integration;

namespace NUnitTests.News
{
    public class NewsTests
    {
        private CustomWebAppFactory<Program>? _factory;
        //private int NewsCount = 0;

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
            
            /*var topicArea = PublicFactory
                .TopicArea(Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID));
                */

            var newsDto = PublicFactory
                .CreatePostNews()
                .SetAuthor("Richard Reintal")
                .SetContent(ContentTypes.TITLE, new Dictionary<string, string>()
                {
                    { "eesti123 title", LanguageCulture.EST },
                    { "english title", LanguageCulture.ENG },
                })
                .SetContent(ContentTypes.BODY, new Dictionary<string, string>()
                {
                    { "eesti body", LanguageCulture.EST },
                    { "english body", LanguageCulture.ENG },
                })
                .SetTopicAreaId(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);

            //var jsonNewsDto = JsonSerializer.Serialize(newsDto);

            var client = _factory!.CreateClient();
            var response = await client.PostAsJsonAsync("/api/News", newsDto);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test, Order(1)]
        public async Task GetAllNews_OneExists_ReturnsOk()
        {
            var client = _factory!.CreateClient();
            var response = await client.GetAsync("/api/et/News/");
            var data = await response.Content.ReadFromJsonAsync<List<App.Domain.News>>();
            Assert.That(data!.Count, Is.EqualTo(1));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        

        [Test, Order(2)]
        public async Task AddNews_MissingTopicArea_ReturnsBadRequest()
        {
            var client = _factory!.CreateClient();
            var payload = PublicFactory.CreatePostNews()
                .SetAuthor("Richard Reintal")
                .SetContent(ContentTypes.TITLE, new Dictionary<string, string>()
                {
                    { "eesti123 title", LanguageCulture.EST },
                    { "english title", LanguageCulture.ENG },
                })
                .SetContent(ContentTypes.BODY, new Dictionary<string, string>()
                {
                    { "eesti body", LanguageCulture.EST },
                    { "english body", LanguageCulture.ENG },
                });

            var response = await client.PostAsJsonAsync("/api/News", payload);
            //NewsCount++;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        
        [Test, Order(3)]
        public async Task AddNews_Et_ReturnsCorrectBody()
        {
            var client = _factory!.CreateClient();
            var bodyString = "eeeee";
            var languageCulture = "et";
            var payload = PublicFactory.CreatePostNews()
                .SetAuthor("Richard Reintal")
                .SetContent(ContentTypes.TITLE, new Dictionary<string, string>()
                {
                    { "estonian title", LanguageCulture.EST },
                    { "english title", LanguageCulture.ENG },
                })
                .SetContent(ContentTypes.BODY, new Dictionary<string, string>()
                {
                    { bodyString, LanguageCulture.EST },
                    { "english body", LanguageCulture.ENG },
                })
                .SetTopicAreaId(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);;

            var response = await client.PostAsJsonAsync("/api/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var newsId = responseData!.Id;
            var getNewsResponse = await client.GetAsync($"/api/{languageCulture}/news/" + newsId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var getNewsResponseData = await getNewsResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(getNewsResponseData);
            Assert.That(getNewsResponseData!.Body, Is.EqualTo(bodyString));
        }
        
        [Test, Order(4)]
        public async Task AddNews_En_ReturnsCorrectBody()
        {
            var client = _factory!.CreateClient();
            var bodyString = "eeeee";
            var languageCulture = "en";
            var payload = PublicFactory.CreatePostNews()
                .SetAuthor("Richard Reintal")
                .SetContent(ContentTypes.TITLE, new Dictionary<string, string>()
                {
                    { "estonian title", LanguageCulture.EST },
                    { "english title", LanguageCulture.ENG },
                })
                .SetContent(ContentTypes.BODY, new Dictionary<string, string>()
                {
                    { "estonian body", LanguageCulture.EST },
                    { bodyString, LanguageCulture.ENG },
                })
                .SetTopicAreaId(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);;

            var response = await client.PostAsJsonAsync("/api/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var newsId = responseData!.Id;
            var getNewsResponse = await client.GetAsync($"/api/{languageCulture}/news/" + newsId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var getNewsResponseData = await getNewsResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(getNewsResponseData);
            Assert.That(getNewsResponseData!.Body, Is.EqualTo(bodyString));
        }
        
        [Test, Order(5)]
        public async Task AddNews_Et_ReturnsCorrectTitle()
        {
            var client = _factory!.CreateClient();
            var titleString = "eeeee";
            var languageCulture = "et";
            var payload = PublicFactory.CreatePostNews()
                .SetAuthor("Richard Reintal")
                .SetContent(ContentTypes.TITLE, new Dictionary<string, string>()
                {
                    { titleString, LanguageCulture.EST },
                    { "english title", LanguageCulture.ENG },
                })
                .SetContent(ContentTypes.BODY, new Dictionary<string, string>()
                {
                    { "estonian body", LanguageCulture.EST },
                    { "english body", LanguageCulture.ENG },
                })
                .SetTopicAreaId(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);;

            var response = await client.PostAsJsonAsync("/api/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var newsId = responseData!.Id;
            var getNewsResponse = await client.GetAsync($"/api/{languageCulture}/news/" + newsId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var getNewsResponseData = await getNewsResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(getNewsResponseData);
            Assert.That(getNewsResponseData!.Title, Is.EqualTo(titleString));
        }
        
        [Test, Order(6)]
        public async Task AddNews_En_ReturnsCorrectTitle()
        {
            var client = _factory!.CreateClient();
            var titleString = "eeeee";
            var languageCulture = "en";
            var payload = PublicFactory.CreatePostNews()
                .SetAuthor("Richard Reintal")
                .SetContent(ContentTypes.TITLE, new Dictionary<string, string>()
                {
                    { "estonain title", LanguageCulture.EST },
                    { titleString, LanguageCulture.ENG },
                })
                .SetContent(ContentTypes.BODY, new Dictionary<string, string>()
                {
                    { "estonian body", LanguageCulture.EST },
                    { "english body", LanguageCulture.ENG },
                })
                .SetTopicAreaId(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID);;

            var response = await client.PostAsJsonAsync("/api/News", payload);
            var responseData = await response.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(responseData);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var newsId = responseData!.Id;
            var getNewsResponse = await client.GetAsync($"/api/{languageCulture}/news/" + newsId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var getNewsResponseData = await getNewsResponse.Content.ReadFromJsonAsync<Public.DTO.V1.News>();
            Assert.NotNull(getNewsResponseData);
            Assert.That(getNewsResponseData!.Title, Is.EqualTo(titleString));
        }
        /*
        [Test, Order(4)]
        public async Task GetAllTopicAreas_WhenCalled_ReturnsOk()
        {
            throw new NotImplementedException();
        }
        */
        
    }
}
