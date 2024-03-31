using System.Net;
using App.BLL.Contracts;
using App.DAL.EF;
using App.Domain;
using App.Domain.Constants;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO;
using Public.DTO.V1;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller for adding persons Contact Us recipents mail
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]

public class EmailRecipentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAppBLL _bll;

    /// <inheritdoc />
    public EmailRecipentsController(AppDbContext context, IAppBLL bll)
    {
        _context = context;
        _bll = bll;
    }

    /// <summary>
    /// Add new
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Add(EmailRecipent data)
    {
        var isValidEmail = _bll.EmailValidationService.IsValid(data.Email); 
        if (!isValidEmail)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = RestApiErrorMessages.InvalidEmail,
                Status = HttpStatusCode.BadRequest
            });
        }   
        var emailAlreadyTaken = _context.EmailRecipents.Any(e => e.Email == data.Email);
        if (emailAlreadyTaken)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = RestApiErrorMessages.AlreadyExists,
                Status = HttpStatusCode.BadRequest
            });
        }
        var dto = new EmailRecipents()
        {
            FirstName = data.FirstName,
            LastName = data.LastName,
            Email = data.Email,
            Comment = data.Comment
        };

        _context.EmailRecipents.Add(dto);
        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Remove from recipents list
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Remove(Guid id)
    {
        var entity = await _context.EmailRecipents.FirstOrDefaultAsync(entity => entity.Id == id);
        if (entity == null)
        {
            return NotFound();
        }

        _context.EmailRecipents.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Update Email recipent
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Update(EmailRecipent data)
    {
        var entity = await _context.EmailRecipents.FirstOrDefaultAsync(entity => entity.Id == data.Id);
        if (entity == null)
        {
            return NotFound();
        }

        entity.Email = data.Email;
        entity.FirstName = data.FirstName;
        entity.LastName = data.LastName;
        entity.Comment = data.Comment;
        
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    /// <summary>
    /// Get all email recipents
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpGet("all")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<List<EmailRecipent>>> GetAll()
    {
        var data = await _context.EmailRecipents.ToListAsync();
        return data.Select(
            e => new EmailRecipent()
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Comment = e.Comment
            }).ToList();
    }
}