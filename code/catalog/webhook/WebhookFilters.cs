using Microsoft.Extensions.Options;

public class WebhookSecretFilter(IOptions<IGDBOptions> options) : IEndpointFilter
{
    private readonly IOptions<IGDBOptions> _options = options;

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var httpContext = context.HttpContext;
        var userAgent = httpContext.Request.Headers.UserAgent;
        var secret = httpContext.Request.Headers["X-Secret"];

        if (secret != _options.Value.WEBHOOK_SECRET && userAgent != "IGDB-Webhook-Bot")
            return Results.Unauthorized();

        return await next(context);
    }
}
