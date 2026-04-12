using CatalogAPI.Data;
using CatalogAPI.Dtos;
using Riok.Mapperly.Abstractions;

namespace CatalogAPI.Features.Games.GetMediaById;

[Mapper]
public static partial class GameMediaMapper
{
    public static partial IQueryable<GameMedia> ProjectTo(this IQueryable<Game> q);

    private static List<string> MapScreenshots(ICollection<Screenshot> screenshots) 
        => screenshots.Where(s => s.ImageId != null).Select(s => s.ImageId!).ToList();

   // private static partial GameMediaVideo MapVideo(Video video);

    // Rename this partial method
    //private static partial GameMedia MapGame(Game game);
}