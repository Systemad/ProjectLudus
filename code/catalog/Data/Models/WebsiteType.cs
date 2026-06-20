using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// website_types lookup table.
/// </summary>
public partial class WebsiteType
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Type { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<CompanyWebsite> CompanyWebsites { get; set; } = new List<CompanyWebsite>();

    public virtual ICollection<Website> Websites { get; set; } = new List<Website>();
}
