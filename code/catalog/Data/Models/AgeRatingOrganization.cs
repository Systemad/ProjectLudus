using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class AgeRatingOrganization
{
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<AgeRatingCategory> AgeRatingCategories { get; set; } = new List<AgeRatingCategory>();

    public virtual ICollection<AgeRatingContentDescriptionsV2> AgeRatingContentDescriptionsV2s { get; set; } = new List<AgeRatingContentDescriptionsV2>();

    public virtual ICollection<AgeRating> AgeRatings { get; set; } = new List<AgeRating>();
}
