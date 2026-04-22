using System;
using System.Collections.Generic;
using NodaTime;

namespace CatalogAPI.Data;

public partial class Event
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public long? EventLogo { get; set; }

    public long? StartTimeEpoch { get; set; }

    public Instant? StartTimeUtc { get; set; }

    public string? TimeZone { get; set; }

    public string? LiveStreamUrl { get; set; }

    public string? Checksum { get; set; }

    public long? EndTimeEpoch { get; set; }

    public Instant? EndTimeUtc { get; set; }

    public string? Description { get; set; }

    public virtual EventLogo? EventLogoNavigation { get; set; }

    public virtual ICollection<EventNetwork1> EventNetwork1s { get; set; } = new List<EventNetwork1>();

    public virtual ICollection<EventNetwork1> EventNetworks { get; set; } = new List<EventNetwork1>();

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
