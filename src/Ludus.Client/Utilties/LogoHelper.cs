namespace Ludus.Client.Utilties;

public static class LogoHelper
{
    public static string GetLogoFromName(this string name)
    {
        var lowered = name.ToLowerInvariant();

        return lowered switch
        {
            _ when lowered.Contains("xbox") => ExternalLogo.XboxOriginal,
            _ when lowered.Contains("playstation") || lowered.Contains("playstation") =>
                ExternalLogo.Playstation,
            _ when lowered.Contains("switch") => ExternalLogo.NintendoSwitch,
            _ when lowered.Contains("windows") => ExternalLogo.PCWindows,
            _
                when lowered.Contains("apple")
                    || lowered.Contains("mac")
                    || lowered.Contains("ios") => ExternalLogo.AppleMAC,
            _ when lowered.Contains("amazon") => ExternalLogo.AmazonFireTV,
            _ when lowered.Contains("linux") => ExternalLogo.Linux,
            _ when lowered.Contains("android") => ExternalLogo.Android,
            _ when lowered.Contains("gamecube") => ExternalLogo.GameCube,
            _ when lowered.Contains("oculus") => ExternalLogo.Oculus,
            _ when lowered.Contains("meta") => ExternalLogo.MetaQuest,
            "wii u" => ExternalLogo.MetaQuest,
            "wii" => ExternalLogo.MetaQuest,
            _ => "",
        };
    }
}
