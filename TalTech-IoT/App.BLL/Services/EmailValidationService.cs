using System.Net.Mail;
using App.BLL.Contracts;

namespace App.BLL.Services;

public class EmailValidationService : IEmailValidationService
{
    public bool IsValid(string emailaddress)
    {
        try
        {
            MailAddress m = new MailAddress(emailaddress);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}