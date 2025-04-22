namespace Ludus.Client.Utilties;

public static class LogoHelper
{
    public static string GetLogoFromName(this string name)
    {
        var lowered = name.ToLowerInvariant();

        return lowered switch
        {
            _ when lowered.Contains("xbox") => Logo.XboxOriginal,
            _ when lowered.Contains("playstation") || lowered.Contains("playstation") =>
                Logo.Playstation,
            _ when lowered.Contains("switch") => Logo.NintendoSwitch,
            _ when lowered.Contains("windows") => Logo.PCWindows,
            _
                when lowered.Contains("apple")
                    || lowered.Contains("mac")
                    || lowered.Contains("ios") => Logo.AppleMAC,
            _ when lowered.Contains("amazon") => Logo.AmazonFireTV,
            _ when lowered.Contains("linux") => Logo.Linux,
            _ when lowered.Contains("android") => Logo.Android,
            _ when lowered.Contains("gamecube") => Logo.GameCube,
            _ when lowered.Contains("oculus") => Logo.Oculus,
            _ when lowered.Contains("meta") => Logo.MetaQuest,
            "wii u" => Logo.MetaQuest,
            "wii" => Logo.MetaQuest,
            _ => "",
        };
    }
}
