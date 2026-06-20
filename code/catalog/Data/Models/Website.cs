using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Website
{
    public long Id { get; set; }

    public long Game { get; set; }

    public bool Trusted { get; set; }

    public string Url { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public long? Type { get; set; }

    public virtual Game GameNavigation { get; set; } = null!;

    public virtual WebsiteType? TypeNavigation { get; set; }
}
