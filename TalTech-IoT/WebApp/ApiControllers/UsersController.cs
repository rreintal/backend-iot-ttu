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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO;
using Public.DTO.Identity;
using Public.DTO.V1.Mappers;
using AppRole = App.Domain.Identity.AppRole;
using AppUser = App.Domain.Identity.AppUser;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller for users authorization and registration service.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _Configuration;
    private readonly AppDbContext _context;
    private readonly IAppBLL _bll;
    private readonly RoleManager<AppRole> _roleManager;


    // TODO: how to remove this yellow line and make it with private documentation?
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
                        Message = "Email already registered!"
                    });
            }
            if (user.NormalizedUserName == register.Username.ToUpper())
            {
                return BadRequest(
                    new RestApiResponse()
                    {
                        Status = HttpStatusCode.Conflict,
                        Message = "Username already registered!"
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
                Message = "PROBLEM WITH PASSWORD", // TODO: for testing!
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
                new Claim(ClaimTypes.Role, IdentityRolesConstants.ROLE_USER)
            });

            if (!result.Succeeded)
            {
                return BadRequest(
                    new RestApiResponse()
                    {
                        Status = HttpStatusCode.MethodNotAllowed,
                        Message = "registration failed!"
                    });
            }

            // generate JWT
            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
            var jwt = IdentityHelpers.GenerateJwt(
                claimsPrincipal.Claims,
                _Configuration.GetValue<string>(StartupConfigConstants.JWT_KEY)!,
                _Configuration.GetValue<string>(StartupConfigConstants.JWT_ISSUER)!,
                _Configuration.GetValue<string>(StartupConfigConstants.JWT_AUDIENCE)!,
                _Configuration.GetValue<int>(StartupConfigConstants.JWT_EXPIRATION_TIME)

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
    /// <param name="payload"></param>
    /// <returns></returns>
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
                Message = "Login failed!"
            });
        }
        
        // verify password
        
        
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, login.Password, false);
        if (!result.Succeeded)
        {
            return NotFound(new RestApiResponse()
            {
                Status = HttpStatusCode.NotFound,
                Message = "WRONG PASSWORD" // TODO: for testing!
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
            _Configuration.GetValue<string>(StartupConfigConstants.JWT_KEY)!,
            _Configuration.GetValue<string>(StartupConfigConstants.JWT_ISSUER)!,
            _Configuration.GetValue<string>(StartupConfigConstants.JWT_AUDIENCE)!,
            _Configuration.GetValue<int>(StartupConfigConstants.JWT_EXPIRATION_TIME)
        );

        //var userRoles = await _userManager.GetRolesAsync(appUser);
        var userRoles = await _roleManager.Roles
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.AppUser)
            .Where(x => x.UserRoles.Any(ur => ur.AppUser!.Id == appUser.Id))
            .ToListAsync();
        
        
        var res = new JWTResponse()
        {
            JWT = jwt,
            RefreshToken = refreshToken.RefreshToken,
            AppUserId = appUser.Id.ToString(),
            Username = appUser.UserName,
            RoleIds = userRoles.Select(e => e.Id)
        };

        
        return Ok(res);
    }

    /// <summary>
    /// Refresh JWT
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
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
        
        // validate JWT, is it correct?
        if (!IdentityHelpers.ValidateToken(
                refreshTokenModel.Jwt,
                _Configuration.GetValue<string>(StartupConfigConstants.JWT_KEY)!,
                _Configuration.GetValue<string>(StartupConfigConstants.JWT_ISSUER)!,
                _Configuration.GetValue<string>(StartupConfigConstants.JWT_AUDIENCE)!))
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

        var refreshTokenFromDatabase = await _context.AppRefreshTokens.Where(t => t.RefreshToken == refreshTokenModel.RefreshToken)
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
            _Configuration[StartupConfigConstants.JWT_KEY]!,
            _Configuration[StartupConfigConstants.JWT_ISSUER]!,
            _Configuration[StartupConfigConstants.JWT_AUDIENCE]!,
            _Configuration.GetValue<int>(StartupConfigConstants.JWT_EXPIRATION_TIME)
            );

        
        // add new refreshtoken!
        var newRefreshToken = new AppRefreshToken();
        newRefreshToken.AppUserId = appUser.Id;
        
        // add new token to db (kas on vaja või see handlib kuidagi ise??)
        await _context.AppRefreshTokens.AddAsync(newRefreshToken);
        
        // remove refreshToken which was used to ask for new JWT!!
        _context.AppRefreshTokens.Remove(refreshTokenFromDatabase);
        
        
        await _context.SaveChangesAsync();

        
        // Idk kas siia on appUserId ka vaja v ei
        var res = new JWTResponse()
        {
            JWT = jwt,
            RefreshToken = newRefreshToken.RefreshToken,
            Username = appUser.UserName
        };

        return Ok(res);
    }

    /// <summary>
    /// Log out
    /// </summary>
    /// <param name="payload"></param>
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
                Message = "Username/password error"
            });
        }
        
        var userRefreshTokens = await _context.AppRefreshTokens
            .Where(x => x.AppUserId == userId)
            .ToListAsync();
        
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
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return BadRequest(
                new RestApiResponse
                {
                    Message = "User not found.",
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
        
        // Password change failed
        return BadRequest(
            new RestApiResponse
            {
                Message = "CHANGE_PASSWORD_FAILED",
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
            _Configuration.GetValue<string>(StartupConfigConstants.JWT_KEY)!,
            _Configuration.GetValue<string>(StartupConfigConstants.JWT_ISSUER)!,
            _Configuration.GetValue<string>(StartupConfigConstants.JWT_AUDIENCE)!,
            _Configuration.GetValue<int>(StartupConfigConstants.JWT_EXPIRATION_TIME)
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
    public async Task<IEnumerable<Public.DTO.Identity.AppRole>> GetAllRoles()
    {
        return (await _roleManager.Roles.ToListAsync()).Select(e => GetAppRoleMapper.Map(e));
    }

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns></returns>
    //[Authorize]
    [HttpGet]
    public async Task<IEnumerable<Public.DTO.Identity.AppUser>> GetAllUsers()
    {
        return (await _bll.UsersService.AllAsync()).Select(e => GetUsersMapper.Map(e));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
    [HttpPost("role")]
    public async Task<ActionResult<RestApiResponse>> AddRole([FromBody] AddRole data)
    {
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
    
    
    // TODO: register method for admin, where he puts in the FN, LN, Email, and then register account
    // and generate random pw (UUID), send this user details to recipent on email!

    
    
    /// <summary>
    /// Suspend an User
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPost("Lock")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = IdentityRolesConstants.ROLE_ADMIN)]
    public async Task<ActionResult> LockAccount(Guid userId)
    {
        var user = await _userManager.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        user.LockoutEnabled = true;
        user.LockoutEnd = DateTimeOffset.MaxValue;
        await _bll.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("Unlock")]
    public async Task<ActionResult> UnlockAccount(Guid userId)
    {
        var user = await _userManager.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound(new RestApiResponse()
            {
                Message = RestApiErrorMessages.GeneralNotFound,
                Status = HttpStatusCode.NotFound
            });
        }

        user.LockoutEnabled = false;
        user.LockoutEnd = null;

        await _bll.SaveChangesAsync();
        return Ok();
    }

}