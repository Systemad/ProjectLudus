using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class PlatformWebsite
{
    public long Id { get; set; }

    public bool? Trusted { get; set; }

    public string Url { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public long? Type { get; set; }
}
