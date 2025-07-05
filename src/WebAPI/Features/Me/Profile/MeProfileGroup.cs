using FastEndpoints;
using WebAPI.Features.Common;

namespace Me.Profile;

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
