using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class PlatformVersion
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? PlatformLogo { get; set; }

    public string Slug { get; set; } = null!;

    public string? Summary { get; set; }

    public string Url { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public string? Cpu { get; set; }

    public string? Media { get; set; }

    public string? Memory { get; set; }

    public string? Output { get; set; }

    public string? Resolutions { get; set; }

    public string? Sound { get; set; }

    public string? Connectivity { get; set; }

    public string? Storage { get; set; }

    public string? Graphics { get; set; }

    public string? Os { get; set; }

    public long? MainManufacturer { get; set; }

    public virtual PlatformVersionCompany? MainManufacturerNavigation { get; set; }

    public virtual PlatformLogo? PlatformLogoNavigation { get; set; }
}
