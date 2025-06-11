using Ludus.Server.Features.Common.Lists;

namespace Me.Lists.GetAll;

public class GetMyListsResponse
{
    public List<UserGameListDto> Lists { get; set; }
}
