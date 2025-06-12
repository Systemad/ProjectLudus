using System.Security.Claims;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Games.Models;
using Ludus.Server.Features.DataAccess;
using Ludus.Shared.Features.Games;
using Marten;

namespace Ludus.Server.Features.Common.Games.Services;

public class GameService : IGameService
{
    private LudusContext _dbContext { get; set; }
    private IGameStore _store { get; set; }

    public GameService(IGameStore store, LudusContext dbContext)
    {
        _store = store;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<GameDto>> CreateGameDtoAsync(
        ClaimsPrincipal user,
        IEnumerable<long> gameIds
    )
    {
        HashSet<long> wishlistedSet = [];
        HashSet<long> hypedSet = [];
        if (user.Identity.IsAuthenticated)
        {
            var userId = user.GetUserId();

            var wishlistedGameIds = await _dbContext
                .Wishlists.Where(w => w.UserId == userId)
                .Select(w => w.GameId)
                .ToListAsync();

            var hypedGamesIds = await _dbContext
                .Hypes.Where(f => f.UserId == userId)
                .Select(f => f.GameId)
                .ToListAsync();
            wishlistedSet = wishlistedGameIds.ToHashSet();
            hypedSet = hypedGamesIds.ToHashSet();
        }

        await using var gameSession = _store.QuerySession();
        var gameDto = await gameSession
            .Query<Game>()
            .Where(g => gameIds.Contains(g.Id))
            .Select(g => new GameDto()
            {
                Id = g.Id,
                Name = g.Name,
                ArtworkImageId = g.Artworks.FirstOrDefault().ImageId,
                CoverImageId = g.Cover.ImageId,
                FirstReleaseDate = g.FirstReleaseDate,
                Publisher = g.InvolvedCompanies.FirstOrDefault(ic => ic.Publisher).Company.Name,
                Platforms = g.Platforms.Select(p => p.Name).ToList(),
                ReleaseDates = g
                    .ReleaseDates.Select(rd => DateTimeOffset.FromUnixTimeSeconds(rd.Date).DateTime)
                    .ToList(),
                GameType = g.GameType.Type,
                IsWishlisted = wishlistedSet.Contains(g.Id),
                IsHyped = hypedSet.Contains(g.Id),
            })
            .ToListAsync();

        return gameDto;
    }
}
