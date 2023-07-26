using App.BLL;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.V1;

namespace WebApp.ApiControllers;

[Route("api/{languageCulture}/[controller]/[action]")]
public class MailController : ControllerBase
{

    private MailSender _sender { get; set; }
    
    public MailController()
    {
        _sender = new MailSender();
    }

    /// <summary>
    /// Used to send email to user with repository download link. (HTTPS or .git) | NOT FINISHED!
    /// </summary>
    /// <param name="data"></param>
    /// <param name="languageCulture"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> Send([FromBody] SendEmail data, string languageCulture)
    {
        // TODO
        var estBody = $"Tere, {data.RecipentEmail}! Olete avaldanud alla laadida $PROJECT_NAME seotud failid. $LINK?";
        var engBody =
            $"Hi, {data.RecipentEmail}! You have sent a request to download related files to $PROJECT_NAME. $LINK?";

        var body = languageCulture == LanguageCulture.ENG ? engBody : estBody;
        
        _sender.SendEmail(data.RecipentEmail, "TODO", body);
        return $"Sent message to {data.RecipentEmail}";
    }

    /// <summary>
    /// Used to send Contact Us to the page administrator
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> Contact([FromBody] ContactForm data)
    {
        // TODO - _sender sends this to admin
        return $"'{data.Email}' wants to contact. \n Message: '{data.MessageText}'";
    }
}