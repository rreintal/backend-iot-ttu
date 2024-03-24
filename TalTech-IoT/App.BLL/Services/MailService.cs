using System.Net;
using System.Net.Mail;
using App.BLL.Contracts;

namespace App.BLL.Services;

public class MailService : IMailService
{
    private SmtpClient SmtpClient = new SmtpClient("smtp.gmail.com", 587);
    
    // TODO - ENV VARIABLE
    private readonly string Email = "dotnettestimine@gmail.com";
    private string Password = "qnjjyjavnikwtfib";

    private const string template =
        "<!DOCTYPE html>\n<html lang=\"en\">\n\n<head>\n    <meta charset=\"UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Document</title>\n    <style>\n        .btn {\n            font-size: 0.8rem;\n            font-weight: 900;\n            text-transform: uppercase;\n            color: #fff;\n            border: 4px solid #e4067e;\n            line-height: 1.25rem;\n            background-color: #e4067e;\n            text-decoration: none;\n            text-align: center;\n            padding: 0.5rem;\n            min-width: 8rem;\n            border-radius: 60px;\n            display: inline-block;\n            /* Corrected from \"inline-block\"; */\n        }\n\n        .header,\n        .footer {\n            height: 60px;\n            background-color: #342b60;\n        }\n\n        .content {\n            background-color: white;\n            font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;\n            font-weight: 400;\n            text-align: center;\n        }\n\n        span {\n            color: #e4067e;\n        }\n\n        .link {\n            font-weight: 800;\n        }\n    </style>\n</head>\n\n<body>\n    <table width=\"100%\" cellspacing=\"20\" cellpadding=\"0\">\n        <tr>\n            <td class=\"header\">&nbsp;</td>\n        </tr>\n        <tr>\n            <td class=\"content\">\n                <h1>Hello <span class=\"name\">$NAME</span></h1>\n                <p style=\"display: inline-block;\">You have requested access to</p> <span class=\"link\">$RESOURCE_NAME</span>\n                <a style=\"display: block;\" href=\"https://$LINK\">$LINK</a>\n            </td>\n        </tr>\n        <tr>\n            <td class=\"footer\">&nbsp;</td>\n        </tr>\n    </table>\n</body>\n\n</html>";
    
    public MailService()
    {
        SmtpClient.UseDefaultCredentials = false;
        SmtpClient.Credentials = new NetworkCredential(Email, Password);
        SmtpClient.EnableSsl = true;
    }
    
    // TODO
    public void SendEmail(string recipentEmail, string subject, string body)
    {
        // TODO - error handling
        // invalid email etc.

        var content = template
            .Replace("$NAME", recipentEmail)
            .Replace("$RESOURCE_NAME", subject)
            .Replace("$LINK", body);
        
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(Email);
        mail.To.Add(recipentEmail);
        mail.Subject = "IOT-TTU Invitation";
        mail.Body = content;
        mail.IsBodyHtml = true;
        SmtpClient.Send(mail);
    }

    public void SendRegistration(string recipentMail, string username, string password)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(Email);
        
        mail.To.Add(recipentMail);
        mail.Subject = $"IOT-TTU: Account confirmation";
        
        // Without encode, in gmail it will be displayed as some random string (not guid)
        mail.Body = $"Your temporary password is: {WebUtility.HtmlEncode(password)}"; 
        SmtpClient.Send(mail);
    }

    public void SendForgotPassword(string recipentMail, string password)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(Email);
        mail.To.Add(recipentMail);
        mail.Subject = $"{recipentMail} forgot password.";
        mail.Body = $"Your new password is: {password}";
        
        SmtpClient.Send(mail);
    }
}