using PaintyTask.Api.Helpers.Interfaces;

namespace PaintyTask.Api.Helpers;

public class AuthorizationHelper : IAuthorizationHelper
{
    public AuthorizationHelper()
    {
    }

    public string GetTokenFromAuthorizationHeader(HttpContext httpContext)
    {
        var token = httpContext.Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ")
            .Last();

        return token 
            ?? throw new UnauthorizedAccessException("You are not authorised");
    }
}