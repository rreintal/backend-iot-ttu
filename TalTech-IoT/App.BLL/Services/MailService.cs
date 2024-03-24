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