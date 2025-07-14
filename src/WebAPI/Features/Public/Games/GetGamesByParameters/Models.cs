using FastEndpoints;
using WebAPI.Features.Common.Endpoints;

namespace Public.Games.GetGamesByParameters;

public class GameSearchRequest : IPaginationParameters
{
    [QueryParam]
    public int PageSize { get; set; } = 40;

    [QueryParam]
    public int PageNumber { get; set; } = 1;

    public string? Name { get; set; } = null;

    public long[]? GenreId { get; set; } = null;

    public long[]? PlatformId { get; set; } = null;

    public long[]? GameModeId { get; set; } = null;

    public long[]? ThemeId { get; set; } = null;

    public long[]? GameTypeId { get; set; } = null;

    public long[]? PlayerPerspectiveId { get; set; } = null;
}
