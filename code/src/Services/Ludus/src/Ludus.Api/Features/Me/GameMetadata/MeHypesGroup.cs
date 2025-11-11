using FastEndpoints;
using Ludus.Api.Features.Common;

namespace Me.GameMetadata;

public class GameMetadataGroup : Group
{
    public GameMetadataGroup()
    {
        Configure(
            $"{ApiRoutes.MeRoutes.GameMetadata}",
            ep =>
            {
                ep.Description(x =>
                {
                    x.WithTags("me", "game", "metadata");
                });
            }
        );
    }
}
