using FastEndpoints;
using WebAPI.Features.Common;

namespace Public.Games;

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
