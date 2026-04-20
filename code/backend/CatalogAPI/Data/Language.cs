using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class Language
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? NativeName { get; set; }

    public string? Locale { get; set; }

    public string? Checksum { get; set; }
}
