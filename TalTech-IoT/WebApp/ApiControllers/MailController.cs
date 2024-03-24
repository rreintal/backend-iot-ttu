using System.Net.Mail;
using App.BLL;
using App.BLL.Contracts;
using App.Domain;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.V1;

namespace WebApp.ApiControllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/{languageCulture}/[controller]/[action]")]
[ApiController]
public class MailController : ControllerBase
{
    private IAppBLL _Bll { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bll"></param>
    public MailController(IAppBLL bll)
    {
        _Bll = bll;
    }

    /// <summary>
    /// Used to send Contact Us to the page administrator
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult Contact([FromBody] ContactForm data)
    {
        _Bll.MailService.SendContactUs(data);
        return Ok();
    }
}