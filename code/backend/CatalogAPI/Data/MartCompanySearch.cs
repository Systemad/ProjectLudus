using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class MartCompanySearch
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Slug { get; set; }

    public string? Url { get; set; }

    public long? UpdatedAt { get; set; }

    public long? StartDate { get; set; }

    public int? StartYear { get; set; }

    public string? ParentCompany { get; set; }

    public string? ChangedCompany { get; set; }

    public string? Status { get; set; }

    public string? LogoUrl { get; set; }

    public int? GamesDevelopedCount { get; set; }

    public int? GamesPublishedCount { get; set; }
}
