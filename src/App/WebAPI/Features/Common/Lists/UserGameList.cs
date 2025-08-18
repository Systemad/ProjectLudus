using WebAPI.Features.Common.Endpoints;
using WebAPI.Features.Common.Games.Models;

namespace WebAPI.Features.Common.Lists;

public class GameListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Public { get; set; }
    public int TotalItems { get; set; }
    public PaginatedResponse<Games.Models.GamePreviewDto> Page { get; set; }
}
