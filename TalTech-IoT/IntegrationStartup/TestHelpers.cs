using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using App.Domain.Constants;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Public.DTO;
using Public.DTO.Identity;

namespace Integration;

public class TestHelpers
{
    private const string VERSION = "v1";
    private const string LOGIN = $"/api/{VERSION}/Users/Login";
    public static async Task Authenticate(HttpClient client, Login loginModel)
    {
        var response = await client.PostAsJsonAsync(LOGIN, loginModel);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(responseContent, options);
        Assert.NotNull(jwtResponse!.JWT);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse.JWT);
    }

    public static Login MakeAdminLoginModel()
    {
        return new Login()
        {
            Email = TestConstants.TestAdminEmail,
            Password = TestConstants.TestAdminPassword
        };
    }
}