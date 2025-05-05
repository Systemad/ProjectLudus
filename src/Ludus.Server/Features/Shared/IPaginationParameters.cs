using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Shared;

public interface IPaginationParameters
{
    [FromQuery(Name = "ps")]
    public int PageSize { get; }

    [FromQuery(Name = "pn")]
    public int PageNumber { get; }
}
