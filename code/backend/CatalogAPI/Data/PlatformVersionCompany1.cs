using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class PlatformVersionCompany1
{
    public long Id { get; set; }

    public long? Company { get; set; }

    public bool? Developer { get; set; }

    public bool? Manufacturer { get; set; }

    public string? Checksum { get; set; }

    public string? Comment { get; set; }

    public virtual Company? CompanyNavigation { get; set; }

    public virtual ICollection<PlatformVersion> PlatformVersions { get; set; } = new List<PlatformVersion>();
}
