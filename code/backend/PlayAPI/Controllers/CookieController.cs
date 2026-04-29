using Microsoft.AspNetCore.Mvc;
using PlayAPI.Features.Cookies;

namespace PlayAPI.Controllers;

[ApiController]
[Route("play/cookies")]
public class CookieController : ControllerBase
{
    private readonly ICookieService _cookieService;

    public CookieController(ICookieService cookieService)
    {
        _cookieService = cookieService;
    }

    [HttpPost("consent")]
    [Tags("Cookie")]
    public IActionResult SetCookieConsent([FromBody] SetCookieConsentRequest request)
    {
        return Ok(new CookieConsentResponse(request.Consent));
    }

    [HttpGet("consent")]
    [Tags("Cookie")]
    public IActionResult CheckCookieConsent()
    {
        var consent = _cookieService.CheckCookieConsent(HttpContext);
        return Ok(new CookieConsentResponse(consent));
    }

    [HttpDelete("consent")]
    [Tags("Cookie")]
    public IActionResult RemoveCookieConsent()
    {
        return NoContent();
    }
}

public sealed record SetCookieConsentRequest(bool Consent);

public sealed record CookieConsentResponse(bool Consent);
