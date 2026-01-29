using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Ludus.Users.Features;

public class MeProfileGroup : Group
{
    public MeProfileGroup()
    {
        Configure(
            $"{ApiRoutes.MeRoutes.Profile}",
            ep =>
            {
                ep.Description(x =>
                {
                    x.WithTags("me", "profile");
                });
            }
        );
    }
}
