using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Character
{
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public long? MugShot { get; set; }

    public long? CharacterGender { get; set; }

    public long? CharacterSpecies { get; set; }

    public string? Description { get; set; }

    public virtual CharacterGender? CharacterGenderNavigation { get; set; }

    public virtual CharacterSpecy? CharacterSpeciesNavigation { get; set; }

    public virtual CharacterMugShot? MugShotNavigation { get; set; }
}
