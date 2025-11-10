import PlaystationIcon from "~/icons/Consoles/PlaystationIcon";
import XboxIcon from "~/icons/Consoles/XboxIcon";
import EpicGamesIcon from "~/icons/Launchers/EpicGamesIcon";
import SteamIcon from "~/icons/Launchers/SteamIcon";

export const platformIconMap: Record<string, React.ReactNode> = {
    steam: <SteamIcon />,
    xbox: <XboxIcon />,
    playstation: <PlaystationIcon />,
    epic: <EpicGamesIcon />,
    // Add other platforms as needed
};
