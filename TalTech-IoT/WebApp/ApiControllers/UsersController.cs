using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using App.BLL.Contracts;
using App.DAL.EF;
using App.Domain;
using App.Domain.Constants;
using App.Domain.Identity;
using Asp.Versioning;
using Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO;
using Public.DTO.Identity;
using Public.DTO.V1;
using Public.DTO.V1.Mappers;
using AppRole = App.Domain.Identity.AppRole;
using AppUser = App.Domain.Identity.AppUser;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller for users authorization and registration service.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class UsersController : ControllerBase
{
    
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _Configuration;
    private readonly AppDbContext _context;
    private readonly IAppBLL _bll;
    private readonly RoleManager<AppRole> _roleManager;


    /// <summary>
    /// Controller for users
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="configuration"></param>
    /// <param name="context"></param>
    /// <param name="bll"></param>
    /// <param name="roleManager"></param>
    public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, AppDbContext context, IAppBLL bll, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _Configuration = configuration;
        _context = context;
        _bll = bll;
        _roleManager = roleManager;
    }

    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="register"></param>
    /// <returns></returns>
    [HttpPost("Register")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiResponse), 400)]
    public async Task<ActionResult<JWTResponse>> Register([FromBody]Register register)
    {
        var user = await _context.Users.Where(x => x.UserName == register.Username || x.Email == register.Email)
            .FirstOrDefaultAsync();

        if (user != null)
        {
            if (user.NormalizedEmail == register.Email.ToUpper())
            {
                return BadRequest(
                    new RestApiResponse()
                    {
                        Status = HttpStatusCode.Conflict,
                        Message = RestApiErrorMessages.UserEmailAlreadyExists
                    });
            }
            if (user.NormalizedUserName == register.Username.ToUpper())
            {
                return BadRequest(
                    new RestApiResponse()
                    {
                        Status = HttpStatusCode.BadRequest,
                        Message =  RestApiErrorMessages.UserUsernameAlreadyExists// "Username already registered!"
                    });
            }
        }
        // Register account
        var refreshToken = new AppRefreshToken();
        var appUser = new AppUser()
        {
            Firstname = register.Firstname,
            Lastname = register.Lastname,
            Email = register.Email,
            UserName = register.Username,
            AppRefreshTokens = new List<AppRefreshToken>() {refreshToken}
        };
        refreshToken.AppUser = appUser;
        
        
        var result = await _userManager.CreateAsync(appUser, register.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = RestApiErrorMessages.UserGeneralError,
                Status = HttpStatusCode.BadRequest
            });
        }

        var role = await _roleManager.FindByIdAsync(register.RoleId.ToString());
        if (role == null)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = RestApiErrorMessages.RoleNotFound,
                Status = HttpStatusCode.BadRequest
            });
        }
        await _userManager.AddToRoleAsync(appUser, role.Name);
        
        result = await _userManager.AddClaimsAsync(appUser, new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, appUser.Firstname),
                new Claim(ClaimTypes.Surname, appUser.Lastname),
                new Claim(ClaimTypes.Role, IdentityRolesConstants.ROLE_MODERATOR)
            });

            if (!result.Succeeded)
            {
                return BadRequest(
                    new RestApiResponse()
                    {
                        Status = HttpStatusCode.BadRequest,
                        Message = "registration failed!"
                    });
            }

            // generate JWT
            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
            var jwt = IdentityHelpers.GenerateJwt(
                claimsPrincipal.Claims,
                Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_KEY)!,
                Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_ISSUER)!,
                Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_AUDIENCE)!,
                10 // TODO: add to env variable

            );

            var res = new JWTResponse()
            {
                JWT = jwt,
                RefreshToken = refreshToken.RefreshToken,
                AppUserId = appUser.Id.ToString(),
                Username = register.Username
            };
            await _context.SaveChangesAsync();
            return Ok(res);
    }

    /// <summary>
    /// Login
    /// </summary>
    [HttpPost("Login")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiResponse), 400)]
    public async Task<ActionResult<JWTResponse>> Login([FromBody] Login login)
    {
        var appUser = await _userManager.FindByEmailAsync(login.Email);
        
        if (appUser == null)
        {
            return NotFound(new RestApiResponse()
            {
                Status = HttpStatusCode.NotFound,
                Message = RestApiErrorMessages.UserGeneralError
            });
        }
        
        // verify password
        
        
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, login.Password, false);
        if (!result.Succeeded)
        {
            return NotFound(new RestApiResponse()
            {
                Status = HttpStatusCode.NotFound,
                Message = RestApiErrorMessages.UserGeneralError // TODO: for testing!
            });
        }
        
        // get user claims
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        
        
        appUser.AppRefreshTokens = await _context
            .Entry(appUser)
            .Collection(a => a.AppRefreshTokens!)
            .Query()
            .Where(t => t.AppUserId == appUser.Id)
            .ToListAsync();
        
        // remove expired REFRESH tokens!!
        foreach (var userRefreshToken in appUser.AppRefreshTokens)
        {
            if (userRefreshToken.ExpirtationDT < DateTime.UtcNow)
            {
                _context.AppRefreshTokens.Remove(userRefreshToken);
            }
        }
        
        var refreshToken = new AppRefreshToken
        {
            AppUserId = appUser.Id
        };
        _context.AppRefreshTokens.Add(refreshToken);
        
        await _context.SaveChangesAsync();
        // generate JWT
        var jwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_KEY)!,
            Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_ISSUER)!,
            Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_AUDIENCE)!,
            10
        );

        var userRoles = await _roleManager.Roles
            .Include(x => x.UserRoles)!
            .ThenInclude(x => x.AppUser)
            .Where(x => x.UserRoles!.Any(ur => ur.AppUser!.Id == appUser.Id))
            .ToListAsync();
        
        
        var res = new JWTResponse()
        {
            JWT = jwt,
            RefreshToken = refreshToken.RefreshToken,
            AppUserId = appUser.Id.ToString(),
            Username = appUser.UserName,
            RoleIds = userRoles.Select(e => e.Id).ToList()
        };

        
        return Ok(res);
    }

    /// <summary>
    /// Refresh JWT
    /// </summary>
    [HttpPost("RefreshToken")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiResponse), 400)]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
    {
        JwtSecurityToken jwtToken;

        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);
            if (jwtToken == null)
            {
                return BadRequest(new RestApiResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = "No token!"
                });
            }
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Message = $"Cant parse token, {e.Message}"
            });
        }
        
        if (!IdentityHelpers.ValidateToken(
                refreshTokenModel.Jwt,
                Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_KEY)!,
                Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_ISSUER)!,
                Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_AUDIENCE)!))
        {
            return BadRequest(new RestApiResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Message = $"JWT validation fail"
            });
        }
        
        
            var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return BadRequest(new RestApiResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Message = "No email in jwt"
            });
        }
        
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        
        // Check if refreshToken exists

        var refreshTokenFromDatabase = await _context
            .AppRefreshTokens
            .Where(t => t.RefreshToken == refreshTokenModel.RefreshToken)
            .FirstOrDefaultAsync();

        if (refreshTokenFromDatabase == null)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = "Refresh token doesn't exist!",
                Status = HttpStatusCode.BadRequest
            });
        }
    
        // Kui refreshToken ei kehti enam!!
        // TODO - tagasta mingi x error, et teaksid telos visata login screenile!!
        if (refreshTokenFromDatabase.ExpirtationDT < DateTime.UtcNow)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = "Refresh token is expired!",
                Status = HttpStatusCode.BadRequest
            });
        }
        
        // generate new jwt

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            return NotFound(new RestApiResponse()
            {
                Status = HttpStatusCode.NotFound,
                Message = "Cannot get claims!"
            });
        }
        
        var jwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_KEY)!,
            Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_ISSUER)!,
            Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_AUDIENCE)!,
            10
            );

        
        // add new refreshtoken!
        var newRefreshToken = new AppRefreshToken();
        newRefreshToken.AppUserId = appUser.Id;
        
        // add new token to db (kas on vaja või see handlib kuidagi ise??)
        await _context.AppRefreshTokens.AddAsync(newRefreshToken);
        
        // remove refreshToken which was used to ask for new JWT!!
        _context.AppRefreshTokens.Remove(refreshTokenFromDatabase);
        
        
        await _context.SaveChangesAsync();

        // TODO: HACK!!!
        var userRoles = await _userManager.GetRolesAsync(appUser);
        if (userRoles.Count != 1)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = "Failed to get user roles. because of a hack, sry boss",
                Status = HttpStatusCode.BadRequest
            });
        }
        var rolename = userRoles[0];
        var userRole = await _roleManager.FindByNameAsync(rolename);
        
        // Idk kas siia on appUserId ka vaja v ei
        var res = new JWTResponse()
        {
            JWT = jwt,
            RefreshToken = newRefreshToken.RefreshToken,
            Username = appUser.UserName,
            RoleIds =  new List<Guid>() {userRole!.Id},
            AppUserId = appUser.Id.ToString()
        };

        return Ok(res);
    }

    /// <summary>
    /// Log out
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="logoutModel"></param>
    /// <returns></returns>
    [HttpPost("Logout")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiResponse), 400)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> LogOut([FromBody] Logout logoutModel)
    {
        var userId = User.GetUserId();

        if (userId == null)
        {
            return BadRequest(new RestApiResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Message = RestApiErrorMessages.UserGeneralError
            });
        }
        
        var userRefreshTokens = await _context.AppRefreshTokens
            .Where(x => x.AppUserId == userId && x.RefreshToken == logoutModel.RefreshToken)
            .ToListAsync();

        if (userRefreshTokens.Count == 0)
        {
            return BadRequest(new RestApiResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Message = RestApiErrorMessages.UserGeneralError
            });
        }
        
        _context.AppRefreshTokens.RemoveRange(userRefreshTokens);

        await _context.SaveChangesAsync();
        
        return Ok();
    }

    /// <summary>
    /// Change password
    /// </summary>
    /// <param name="model">A model containing UserId and the new password.</param>
    /// <returns></returns>
    [HttpPost("ChangePassword")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiResponse), 400)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        // TODO: ei tööta kuna see pole [Authorized], me ei saa useri ID kätte
        // Alternatiiv, lisame userID ka query paramiga kaasa, et parooli vahetada.
        // Meil tuleks : www.iot.ttu.ee/et/changePassword/?id=userId
        
        // Minu eelistus - Alternatiiv 2 -
        // Kasutajale saadetakse parool, ta peab sisse logima ja parooli ära vahetama ise
        // nii saame kasutada [Authorize] siin endpointil
        var userId = User.GetUserId();
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return BadRequest(
                new RestApiResponse
                {
                    Message = RestApiErrorMessages.UserGeneralError,
                    Status = HttpStatusCode.BadRequest
                });
        }

        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, model.OldPassword);

        if (!isCorrectPassword)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = RestApiErrorMessages.UserGeneralError,
                Status = HttpStatusCode.BadRequest
            });
        }

        // Change the user's password
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

        if (result.Succeeded)
        {
            // TODO: generate new jwt
            var jwt = await MakeJWT(user);
            
            return Ok(jwt);
        }
        
        // Should not get to this point!
        return BadRequest(
            new RestApiResponse
            {
                Message = RestApiErrorMessages.UserGeneralError,
                Status = HttpStatusCode.BadRequest
            });
        }


    private async Task<JWTResponse> MakeJWT(AppUser appUser)
    {
        
        appUser.AppRefreshTokens = await _context
            .Entry(appUser)
            .Collection(a => a.AppRefreshTokens!)
            .Query()
            .Where(t => t.AppUserId == appUser.Id)
            .ToListAsync();
        
        // remove expired REFRESH tokens!!
        foreach (var userRefreshToken in appUser.AppRefreshTokens)
        {
            if (userRefreshToken.ExpirtationDT < DateTime.UtcNow)
            {
                _context.AppRefreshTokens.Remove(userRefreshToken);
            }
        }
        
        var refreshToken = new AppRefreshToken
        {
            AppUserId = appUser.Id
        };
        _context.AppRefreshTokens.Add(refreshToken);
        
        await _context.SaveChangesAsync();
        // generate JWT
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        var jwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_KEY)!,
            Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_ISSUER)!,
            Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_AUDIENCE)!,
            10
        );
        var res = new JWTResponse()
        {
            JWT = jwt,
            RefreshToken = refreshToken.RefreshToken,
            AppUserId = appUser.Id.ToString()
        };

        return res;
    }

    /// <summary>
    /// Get all existing roles.
    /// </summary>
    /// <returns></returns>
    [HttpGet("Roles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IEnumerable<Public.DTO.Identity.AppRole>> GetAllRoles()
    {
        return (await _roleManager.Roles.ToListAsync()).Select(e => GetAppRoleMapper.Map(e));
    }

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IEnumerable<Public.DTO.Identity.AppUser>> GetAllUsers(bool deleted)
    {
        return (await _bll.UsersService.AllAsyncFiltered(deleted)).Select(e => GetUsersMapper.Map(e));
    }

    /// <summary>
    /// Add role to user
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = IdentityRolesConstants.ROLE_ADMIN)]
    [HttpPost("role")]
    public async Task<ActionResult<RestApiResponse>> AddRole([FromBody] AddRole data)
    {
        if (User.GetUserId() == data.UserId)
        {
            return BadRequest(new RestApiResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Message = "cant change its own roles"
            });
        }
        var role = await _roleManager.FindByIdAsync(data.RoleId.ToString());
        if (role == null)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = "ROLE_NOT_EXISTS",
                Status = HttpStatusCode.BadRequest
            });
        }

        var appUser = await _userManager.FindByIdAsync(data.UserId.ToString());

        if (appUser == null)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.BadRequest
            });
        }

        var roles = await _userManager.GetRolesAsync(appUser);
        await _userManager.RemoveFromRolesAsync(appUser, roles);

        await _userManager.AddToRoleAsync(appUser, role.Name);
        await _bll.SaveChangesAsync();
        return Ok();
    }
    
    
    
    /// <summary>
    /// Register user for unknown person. User details will be sent on email. (mail server not functioning yet)
    /// </summary>
    /// <param name="register"></param>
    /// <returns></returns>
    [HttpPost("{languageCulture}/RegisterUnknown")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = IdentityRolesConstants.ROLE_ADMIN)]
    public async Task<ActionResult> AdminRegister([FromBody] RegisterUnknown register, string languageCulture)
    {
        var RandomUserPassword = Guid.NewGuid().ToString();
        var user = await _context.Users.Where(x => x.UserName == register.Username || x.Email == register.Email)
            .FirstOrDefaultAsync();

        if (user != null)
        {
            if (user.NormalizedEmail == register.Email.ToUpper())
            {
                return BadRequest(
                    new RestApiResponse()
                    {
                        Status = HttpStatusCode.Conflict,
                        Message = RestApiErrorMessages.UserEmailAlreadyExists
                    });
            }
            if (user.NormalizedUserName == register.Username.ToUpper())
            {
                return BadRequest(
                    new RestApiResponse()
                    {
                        Status = HttpStatusCode.BadRequest,
                        Message =  RestApiErrorMessages.UserUsernameAlreadyExists  // "Username already registered!"
                    });
            }
        }
        // Register account
        var refreshToken = new AppRefreshToken();
        var appUser = new AppUser()
        {
            Firstname = register.Firstname,
            Lastname = register.Lastname,
            Email = register.Email,
            UserName = register.Username,
            AppRefreshTokens = new List<AppRefreshToken>() {refreshToken}
        };
        refreshToken.AppUser = appUser;
        
        
        var result = await _userManager.CreateAsync(appUser, RandomUserPassword);
        if (!result.Succeeded)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = RestApiErrorMessages.UserGeneralError,
                Status = HttpStatusCode.BadRequest
            });
        }

        var role = await _roleManager.FindByIdAsync(register.RoleId.ToString());
        if (role == null)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = RestApiErrorMessages.RoleNotFound,
                Status = HttpStatusCode.BadRequest
            });
        }
        await _userManager.AddToRoleAsync(appUser, role.Name);
        
        result = await _userManager.AddClaimsAsync(appUser, new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, appUser.Firstname),
                new Claim(ClaimTypes.Surname, appUser.Lastname),
                //new Claim(ClaimTypes.Role, IdentityRolesConstants.ROLE_MODERATOR)
            });

            if (!result.Succeeded)
            {
                return BadRequest(
                    new RestApiResponse()
                    {
                        Status = HttpStatusCode.BadRequest,
                        Message = "registration failed!"
                    });
            }

            // TODO: send email, if email is valid then saveChanges!!
            // TODO: does it save user even before saving changes?
            
            _bll.MailService.SendRegistration(register.Email, register.Username, RandomUserPassword, languageCulture);
            await _context.SaveChangesAsync();
            Console.WriteLine($"user random password = {RandomUserPassword}");
            
            return Ok();
    }

    /// <summary>
    /// Delete account
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost("Delete")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = IdentityRolesConstants.ROLE_ADMIN)]
    public async Task<ActionResult> DeleteAccount(UserIdDto data)
    {
        if (User.GetUserId() == data.UserId)
        {
            return BadRequest(new RestApiResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Message = RestApiErrorMessages.UserDeleteHimselfError
            });
        }
        var user = await _userManager.FindByIdAsync(data.UserId.ToString());
        if (user == null)
        {
            return NotFound(new RestApiResponse()
            {
                Status = HttpStatusCode.NotFound,
                Message = RestApiErrorMessages.GeneralNotFound
            });
        }
        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        await _userManager.DeleteAsync(user);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("ResetPassword")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = IdentityRolesConstants.ROLE_ADMIN)]
    public async Task<ActionResult> ResetPassword(UserIdDto data)
    {
        var user = await _userManager.FindByIdAsync(data.UserId.ToString());
        if (user == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }
        
        await _userManager.UpdateSecurityStampAsync(user);

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var newPassword = Guid.NewGuid().ToString();
        await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        _bll.MailService.SendForgotPassword(user.Email, newPassword);
        await _bll.SaveChangesAsync();
        return Ok();
    }
}