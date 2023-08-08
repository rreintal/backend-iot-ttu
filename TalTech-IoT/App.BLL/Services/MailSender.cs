using System.Net;
using System.Net.Mail;
using App.BLL.Contracts;

namespace App.BLL.Services;

public class MailSender : IMailService
{
    private SmtpClient SmtpClient = new SmtpClient("smtp.gmail.com", 587);
    
    // TODO - ENV VARIABLE
    private readonly string Email = "dotnettestimine@gmail.com";
    private string Password = "kwdzedzwsiuktszw";
    
    public MailSender()
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

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(Email);
        mail.To.Add(recipentEmail);
        mail.Subject = subject;
        mail.Body = body;
        
        SmtpClient.Send(mail);
    }
}