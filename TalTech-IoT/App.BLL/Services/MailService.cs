using System.Net;
using System.Net.Mail;
using App.BLL.Contracts;
using App.Domain;
using Public.DTO.V1;

namespace App.BLL.Services;

public class MailService : IMailService
{
    private SmtpClient SmtpClient = new SmtpClient("smtp.gmail.com", 587);
    
    // TODO - ENV VARIABLE
    private readonly string Email = "dotnettestimine@gmail.com";
    private string Password = "qnjjyjavnikwtfib";

    private const string ACCESS_REPOSITORY_TEMPLATE_TITLE_EN = "IOT-TTU: Resource access request";
    private const string ACCESS_REPOSITORY_TEMPLATE_TITLE_ET = "IOT-TTU: Materjali ligipääs";

    private const string ACCESS_REPOSITORY_TEMPLATE_BODY_EN =
        "<!DOCTYPE html>\n<html lang=\"en\">\n\n<head>\n    <meta charset=\"UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Document</title>\n    <style>\n        .btn {\n            font-size: 0.8rem;\n            font-weight: 900;\n            text-transform: uppercase;\n            color: #fff;\n            border: 4px solid #e4067e;\n            line-height: 1.25rem;\n            background-color: #e4067e;\n            text-decoration: none;\n            text-align: center;\n            padding: 0.5rem;\n            min-width: 8rem;\n            border-radius: 60px;\n            display: inline-block;\n            /* Corrected from \"inline-block\"; */\n        }\n\n        .header,\n        .footer {\n            height: 60px;\n            background-color: #342b60;\n        }\n\n        .content {\n            background-color: white;\n            font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;\n            font-weight: 400;\n            text-align: center;\n        }\n\n        span {\n            color: #e4067e;\n        }\n\n        .link {\n            font-weight: 800;\n        }\n    </style>\n</head>\n\n<body style=\"margin: 0; padding: 0;\">\n    <table role=\"presentation\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin: auto;\">\n        <tr>\n            <td class=\"header\" style=\"height: 60px; background-color: #342b60;\">&nbsp;</td>\n        </tr>\n        <tr>\n            <td class=\"content\"\n                style=\"background-color: white; font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif; font-weight: 400; text-align: center;\">\n                <h1 style=\"margin: 0; padding: 0;\">Hello <span class=\"name\" style=\"color: #e4067e;\">$NAME</span></h1>\n                <p style=\"margin: 0; padding: 0; display: inline-block;\">You have requested access to</p> <span\n                    class=\"link\" style=\"font-weight: 800; color: #e4067e;\">$RESOURCE_NAME</span>\n                <a href=\"https://$LINK\" style=\"display: block; color: #e4067e; text-decoration: none;\">$LINK</a>\n            </td>\n        </tr>\n        <tr>\n            <td class=\"footer\" style=\"height: 60px; background-color: #342b60;\">&nbsp;</td>\n        </tr>\n    </table>\n</body>\n\n</html>";

    private const string ACCESS_REPOSITORY_TEMPLATE_BODY_ET =
        "<!DOCTYPE html>\n<html lang=\"en\">\n\n<head>\n    <meta charset=\"UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Document</title>\n    <style>\n        .btn {\n            font-size: 0.8rem;\n            font-weight: 900;\n            text-transform: uppercase;\n            color: #fff;\n            border: 4px solid #e4067e;\n            line-height: 1.25rem;\n            background-color: #e4067e;\n            text-decoration: none;\n            text-align: center;\n            padding: 0.5rem;\n            min-width: 8rem;\n            border-radius: 60px;\n            display: inline-block;\n            /* Corrected from \"inline-block\"; */\n        }\n\n        .header,\n        .footer {\n            height: 60px;\n            background-color: #342b60;\n        }\n\n        .content {\n            background-color: white;\n            font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;\n            font-weight: 400;\n            text-align: center;\n        }\n\n        span {\n            color: #e4067e;\n        }\n\n        .link {\n            font-weight: 800;\n        }\n    </style>\n</head>\n\n<body style=\"margin: 0; padding: 0;\">\n    <table role=\"presentation\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin: auto;\">\n        <tr>\n            <td class=\"header\" style=\"height: 60px; background-color: #342b60;\">&nbsp;</td>\n        </tr>\n        <tr>\n            <td class=\"content\"\n                style=\"background-color: white; font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif; font-weight: 400; text-align: center;\">\n                <h1 style=\"margin: 0; padding: 0;\">Tere <span class=\"name\" style=\"color: #e4067e;\">$NAME</span></h1>\n                <p style=\"margin: 0; padding: 0; display: inline-block;\">Teie soovitud materjal</p> <span\n                    class=\"link\" style=\"font-weight: 800; color: #e4067e;\">$RESOURCE_NAME</span>\n                <a href=\"https://$LINK\" style=\"display: block; color: #e4067e; text-decoration: none;\">$LINK</a>\n            </td>\n        </tr>\n        <tr>\n            <td class=\"footer\" style=\"height: 60px; background-color: #342b60;\">&nbsp;</td>\n        </tr>\n    </table>\n</body>\n\n</html>";
    

