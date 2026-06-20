using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// company_statuses lookup table.
/// </summary>
public partial class CompanyStatus
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
