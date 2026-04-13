namespace CatalogAPI.Features.Games.GetMedia;

[Mapper]
public static partial class GameMediaMapper
{
#pragma warning disable RMG020
    public static partial IQueryable<GameMediaDto> ProjectTo(this IQueryable<Game> q);
#pragma warning restore RMG020
    private static List<string> MapScreenshots(ICollection<Screenshot> screenshots) =>
        screenshots.Where(s => s.ImageId != null).Select(s => s.ImageId!).ToList();

    // private static partial GameMediaVideo MapVideo(Video video);

    // Rename this partial method
    //private static partial GameMedia MapGame(Game game);
}
