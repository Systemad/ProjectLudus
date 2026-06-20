using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Event
{
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public long? EventLogo { get; set; }

    public long? StartTimeEpoch { get; set; }

    public DateTime? StartTimeUtc { get; set; }

    public string? TimeZone { get; set; }

    public string? LiveStreamUrl { get; set; }

    public string Checksum { get; set; } = null!;

    public long? EndTimeEpoch { get; set; }

    public DateTime? EndTimeUtc { get; set; }

    public string? Description { get; set; }

    public virtual EventLogo? EventLogoNavigation { get; set; }

    public virtual ICollection<EventNetwork> EventNetworks { get; set; } = new List<EventNetwork>();

    public virtual ICollection<EventNetwork> EventNetworksNavigation { get; set; } = new List<EventNetwork>();

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
