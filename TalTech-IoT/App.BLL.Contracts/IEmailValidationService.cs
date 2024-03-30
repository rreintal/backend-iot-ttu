namespace App.BLL.Contracts;

public interface IEmailValidationService
{
    public bool IsValid(string emailaddress);
}