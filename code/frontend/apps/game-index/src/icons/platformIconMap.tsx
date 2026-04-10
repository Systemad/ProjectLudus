import AppleIcon from "./Consoles/AppleIcon";
import LinuxIcons from "./Consoles/LinuxIcons";
import PlaystationIcon from "./Consoles/PlaystationIcon";
import WindowsIcon from "./Consoles/WindowsIcon";
import XboxIcon from "./Consoles/XboxIcon";
import CryEngineIcon from "./GameEngines/CryEngineIcon";
import GodotEngineIcon from "./GameEngines/GodotEngineIcon";
import SourceEngineIcon from "./GameEngines/SourceEngineIcon";
import UnityEngineIcon from "./GameEngines/UnityEngineIcon";
import UnrealEngineIcon from "./GameEngines/UnrealEngineIcon";
import BattleNetIcon from "./Launchers/BattleNetIcon";
import EAIcon from "./Launchers/EAIcon";
import EpicGamesIcon from "./Launchers/EpicGamesIcon";
import GOGIcon from "./Launchers/GOGIcon";
import ItchIoIcon from "./Launchers/ItchIoIcon";
import SteamIcon from "./Launchers/SteamIcon";
import SteamIcon2 from "./Launchers/SteamIcon2";
import UbisoftIcon from "./Launchers/UbisoftIcon";

export const platformIconMap: Record<string, React.ReactNode> = {
    apple: <AppleIcon />,
    Mac: <AppleIcon />,
    linux: <LinuxIcons />,
    windows: <WindowsIcon />,
    "pc (microsoft windows)": <WindowsIcon />,
    xbox: <XboxIcon />,
    playstation: <PlaystationIcon />,
    steam: <SteamIcon />,
    steam2: <SteamIcon2 />,
    epic: <EpicGamesIcon />,
    battlenet: <BattleNetIcon />,
    "battle-net": <BattleNetIcon />,
    ea: <EAIcon />,
    gog: <GOGIcon />,
    itch: <ItchIoIcon />,
    itchio: <ItchIoIcon />,
    ubisoft: <UbisoftIcon />,
    cryengine: <CryEngineIcon />,
    godot: <GodotEngineIcon />,
    source: <SourceEngineIcon />,
    unity: <UnityEngineIcon />,
    unreal: <UnrealEngineIcon />,
};
