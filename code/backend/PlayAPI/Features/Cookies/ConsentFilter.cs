using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace PlayAPI.Features.Cookies;

public class ConsentFilter : IEndpointFilter
{
    private readonly ICookieService _cookieService;

    public ConsentFilter(ICookieService cookieService)
    {
        _cookieService = cookieService;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var httpContext = context.HttpContext;

        if (!_cookieService.CheckCookieConsent(httpContext))
        {
            return Results.NoContent();
        }

        return await next(context);
    }
}
