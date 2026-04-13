namespace CatalogAPI.Data;

/// <summary>
/// Relationship rows that connect games with involved companies and their roles.
/// </summary>
public partial class InvolvedCompany
{
    /// <summary>
    /// The uniqueness key for the involved-company relationship row.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The company involved with the game.
    /// </summary>
    public long? Company { get; set; }

    /// <summary>
    /// The source record creation timestamp.
    /// </summary>
    public long? CreatedAt { get; set; }

    /// <summary>
    /// True when the company is a developer for this game.
    /// </summary>
    public bool? Developer { get; set; }

    /// <summary>
    /// The game referenced by this involved-company relationship.
    /// </summary>
    public long? Game { get; set; }

    public bool? Porting { get; set; }

    public bool? Publisher { get; set; }

    public bool? Supporting { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Checksum { get; set; }

    public virtual Company? CompanyNavigation { get; set; }

    public virtual Game? GameNavigation { get; set; }
}
