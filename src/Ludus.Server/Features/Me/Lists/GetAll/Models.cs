using Ludus.Server.Features.Common.Lists;

namespace Ludus.Server.Features.Me.Lists.GetAll;

public class GetMyListsResponse
{
    public List<UserGameListDto> Lists { get; set; }
}
