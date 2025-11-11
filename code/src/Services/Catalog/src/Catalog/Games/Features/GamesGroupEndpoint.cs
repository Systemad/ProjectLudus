using BuildingBlocks.Web;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Catalog.Games.Features;

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
