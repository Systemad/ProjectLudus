using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class AgeRating
{
    public long Id { get; set; }

    public string Checksum { get; set; } = null!;

    public long? Organization { get; set; }

    public long? RatingCategory { get; set; }

    public string? Synopsis { get; set; }

    public virtual AgeRatingOrganization? OrganizationNavigation { get; set; }

    public virtual AgeRatingCategory? RatingCategoryNavigation { get; set; }
}
