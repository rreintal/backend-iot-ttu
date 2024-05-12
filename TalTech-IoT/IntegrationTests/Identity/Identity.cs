using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using App.Domain.Constants;
using Integration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Public.DTO;
using Public.DTO.Identity;
using Public.DTO.V1;
using AppRole = DAL.DTO.Identity.AppRole;
using AppUser = App.Domain.Identity.AppUser;

namespace NUnitTests.Identity;

public class Identity
{
    private CustomWebAppFactory<Program>? _factory;
    private const string VERSION = "v1";
    private string BASE_URL => $"api/{VERSION}/Users";

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = new CustomWebAppFactory<Program>();
    }
    
    
    [Test, Order(1)]
    public async Task Register_NewUser_ReturnsOk()
    {
        // Arrange
        using (var scope = _factory!.Services.CreateScope())
        {
            // Resolve the RoleManager<AppRole> service
            var _roleManager = scope.ServiceProvider.GetService<RoleManager<App.Domain.Identity.AppRole>>();
            var client = _factory!.CreateClient();
            var userRole = await _roleManager!.FindByNameAsync(IdentityRolesConstants.ROLE_MODERATOR);
            Assert.NotNull(userRole);
        
            var registerModel = new Register
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Username = "johndoe123",
                Password = "Johndoe123.",
                RoleId = userRole!.Id
            };

            // Act
            var response = await client.PostAsJsonAsync($"{BASE_URL}/Register", registerModel);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(jwtResponse);
        }
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
            Password = "Johndoe123."
        };

        // Act
        var response = await client.PostAsJsonAsync($"{BASE_URL}/Register", registerModel);

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
            Password = "Johndoe123." 
        };

        // Act
        var response = await client.PostAsJsonAsync($"{BASE_URL}/Login", loginModel);

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
            RefreshToken = jwtResponse!.RefreshToken,
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
            Password = "Johndoe123.22"  // Assuming this is an incorrect password
        };

        // Act
        var response = await client.PostAsJsonAsync($"{BASE_URL}/Login", loginModel);
        var responseStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseStr);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    /*
    
    [Test, Order(6)]
    public async Task Users_LockAccountValidData_ReturnsOk()
    {
        
            var client = _factory!.CreateClient();
        
            // Create new account to lock
            var userRole = await roleManager!.FindByNameAsync(IdentityRolesConstants.ROLE_MODERATOR);
            Assert.NotNull(userRole);
        
            var registerModel = new Register
            {
                Firstname = "UserToLock",
                Lastname = "LockedUserln",
                Email = "locker@example.com",
                Username = "LockMan",
                Password = "Johndoe123.",
                RoleId = userRole!.Id
            };

            var response = await client.PostAsJsonAsync($"{BASE_URL}/Register", registerModel);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(jwtResponse);

            var lockModel = new UserIdDto()
            {
                UserId = Guid.Parse(jwtResponse!.AppUserId)
            };

            var lockAccount = await client.PostAsJsonAsync($"{BASE_URL}/Lock", lockModel);
        
            Assert.That(lockAccount.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
    }
    
    [Test, Order(7)]
    public async Task Users_LockAccountAlreadyLocked_ReturnsBadRequest()
    {
        var roleManager = scope.ServiceProvider.GetService<RoleManager<AppRole>>();
            var client = _factory!.CreateClient();
        
            // Create new account to lock
            var userRole = await roleManager!.FindByNameAsync(IdentityRolesConstants.ROLE_MODERATOR);
            Assert.NotNull(userRole);
        
            var registerModel = new Register
            {
                Firstname = "UserToLocks",
                Lastname = "LockedUserlns",
                Email = "lockesr@example.com",
                Username = "LocskMan",
                Password = "Johndoe123.",
                RoleId = userRole!.Id
            };

            var response = await client.PostAsJsonAsync($"{BASE_URL}/Register", registerModel);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(jwtResponse);

            var lockModel = new UserIdDto()
            {
                UserId = Guid.Parse(jwtResponse!.AppUserId)
            };

            var lockAccount = await client.PostAsJsonAsync($"{BASE_URL}/Lock", lockModel);
        
            Assert.That(lockAccount.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
            var lockAccountAgain = await client.PostAsJsonAsync($"{BASE_URL}/Lock", lockModel);
            Assert.That(lockAccountAgain.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            
    }
    */
    
}