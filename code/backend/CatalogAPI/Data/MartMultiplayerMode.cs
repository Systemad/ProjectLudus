using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class MartMultiplayerMode
{
    public long? Id { get; set; }

    public bool? Campaigncoop { get; set; }

    public bool? Dropin { get; set; }

    public long? Game { get; set; }

    public bool? Lancoop { get; set; }

    public bool? Offlinecoop { get; set; }

    public bool? Onlinecoop { get; set; }

    public long? Platform { get; set; }

    public bool? Splitscreen { get; set; }

    public string? Checksum { get; set; }

    public long? Offlinemax { get; set; }

    public long? Onlinemax { get; set; }

    public long? Onlinecoopmax { get; set; }

    public long? Offlinecoopmax { get; set; }
}
