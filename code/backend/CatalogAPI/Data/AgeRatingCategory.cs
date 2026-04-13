namespace CatalogAPI.Data;

public partial class AgeRatingCategory
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Rating { get; set; }

    public long? Organization { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<AgeRating> AgeRatings { get; set; } = new List<AgeRating>();

    public virtual AgeRatingOrganization? OrganizationNavigation { get; set; }
}
