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
        "<!DOCTYPE html>\n<html lang=\"en\">\n\n<head>\n    <meta charset=\"UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Document</title>\n    <style>\n        .header,\n        .footer {\n            height: 60px;\n            background-color: #342b60;\n        }\n\n        .content {\n            background-color: white;\n            font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;\n            font-weight: 400;\n            text-align: center;\n        }\n\n        .bold {\n            font-weight: 700;\n        }\n\n        .link {\n            font-weight: 800;\n        }\n    </style>\n</head>\n\n<body>\n    <table width=\"100%\" cellspacing=\"70\" cellpadding=\"10\">\n        <tr>\n            <td class=\"header\">&nbsp;</td>\n        </tr>\n        <tr>\n            <td class=\"content\">\n                <h1>Hello $NAME</h1>\n                <p style=\"display: inline-block;\">You have requested access to <span class=\"bold\">$RESOURCE_NAME</span></p>\n                <a style=\"display: block;\" href=\"$LINK\">$LINK</a>\n            </td>\n        </tr>\n        <tr>\n            <td class=\"footer\">&nbsp;</td>\n        </tr>\n    </table>\n</body>\n\n</html>";

    private const string ACCESS_REPOSITORY_TEMPLATE_BODY_ET =
        "<!DOCTYPE html>\n<html lang=\"en\">\n\n<head>\n    <meta charset=\"UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Document</title>\n    <style>\n        .header,\n        .footer {\n            height: 60px;\n            background-color: #342b60;\n        }\n\n        .content {\n            background-color: white;\n            font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;\n            font-weight: 400;\n            text-align: center;\n        }\n\n        .bold {\n            font-weight: 700;\n        }\n\n        .link {\n            font-weight: 800;\n        }\n    </style>\n</head>\n\n<body>\n    <table width=\"100%\" cellspacing=\"70\" cellpadding=\"10\">\n        <tr>\n            <td class=\"header\">&nbsp;</td>\n        </tr>\n        <tr>\n            <td class=\"content\">\n                <h1>Tere $NAME</h1>\n                <p style=\"display: inline-block;\">Allpool olev link suunab Teid soovitud materjalile <span class=\"bold\">$RESOURCE_NAME</span></p>\n                <a style=\"display: block;\" href=\"$LINK\">$LINK</a>\n            </td>\n        </tr>\n        <tr>\n            <td class=\"footer\">&nbsp;</td>\n        </tr>\n    </table>\n</body>\n\n</html>";
    

    private const string CONTACT_US_TEMPLATE_ET = "Nimi: $FIRSTNAME $LASTNAME \n" +
                                               "Email: $SENDER_EMAIL \n" +
                                               "Sõnum: $BODY";

    private const string REGISTRATION_SUBJECT_ET = "IOT-TTU: Kasutaja kinnitamine";
    private const string REGISTRATION_SUBJECT_EN = "IOT-TTU: Account confirmation";

    private const string REGISTRATION_BODY_ET = "<!DOCTYPE html>\n<html lang=\"en\">\n\n<head>\n    <meta charset=\"UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Document</title>\n    <style>\n        .header,\n        .footer {\n            height: 60px;\n            background-color: #342b60;\n        }\n\n        .content {\n            background-color: white;\n            font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;\n            font-weight: 400;\n            text-align: center;\n        }\n\n        .bold {\n            font-weight: 700;\n        }\n\n        .link {\n            font-weight: 800;\n        }\n    </style>\n</head>\n\n<body>\n    <table width=\"100%\" cellspacing=\"70\" cellpadding=\"10\">\n        <tr>\n            <td class=\"header\">&nbsp;</td>\n        </tr>\n        <tr>\n            <td class=\"content\">\n                <p style=\"display: inline-block; margin: 0; padding: 0;\">Teie uus parool on: <span class=\"bold\"\n                        style=\"font-weight: bold;\">$PASSWORD</span></p>\n                <p style=\"color: #ff6b6b; margin: 0; padding: 0;\">Palun vahetage parool sisselogimise järel turvalisuse tagamiseks!</p>\n            </td>\n        </tr>\n        <tr>\n            <td class=\"footer\">&nbsp;</td>\n        </tr>\n    </table>\n</body>\n\n</html>";
    private const string REGISTRATION_BODY_EN = "<!DOCTYPE html>\n<html lang=\"en\">\n\n<head>\n    <meta charset=\"UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Document</title>\n    <style>\n        .header,\n        .footer {\n            height: 60px;\n            background-color: #342b60;\n        }\n\n        .content {\n            background-color: white;\n            font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;\n            font-weight: 400;\n            text-align: center;\n        }\n\n        .bold {\n            font-weight: 700;\n        }\n\n        .link {\n            font-weight: 800;\n        }\n    </style>\n</head>\n\n<body>\n    <table width=\"100%\" cellspacing=\"70\" cellpadding=\"10\">\n        <tr>\n            <td class=\"header\">&nbsp;</td>\n        </tr>\n        <tr>\n            <td class=\"content\">\n                <p style=\"display: inline-block; margin: 0; padding: 0;\">Your new password is: <span class=\"bold\"\n                        style=\"font-weight: bold;\">$PASSWORD</span></p>\n                <p style=\"color: #ff6b6b; margin: 0; padding: 0;\">Please change your password after logging in for security purposes!\n                </p>\n\n            </td>\n        </tr>\n        <tr>\n            <td class=\"footer\">&nbsp;</td>\n        </tr>\n    </table>\n</body>\n\n</html>";
                                               
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
        mail.IsBodyHtml = true;
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
        mail.IsBodyHtml = true;
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