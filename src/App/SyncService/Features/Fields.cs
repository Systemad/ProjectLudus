using SyncService.Features.Companies;

namespace SyncService.Features.Games;

public enum IgdbReference
{
    GAMES,
    PLATFORMS,
    GAMEENGINES,
    COMPANIES,
    THEMES,
    GENRES,
}

public static class IgdbFields
{
    public static Dictionary<IgdbReference, (string Endpoint, List<string> Fields)> Queries = new Dictionary<IgdbReference, (string Endpoint, List<string> Fields)>
    {
        {
            IgdbReference.PLATFORMS,
            (CompanyQuery.Endpoint, CompanyQuery.Fields)
            
        },
        { IgdbReference.GAMEENGINES, ("", [""])  },
        { IgdbReference.COMPANIES, ("", [""])  },
        { IgdbReference.THEMES, ("", [""])  },
        { IgdbReference.GENRES, ("", [""])  },
    };
}