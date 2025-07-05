using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Features.Common.Endpoints;

public interface IPaginationParameters
{
    [FromQuery(Name = "ps")]
    public int PageSize { get; }

    [FromQuery(Name = "pn")]
    public int PageNumber { get; }
}
