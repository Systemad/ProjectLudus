using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Video
{
    public long Id { get; set; }

    public long? Game { get; set; }

    public string Name { get; set; } = null!;

    public string? VideoId { get; set; }

    public string Checksum { get; set; } = null!;

    public virtual Game? GameNavigation { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
