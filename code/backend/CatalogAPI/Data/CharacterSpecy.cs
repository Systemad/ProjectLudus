using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class CharacterSpecy
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
}
