using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared;

public class MUser
{
    public int Id { get; set; }
    [StringLength(32, MinimumLength = 3)] public string Name { get; set; }
    public string Role { get; set; }
    public string SteamId { get; set; }
    public int? AvatarImageId { get; set; }
    public string Color { get; set; }
    public string TermsAndConditions { get; set; }
    [StringLength(4000)] public string Biography { get; set; }
    public bool IsVerifiedSeller { get; set; }
    public DateTime CreateDate { get; set; }


    private const string SteamProfilesPath = "https://steamcommunity.com/profiles/";
    private const string BlueColorHex = "#0066ff";

    [JsonIgnore]
    public string SteamProfileUrl => new StringBuilder()
        .Append(SteamProfilesPath)
        .Append(SteamId)
        .ToString();

    [JsonIgnore] public string BackgroundColor => Color ?? BlueColorHex;


    public static MUser FromUser(MUser user)
    {
        return new MUser()
        {
            Id = user.Id,
            SteamId = user.SteamId,
            Name = user.Name,
            AvatarImageId = user.AvatarImageId,
            Color = user.Color,
            Role = user.Role,
            TermsAndConditions = user.TermsAndConditions,
            Biography = user.Biography,
            IsVerifiedSeller = user.IsVerifiedSeller,
            CreateDate = user.CreateDate
        };
    }
}