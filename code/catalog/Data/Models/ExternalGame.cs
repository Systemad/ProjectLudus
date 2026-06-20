using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class ExternalGame
{
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long? Game { get; set; }

    public string Name { get; set; } = null!;

    public string? Uid { get; set; }

    public long UpdatedAt { get; set; }

    public string? Url { get; set; }

    public string Checksum { get; set; } = null!;

    public long? Year { get; set; }

    public long? Platform { get; set; }

    public long? ExternalGameSource { get; set; }

    public long? GameReleaseFormat { get; set; }

    public virtual ExternalGameSource? ExternalGameSourceNavigation { get; set; }

    public virtual Game? GameNavigation { get; set; }

    public virtual GameReleaseFormat? GameReleaseFormatNavigation { get; set; }

    public virtual Platform? PlatformNavigation { get; set; }
}
