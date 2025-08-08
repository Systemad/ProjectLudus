using FastEndpoints;
using WebAPI.Features.Common.Endpoints;

namespace Public.Games.GetGamesByParameters;

public class GameSearchRequest : PaginationRequest
{
    [QueryParam, BindFrom("name")] public string? Name { get; set; }
    [QueryParam, BindFrom("genres")] public List<long>? Genres { get; set; }
    [QueryParam, BindFrom("platforms")] public List<long>? Platforms { get; set; }
    [QueryParam, BindFrom("gamemodes")] public List<long>? GameModes { get; set; }
    [QueryParam, BindFrom("themes")]public List<long>? Themes { get; set; }
    [QueryParam, BindFrom("gametypes")] public List<long>? GameTypes { get; set; }
    [QueryParam, BindFrom("pps")] public List<long>? PlayerPerspectives { get; set; }
}

/*
    [QueryParam, BindFrom("pageSize")]
    public int? _pageSize { get; set; }

    [QueryParam, BindFrom("pageNumber")]
    public int? _pageNumber { get; set; }
*/