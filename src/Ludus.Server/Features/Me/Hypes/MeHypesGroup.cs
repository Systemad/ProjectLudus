using FastEndpoints;
using Ludus.Server.Features.Common;

namespace Me.Hypes;

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
