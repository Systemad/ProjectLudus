namespace WebAPI.Features.Common.Endpoints;

public interface IPaginationParameters
{
    int PageSize { get; }
    int PageNumber { get; }
}
