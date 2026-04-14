using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class WebsiteType
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Type { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<CompanyWebsite> CompanyWebsites { get; set; } = new List<CompanyWebsite>();

    public virtual ICollection<Website> Websites { get; set; } = new List<Website>();
}
