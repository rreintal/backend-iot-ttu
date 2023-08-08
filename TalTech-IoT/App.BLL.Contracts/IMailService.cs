namespace App.BLL.Contracts;

public interface IMailService
{
    public void SendEmail(string recipentEmail, string subject, string body);
}