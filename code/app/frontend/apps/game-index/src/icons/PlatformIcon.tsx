import { Tooltip } from "@astryxdesign/core/Tooltip";
import { LuGlobe } from "react-icons/lu";
import { iconMap } from "./platformIconMap";

interface PlatformIconProps {
    type: string;
    tooltip: string;
    boxSize?: string | number;
}

export function PlatformIcon({ type, tooltip, boxSize = "1rem" }: PlatformIconProps) {
    const IconComponent = iconMap[type] ?? LuGlobe;

    return (
        <Tooltip content={tooltip} placement="start">
            <IconComponent style={{ width: boxSize, height: boxSize, color: "currentColor" }} />
        </Tooltip>
    );
}
