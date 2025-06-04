using FastEndpoints;
using Ludus.Server.Features.Common;

namespace Ludus.Server.Features.Collection;

public class CollectionGroupEndpoint : Group
{
    public CollectionGroupEndpoint()
    {
        Configure(
            $"{ApiRoutes.Track}",
            ep =>
            {
                ep.Description(x =>
                {
                    x.WithTags("Collection");
                });
            }
        );
    }
}
