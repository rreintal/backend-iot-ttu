using App.Domain;
using Public.DTO.V1;

namespace App.BLL.Contracts;

public interface IMailService
{
    public void SendRegistration(string recipentMail, string username, string password, string languageCulture);

    public void SendForgotPassword(string recipentMail, string password);

    public void SendContactUs(ContactForm data, List<EmailRecipents> emailRecipentsList);

    public void AccessResource(string recipentMail, string resourceName, string link, string languageCulture);
}