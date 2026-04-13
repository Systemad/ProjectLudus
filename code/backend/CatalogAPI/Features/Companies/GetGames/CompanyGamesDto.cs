namespace CatalogAPI.Features.Companies.GetGames;

public record CompanyGamesDto(
    List<GamesSearchDto> PublishedGames,
    List<GamesSearchDto> DevelopedGames
);
