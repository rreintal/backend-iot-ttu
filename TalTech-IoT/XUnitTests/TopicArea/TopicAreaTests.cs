using Integration;
using Xunit.Abstractions;
using System.Net;
using System.Net.Http.Json;
using Public.DTO.V1;

namespace XUnitTests.News
{
    public class TopicAreaTests : IClassFixture<CustomWebAppFactory<Program>>
    {
        private readonly CustomWebAppFactory<Program> _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public TopicAreaTests(ITestOutputHelper testOutputHelper, CustomWebAppFactory<Program> factory)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task CreateTopicArea_Success_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var newTopicArea = new PostTopicAreaDto { /* Initialize with valid data */ };
            var response = await client.PostAsJsonAsync("api/TopicAreas", newTopicArea);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            // Additional assertions as needed
        }

        [Fact]
        public async Task CreateTopicArea_ParentDoesNotExist_ReturnsConflict()
        {
            var client = _factory.CreateClient();
            var invalidTopicArea = new PostTopicAreaDto { /* Initialize with invalid parent ID */ };
            var response = await client.PostAsJsonAsync("api/TopicAreas", invalidTopicArea);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            // Additional assertions as needed
        }

        [Fact]
        public async Task GetAllTopicAreas_ReturnsOkWithData()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/en/TopicAreas");

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<TopicArea>>();
            Assert.NotNull(result);
            // Additional assertions as needed
        }

        [Fact]
        public async Task GetTopicAreasWithTranslation_ReturnsOkWithData()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/TopicAreas/GetWithTranslation");

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<TopicAreaWithTranslation>>();
            Assert.NotNull(result);
            // Additional assertions as needed
        }

        [Fact]
        public async Task GetTopicAreasWithCount_ReturnsOkWithData()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"api/TopicAreas?languageCulture=en&News=true&Projects=false");

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<TopicAreaWithCount>>();
            Assert.NotNull(result);
            // Additional assertions as needed
        }

        // Additional tests for other scenarios...
    }
}
