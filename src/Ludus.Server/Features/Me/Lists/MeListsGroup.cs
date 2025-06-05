using FastEndpoints;
using Ludus.Server.Features.Common;

namespace Ludus.Server.Features.Me.Lists;

public class MeListsGroup : Group
{
    public MeListsGroup()
    {
        Configure(
            $"{ApiRoutes.MeRoutes.Lists}",
            ep =>
            {
                ep.Description(x =>
                {
                    x.WithTags("me", "lists");
                });
            }
        );
    }
}
