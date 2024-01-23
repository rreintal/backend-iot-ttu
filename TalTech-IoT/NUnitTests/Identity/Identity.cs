using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Integration;
using Public.DTO;
using Public.DTO.Identity;

namespace NUnitTests;

public class Identity
{
    private CustomWebAppFactory<Program>? _factory;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = new CustomWebAppFactory<Program>();
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _factory?.Dispose();
        
    }
    
    [Test, Order(1)]
    public async Task Register_NewUser_ReturnsOk()
    {
        // Arrange
        var client = _factory!.CreateClient();
        var registerModel = new Register
        {
            Firstname = "John",
            Lastname = "Doe",
            Email = "john.doe@example.com",
            Username = "johndoe123",
            // Add other required properties
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/Users/Register", registerModel);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        Assert.NotNull(jwtResponse);
        // Add more assertions as needed
    }
    
    [Test, Order(2)]
    public async Task Register_ExistingUser_ReturnsConflict()
    {
        // Arrange
        var client = _factory!.CreateClient();
        var registerModel = new Register
        {
            Firstname = "John",
            Lastname = "Doe",
            Email = "john.doe@example.com",
            Username = "johndoe123",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/Users/Register", registerModel);

        // Assert
        
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
    
    [Test, Order(3)]
    public async Task Login_ExistingUser_ReturnsOk()
    {
        // Arrange
        var client = _factory!.CreateClient();
        var loginModel = new Login
        {
            Email = "john.doe@example.com",
            Password = "Change.Me123" 
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/Users/Login", loginModel);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(responseContent);
        
        Assert.NotNull(jwtResponse);
    }
    
    /*
    [Test, Order(4)]
    public async Task Logout_CorrectJwt_ReturnsOk()
    {
        // Arrange
        var client = _factory!.CreateClient();
        var loginModel = new Login
        {
            Email = "john.doe@example.com",
            Password = "Change.Me123" 
        };

        // Act
        var responseForJwt = await client.PostAsJsonAsync("/api/Users/Login", loginModel);

        var responseContent = await responseForJwt.Content.ReadAsStringAsync();
        var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(responseContent);
        
        var logoutModel = new Logout()
        {
            RefreshToken = jwtResponse!.RefreshToken
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/Users/Logout", logoutModel);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    */
    [Test, Order(5)]
    public async Task Login_IncorrectPassword_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory!.CreateClient();
        var loginModel = new Login
        {
            Email = "john.doe@example.com",
            Password = "IncorrectPassword" // Assuming this is an incorrect password
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/Users/Login", loginModel);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
}