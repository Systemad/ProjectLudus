using CatalogAPI.Common;
using FastEndpoints;

namespace Features.Games;

public class GamesGroupEndpoint : Group
{
    public GamesGroupEndpoint()
    {
        Configure(
            $"{ApiRoutes.Games}",
            ep =>
            {
                ep.Description(x =>
                {
                    x.WithTags("Games");
                });
            }
        );
    }
}
