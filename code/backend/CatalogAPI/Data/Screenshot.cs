using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class Screenshot
{
    public long Id { get; set; }

    public bool? AlphaChannel { get; set; }

    public bool? Animated { get; set; }

    public long? Game { get; set; }

    public long? Height { get; set; }

    public string? ImageId { get; set; }

    public string? Url { get; set; }

    public long? Width { get; set; }

    public string? Checksum { get; set; }

    public virtual Game? GameNavigation { get; set; }
}
