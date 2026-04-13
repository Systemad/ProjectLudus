namespace CatalogAPI.Data;

public partial class AgeRatingOrganization
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<AgeRatingCategory> AgeRatingCategories { get; set; } = new List<AgeRatingCategory>();

    public virtual ICollection<AgeRatingContentDescriptionsV2> AgeRatingContentDescriptionsV2s { get; set; } = new List<AgeRatingContentDescriptionsV2>();

    public virtual ICollection<AgeRating> AgeRatings { get; set; } = new List<AgeRating>();
}
