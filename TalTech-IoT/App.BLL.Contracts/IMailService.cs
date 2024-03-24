namespace App.BLL.Contracts;

public interface IMailService
{
    public void SendEmail(string recipentEmail, string subject, string body);

    public void SendRegistration(string recipentMail, string username, string password);

    public void SendForgotPassword(string recipentMail, string password);
}