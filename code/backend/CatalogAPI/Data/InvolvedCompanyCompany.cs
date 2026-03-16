using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class InvolvedCompanyCompany
{
    public long InvolvedCompanyId { get; set; }

    public long? CompanyId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual InvolvedCompany InvolvedCompany { get; set; } = null!;
}
