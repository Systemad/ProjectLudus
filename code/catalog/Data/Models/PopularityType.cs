using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// popularity_types lookup table.
/// </summary>
public partial class PopularityType
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }
}
