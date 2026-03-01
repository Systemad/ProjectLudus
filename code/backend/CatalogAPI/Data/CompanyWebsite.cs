using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class CompanyWebsite
{
    public long Id { get; set; }

    public bool? Trusted { get; set; }

    public string? Url { get; set; }

    public string? Checksum { get; set; }

    public long? Type { get; set; }

    public virtual WebsiteType? TypeNavigation { get; set; }
}
