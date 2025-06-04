namespace Ludus.Server.Features.Collection.Common;

public static class UserGameMapper
{
    public static UserGameStateDto AsDto(this UserGameState collection)
    {
        return new UserGameStateDto(
            collection.Id,
            collection.GameId,
            collection.Status,
            collection.StartDate,
            collection.EndDate,
            collection.UpdatedAt,
            collection.Rating,
            collection.Notes,
            collection.IsWishlisted,
            collection.IsFavorited
        );
    }
}