    private const string CONTACT_US_TEMPLATE_ET = "Nimi: $FIRSTNAME $LASTNAME \n" +
                                               "Email: $SENDER_EMAIL \n" +
                                               "Sõnum: $BODY";

    private const string REGISTRATION_SUBJECT_ET = "IOT-TTU: Kasutaja kinnitamine";
    private const string REGISTRATION_SUBJECT_EN = "IOT-TTU: Account confirmation";

    private const string REGISTRATION_BODY_ET = "Teie parool on: $PASSWORD";
    private const string REGISTRATION_BODY_EN = "Your password is: $PASSWORD";
                                               
    public MailService()
    {
        SmtpClient.UseDefaultCredentials = false;
        SmtpClient.Credentials = new NetworkCredential(Email, Password);
        SmtpClient.EnableSsl = true;
    }
    
    public void SendRegistration(string recipentMail, string username, string password, string languageCulture)
    {
        MailMessage mail = new MailMessage();
        var subjectTemplate = ChooseTemplate(REGISTRATION_SUBJECT_ET, REGISTRATION_SUBJECT_EN, languageCulture);
        var bodyTemplate = ChooseTemplate(REGISTRATION_BODY_ET, REGISTRATION_BODY_EN, languageCulture);
        mail.From = new MailAddress(Email);
        mail.To.Add(recipentMail);
        mail.Subject = subjectTemplate;
        mail.Body = MakeRegistrationBody(password, bodyTemplate);
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

    public void SendContactUs(ContactForm data)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(Email);
        mail.To.Add(Email); // TODO: add email of the admin!
        var bodyTemplate = 

        mail.Subject = $"Contact us - {DateTime.UtcNow}";
        mail.Body = MakeContactUsBody(data);
        SmtpClient.Send(mail);

    }

    public void AccessResource(string recipentMail, string resourceName, string link, string languageCulture)
    {
        MailMessage mail = new MailMessage();
        var subjectTemplate = ChooseTemplate(ACCESS_REPOSITORY_TEMPLATE_TITLE_ET,ACCESS_REPOSITORY_TEMPLATE_TITLE_EN, languageCulture);
        var bodyTemplate = ChooseTemplate(ACCESS_REPOSITORY_TEMPLATE_BODY_EN, ACCESS_REPOSITORY_TEMPLATE_BODY_EN, languageCulture);
        mail.From = new MailAddress(Email);
        mail.To.Add(recipentMail);
        mail.Subject = subjectTemplate;
        mail.Body = MakeRequestResourceBody(bodyTemplate, recipentMail, resourceName, link);
        SmtpClient.Send(mail);
    }

    private string ChooseTemplate(string templateET, string templateEN, string languageCulture)
    {
        switch (languageCulture)
        {
            case LanguageCulture.ENG:
                return templateEN;
            case LanguageCulture.EST:
                return templateET;
        }

        throw new Exception("Not supported languageCulture");
    }

    private string MakeRequestResourceBody(string template, string name, string resourceName, string link)
    {
        return template
            .Replace("$NAME", name)
            .Replace("$RESOURCE_NAME", resourceName)
            .Replace("$LINK", link);
    }

    private string MakeContactUsBody(ContactForm data)
    {
        return CONTACT_US_TEMPLATE_ET
            .Replace("$SENDER_EMAIL", data.Email)
            .Replace("$FIRSTNAME", data.FirstName)
            .Replace("$LASTNAME", data.LastName)
            .Replace("$BODY", data.MessageText);
    }

    private string MakeRegistrationBody(string password, string template)
    {
        // Without encode, in gmail it will be displayed as some random string (not guid)
        return template.Replace("$PASSWORD", WebUtility.HtmlEncode(password));
    }
}