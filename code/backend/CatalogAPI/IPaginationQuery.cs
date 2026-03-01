namespace CatalogAPI;

public interface IPaginationQuery
{
    int PageSize { get; }
    int PageNumber { get; }
}
