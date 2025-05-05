using Ludus.Server.Features.GameEntries;
using Ludus.Server.Features.Games;
using Ludus.Shared.Features.Games;
using Marten;

namespace Ludus.Server.Features.Lists.Services;

public class UserListService
{
    private IUserStore UserStore { get; set; }
    private IDocumentStore GameDb { get; set; }

    public UserListService(IUserStore userStore, IDocumentStore gameDb)
    {
        UserStore = userStore;
        GameDb = gameDb;
    }

    public async Task<UserGameListDto> GetUserGameListDtoAsync(
        Guid userId,
        Guid listId,
        bool fetchPreview
    )
    {
        await using var session = UserStore.LightweightSession();
        await using var gameSession = GameDb.LightweightSession();
        var list = await session.LoadAsync<UserGameList>(listId);

        var gameEntryIds = fetchPreview ? list.GameEntryIds.Take(6).ToList() : list.GameEntryIds;

        var gameEntries = await session
            .Query<GameEntry>()
            .Where(x => x.UserId == userId)
            .Where(x => gameEntryIds.Contains(x.Id))
            .ToListAsync();

        var gameIds = gameEntries.Select(x => x.GameId).Distinct().ToList();

        var games = await gameSession
            .Query<Game>()
            .Where(g => gameIds.Contains(g.Id))
            .ToListAsync();
        var gameLookup = games.ToDictionary(g => g.Id, g => g);
        var listDto = gameEntries
            .Select(entry =>
            {
                if (gameLookup.TryGetValue(entry.GameId, out var game))
                    return entry.ToGameEntryPreviewDto(game.ToGameDto());
                return null;
            })
            .Where(dto => dto != null)
            .ToList();

        var preview = new UserGameListDto(list.Id, list.Name, list.Public, listDto);
        return preview;
    }

    public async Task<IEnumerable<UserGameListDto>> GetUserGameListsDtoAsync(
        Guid userId,
        bool fetchPreview
    )
    {
        await using var session = UserStore.LightweightSession();
        await using var gameSession = GameDb.LightweightSession();
        var lists = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var gameEntryIds = fetchPreview
            ? lists.SelectMany(x => x.GameEntryIds).Take(6).ToList()
            : lists.SelectMany(x => x.GameEntryIds).ToList();

        var gameEntries = await session
            .Query<GameEntry>()
            .Where(x => x.UserId == userId)
            .Where(x => gameEntryIds.Contains(x.Id))
            .ToListAsync();

        var gameIds = gameEntries.Select(x => x.GameId).Distinct().ToList();

        var games = await gameSession
            .Query<Game>()
            .Where(g => gameIds.Contains(g.Id))
            .ToListAsync();
        var entryLookup = gameEntries.ToDictionary(g => g.Id, g => g);
        var gameLookup = games.ToDictionary(g => g.Id, g => g);

        var listDtos = lists.Select(list =>
        {
            var previewEntries = fetchPreview
                ? list.GameEntryIds.Take(6).ToList()
                : list.GameEntryIds;
            var gamesDtoEntry = previewEntries
                .Select(id =>
                {
                    if (
                        entryLookup.TryGetValue(id, out var entry)
                        && gameLookup.TryGetValue(entry.GameId, out var game)
                    )
                    {
                        return entry.ToGameEntryPreviewDto(game.ToGameDto());
                    }

                    return null;
                })
                .Where(dto => dto != null)
                .ToList();
            return new UserGameListDto(list.Id, list.Name, list.Public, gamesDtoEntry);
        });

        return listDtos;
    }
}
