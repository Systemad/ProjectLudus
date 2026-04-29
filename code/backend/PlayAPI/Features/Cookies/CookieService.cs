using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace PlayAPI.Features.Cookies;

public class CookieService : ICookieService
{
    private const string ConsentCookieName = "AnalyticsConsent";

    public bool CheckCookieConsent(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue(ConsentCookieName, out var value))
        {
            try
            {
                var decoded = HttpUtility.UrlDecode(value);
                using var doc = JsonDocument.Parse(decoded);
                var categories = doc.RootElement.GetProperty("categories").EnumerateArray();
                return categories.Any(c => c.GetString() == "analytics");
            }
            catch
            {
                return false;
            }
        }

        return false;
    }

    public Guid? GetSessionId(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue(ConsentCookieName, out var value))
        {
            try
            {
                var decoded = HttpUtility.UrlDecode(value);
                using var doc = JsonDocument.Parse(decoded);
                var consentId = doc.RootElement.GetProperty("consentId").GetString();
                if (Guid.TryParse(consentId, out var sessionId))
                {
                    return sessionId;
                }
            }
            catch { }
        }

        return null;
    }
}
