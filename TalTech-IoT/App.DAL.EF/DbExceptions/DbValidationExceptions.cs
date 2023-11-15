namespace App.DAL.EF.DbExceptions;

public class DbValidationExceptions : Exception
{
    public string ErrorMessage { get; set; } = default!;

    public int ErrorCode { get; set; } = default;
}