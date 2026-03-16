using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class InvolvedCompany
{
    public long Id { get; set; }

    public long? Company { get; set; }

    public long? CreatedAt { get; set; }

    public bool? Developer { get; set; }

    public long? Game { get; set; }

    public bool? Porting { get; set; }

    public bool? Publisher { get; set; }

    public bool? Supporting { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Checksum { get; set; }

    public virtual InvolvedCompanyCompany? InvolvedCompanyCompany { get; set; }
}
