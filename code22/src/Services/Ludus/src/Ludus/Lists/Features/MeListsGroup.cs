using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Ludus.Lists.Features;

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
