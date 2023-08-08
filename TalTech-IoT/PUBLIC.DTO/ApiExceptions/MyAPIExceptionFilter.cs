using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Public.DTO.ApiExceptions;

public class MyAPIExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is ValueAlreadyExistsException indexConflictException)
        {
            context.ExceptionHandled = true;
            
            context.Result = new JsonResult(
                new RestApiResponse()
                {
                    Message = $"Topic Area '{indexConflictException.Name}' already exists. Can't add duplicate.",
                    StatusCode = (int)HttpStatusCode.Forbidden,
                });
        }
        // add other exceptions!
    }
}