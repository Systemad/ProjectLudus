using Shared.Features.Games;

namespace WebAPI.Features.Common.Games.Models;

public class GameDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string ArtworkImageId { get; set; }
    public string CoverImageId { get; set; }
    public long FirstReleaseDate { get; set; }
    //public List<InvolvedCompany>? InvolvedCompanies { get; set; }
    public List<Platform> Platforms { get; set; }
    public List<DateTime> ReleaseDates { get; set; }
    public GameType? GameType { get; set; }
    public bool IsHyped { get; set; }
    public bool IsWishlisted { get; set; }
}
