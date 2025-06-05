using FastEndpoints;
using Ludus.Server.Features.Common;

namespace Ludus.Server.Features.Me.Collections;

public class MeCollectionsGroup : Group
{
    public MeCollectionsGroup()
    {
        Configure(
            $"{ApiRoutes.MeRoutes.Collections}",
            ep =>
            {
                ep.Description(x =>
                {
                    x.WithTags("me", "collections");
                });
            }
        );
    }
}
