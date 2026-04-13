namespace CatalogAPI.Data;

public partial class AgeRatingContentDescriptionsV2
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Description { get; set; }

    public long? Organization { get; set; }

    public string? Checksum { get; set; }

    public long? DescriptionType { get; set; }

    public virtual AgeRatingContentDescriptionType? DescriptionTypeNavigation { get; set; }

    public virtual AgeRatingOrganization? OrganizationNavigation { get; set; }
}
