using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class ReleaseDate
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? Date { get; set; }

    public long? Game { get; set; }

    public string? Human { get; set; }

    public long? M { get; set; }

    public long? Platform { get; set; }

    public long? UpdatedAt { get; set; }

    public long? Y { get; set; }

    public string? Checksum { get; set; }

    public long? Status { get; set; }

    public long? DateFormat { get; set; }

    public long? ReleaseRegion { get; set; }

    public virtual DateFormat? DateFormatNavigation { get; set; }

    public virtual Game? GameNavigation { get; set; }

    public virtual Platform? PlatformNavigation { get; set; }

    public virtual ReleaseDateRegion? ReleaseRegionNavigation { get; set; }

    public virtual ReleaseDateStatus? StatusNavigation { get; set; }
}
