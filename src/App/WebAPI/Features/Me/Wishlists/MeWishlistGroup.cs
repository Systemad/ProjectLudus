using FastEndpoints;
using WebAPI.Features.Common;

namespace Me.Wishlists;

public class MeWishlistGroup : Group
{
    public MeWishlistGroup()
    {
        Configure(
            $"{ApiRoutes.MeRoutes.Wishlist}",
            ep =>
            {
                ep.Description(x =>
                {
                    x.WithTags("me", "wishlist");
                });
            }
        );
    }
}
