using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class CharacterSpecy
{
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
}
