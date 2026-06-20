using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// language_support_types lookup table.
/// </summary>
public partial class LanguageSupportType
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<LanguageSupport> LanguageSupports { get; set; } = new List<LanguageSupport>();
}
