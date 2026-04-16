using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class Event
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public long? EventLogo { get; set; }

    public long? StartTime { get; set; }

    public string? TimeZone { get; set; }

    public string? LiveStreamUrl { get; set; }

    public string Games { get; set; } = null!;

    public string? Checksum { get; set; }

    public long? EndTime { get; set; }

    public string? Description { get; set; }

    public string Videos { get; set; } = null!;

    public string EventNetworks { get; set; } = null!;

    public virtual EventLogo? EventLogoNavigation { get; set; }

    public virtual ICollection<EventNetwork> EventNetworksNavigation { get; set; } = new List<EventNetwork>();
}
