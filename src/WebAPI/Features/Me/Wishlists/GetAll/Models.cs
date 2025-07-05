using WebAPI.Features.Common.Games.Models;

namespace Me.Wishlists.GetAll;

public class GetWishlistedGamesResponse
{
    public List<GameDto> Games { get; set; }
}
