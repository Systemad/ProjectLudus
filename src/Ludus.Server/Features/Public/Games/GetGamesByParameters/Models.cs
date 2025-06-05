using Ludus.Server.Features.Common.Endpoints;
using Ludus.Server.Features.Common.Games.Models;
using Ludus.Server.Features.Public.Games.Common;

namespace Ludus.Server.Features.Public.Games.GetGamesByParameters;

public class GameSearchRequest : IPaginationParameters
{
    public int PageSize { get; set; } = 40;

    public int PageNumber { get; set; } = 1;

    public string? Name { get; set; } = null;

    public long[]? GenreId { get; set; } = null;

    public long[]? PlatformId { get; set; } = null;

    public long[]? GameModeId { get; set; } = null;

    public long[]? ThemeId { get; set; } = null;

    public long[]? GameTypeId { get; set; } = null;

    public long[]? PlayerPerspectiveId { get; set; } = null;
}

public record GetSearchGamesResponse(
    IEnumerable<GameDto> Items,
    long TotalItemCount,
    long PageCount,
    long PageNumer,
    bool IsLastPage
) : IPaginatedResponse<GameDto>;
