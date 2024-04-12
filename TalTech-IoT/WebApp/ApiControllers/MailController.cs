using System.Net.Mail;
using App.BLL;
using App.BLL.Contracts;
using App.DAL.EF;
using App.Domain;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO.V1;

namespace WebApp.ApiControllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/{languageCulture}/[controller]/[action]")]
[ApiController]
public class MailController : ControllerBase
{
    private IAppBLL _Bll { get; }
    private AppDbContext _context { get; } // TODO: make service for EmailRecipents!!
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bll"></param>
    public MailController(IAppBLL bll, AppDbContext context)
    {
        _Bll = bll;
        _context = context;
    }

    /// <summary>
    /// Used to send Contact Us to the page administrator
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> Contact([FromBody] ContactForm data, [FromQuery] Guid? fromNews)
    {
        var recipents = await _context.EmailRecipents.ToListAsync();
        _Bll.MailService.SendContactUs(data, recipents, fromNews);
        return Ok();
    }
}