using FastEndpoints;
using Ludus.Server.Features.Common;

namespace Ludus.Server.Features.Games;

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
