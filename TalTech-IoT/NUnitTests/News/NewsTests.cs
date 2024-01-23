using App.DAL.EF.Seeding;
using App.Domain;
using Factory;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Integration;

namespace NUnitTests.News
{
    public class NewsTests
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
        public async Task AddNews_ValidData_ReturnsOk()
        {
            
            var topicArea = PublicFactory
                .TopicArea(Guid.Parse(AppDataSeeding.TOPIC_AREA_ROBOTICS_ID));

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
                .SetTopicArea(topicArea)
                .SetImage("empty");

            var jsonNewsDto = JsonSerializer.Serialize(newsDto);

            var client = _factory!.CreateClient();
            var content = new StringContent(jsonNewsDto, Encoding.UTF8, "application/json");
            var response = await client.PostAsJsonAsync("/api/Users/Register", content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
        }

        [Test, Order(1)]
        public async Task GetAllNews_One_ReturnsOk()
        {
            /*
            var client = _factory!.CreateClient();
            var response = await client.GetAsync("/api/News/Get");
            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<IEnumerable<Public.DTO.V1.News>>(responseContent);
            Assert.That(data!.ToList().Count, Is.EqualTo(1));            
            */
            throw new NotImplementedException();
        }
        

        [Test, Order(1)]
        public async Task AddNews_MissingAuthor_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test, Order(2)]
        public async Task News_PostWithoutAuthor_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test, Order(3)]
        public async Task GetAllNews_WhenCalled_ReturnsOk()
        {
            throw new NotImplementedException();
        }

        [Test, Order(4)]
        public async Task GetAllTopicAreas_WhenCalled_ReturnsOk()
        {
            throw new NotImplementedException();
        }
        
    }
}
