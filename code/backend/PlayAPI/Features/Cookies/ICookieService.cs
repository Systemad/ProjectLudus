using Microsoft.AspNetCore.Http;

namespace PlayAPI.Features.Cookies;

public interface ICookieService
{
    bool CheckCookieConsent(HttpContext httpContext);
    Guid? GetSessionId(HttpContext httpContext);
}
