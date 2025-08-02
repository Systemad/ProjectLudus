using FastEndpoints;
using WebAPI.Features.Common.Endpoints;

namespace Public.Games.GetGamesByParameters;

public class GameSearchRequest : IPaginationParameters
{
    [QueryParam] public int PageSize { get; set; } = 40;
    [QueryParam] public int PageNumber { get; set; } = 1;
    [QueryParam] public string? Name { get; set; }
    [QueryParam] public long[]? Genres { get; set; }
    [QueryParam] public long[]? Platforms { get; set; }
    [QueryParam] public long[]? GameModes { get; set; }
    [QueryParam] public long[]? Themes { get; set; }
    [QueryParam] public long[]? GameTypes { get; set; }
    [QueryParam] public long[]? PlayerPerspectives { get; set; }
}