using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class BridgeCompanyTypeHistory
{
    public long CompanyId { get; set; }

    public long CompanyTypeId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual CompanyType CompanyType { get; set; } = null!;
}
