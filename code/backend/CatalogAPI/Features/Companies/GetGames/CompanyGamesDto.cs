namespace CatalogAPI.Features.Companies.GetGames;

public record CompanyGamesDto(List<GameDto> PublishedGames, List<GameDto> DevelopedGames);
