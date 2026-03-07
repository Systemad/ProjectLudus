import PlaystationIcon from "./Consoles/PlaystationIcon";
import XboxIcon from "./Consoles/XboxIcon";
import EpicGamesIcon from "./Launchers/EpicGamesIcon";
import SteamIcon from "./Launchers/SteamIcon";

export const platformIconMap: Record<string, React.ReactNode> = {
    steam: <SteamIcon />,
    xbox: <XboxIcon />,
    playstation: <PlaystationIcon />,
    epic: <EpicGamesIcon />,
    // Add other platforms as needed
};
