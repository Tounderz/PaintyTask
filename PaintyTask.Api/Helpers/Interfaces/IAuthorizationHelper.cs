namespace PaintyTask.Api.Helpers.Interfaces;

public interface IAuthorizationHelper
{
    string GetTokenFromAuthorizationHeader(HttpContext httpContext);
}