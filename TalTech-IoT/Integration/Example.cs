using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Integration;

public class ExampleTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public ExampleTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async void GetNews()
    {
        // TODO - maybe create another db just for testing?!
        // Arrange
        var client = _factory.CreateClient(); // Create an HttpClient instance from the WebApplicationFactory.

        // Act
        var response = await client.GetAsync("api/et/project/"); // Send an HTTP GET request to your API endpoint.

        // Assert
        _testOutputHelper.WriteLine("ABC");
        _testOutputHelper.WriteLine(response.Content.ToString());
        response.EnsureSuccessStatusCode(); // Ensure that the response has a 2xx status code.
    } 
}