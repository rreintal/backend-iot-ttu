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

    [HttpPost]
    public async Task<string> SendEmail([FromBody] SendEmail data, string languageCulture)
    {
        // TODO
        var estBody = $"Tere, {data.RecipentEmail}! Olete avaldanud alla laadida $PROJECT_NAME seotud failid. $LINK?";
        var engBody =
            $"Hi, {data.RecipentEmail}! You have sent a request to download related files to $PROJECT_NAME. $LINK?";

        var body = languageCulture == LanguageCulture.ENG ? engBody : estBody;
        
        _sender.SendEmail(data.RecipentEmail, "TODO", body);
        return $"Sent message to {data.RecipentEmail}";
    }
}