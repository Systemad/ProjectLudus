namespace Ludus.Server.Features.Common.Games.Models;

public record UserGameInfoDto(long GameId, bool IsWishlisted, bool IsFavorited, bool IsHyped);
