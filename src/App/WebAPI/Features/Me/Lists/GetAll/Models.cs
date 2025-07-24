using WebAPI.Features.Common.Lists;

namespace Me.Lists.GetAll;

public class GetMyListsResponse
{
    public GameListDto[] Lists { get; set; }
}
