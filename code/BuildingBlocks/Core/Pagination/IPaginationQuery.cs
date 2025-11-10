using System.Text.Json.Serialization;

namespace BuildingBlocks.Core.Pagination;

public interface IPaginationQuery
{
    int PageSize { get; }
    int PageNumber { get; }
}

