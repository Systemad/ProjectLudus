using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Ludus.Hypes;

public class MeHypesGroup : Group
{
    public MeHypesGroup()
    {
        Configure(
            $"{ApiRoutes.MeRoutes.Hypes}",
            ep =>
            {
                ep.Description(x =>
                {
                    x.WithTags("me", "hypes");
                });
            }
        );
    }
}
