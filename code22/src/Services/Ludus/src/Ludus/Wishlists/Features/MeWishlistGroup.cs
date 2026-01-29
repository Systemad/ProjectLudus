using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Ludus.Wishlists.Features;

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
