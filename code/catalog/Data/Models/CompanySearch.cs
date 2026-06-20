using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Search-ready companies dataset for Typesense indexing
/// </summary>
public partial class CompanySearch
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Slug { get; set; } = null!;

    public string Url { get; set; } = null!;

    public long UpdatedAt { get; set; }

    public long? StartDate { get; set; }

    public int? StartYear { get; set; }

    public string? ParentCompany { get; set; }

    public string? ChangedCompany { get; set; }

    public string? Status { get; set; }

    public string? LogoUrl { get; set; }

    public int? GamesDevelopedCount { get; set; }

    public int? GamesPublishedCount { get; set; }
}
