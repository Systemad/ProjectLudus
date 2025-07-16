import PlaystationIcon from "~/icons/Consoles/PlaystationIcon";
import XboxIcon from "~/icons/Consoles/XboxIcon";
import EpicGamesIcon from "~/icons/Launchers/EpicGamesIcon";
import SteamIcon from "~/icons/Launchers/SteamIcon";

export const platformIconMap: Record<string, React.ReactNode> = {
    Steam: <SteamIcon />,
    Xbox: <XboxIcon />,
    Playstation: <PlaystationIcon />,
    Epic: <EpicGamesIcon />,
    // Add other platforms as needed
};
