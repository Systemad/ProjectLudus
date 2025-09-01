using FastEndpoints;
using WebAPI.Features.Common.Endpoints;

namespace Public.Games.GetGamesByParameters;

public class GameSearchRequest : PaginationRequest
{
    [QueryParam, BindFrom("name")] public string? Query { get; set; }
    [QueryParam, BindFrom("genres")] public long[]? Genres { get; set; }
    [QueryParam, BindFrom("platforms")] public long[]? Platforms { get; set; }
    [QueryParam, BindFrom("gamemodes")] public long[]? GameModes { get; set; }
    [QueryParam, BindFrom("themes")] public long[]? Themes { get; set; }
    [QueryParam, BindFrom("gametypes")] public long[]? GameTypes { get; set; }
    [QueryParam, BindFrom("pps")] public long[]? PlayerPerspectives { get; set; }
}
