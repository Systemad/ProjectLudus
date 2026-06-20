import { GlobeIcon, Icon, Tooltip } from "ui";
import { iconMap } from "./platformIconMap";

interface PlatformIconProps {
    type: string;
    tooltip: string;
    boxSize?: string | number;
}

export function PlatformIcon({ type, tooltip, boxSize = "1rem" }: PlatformIconProps) {
    const IconComponent = iconMap[type] ?? GlobeIcon;

    return (
        <Tooltip content={tooltip} placement="start">
            <Icon as={IconComponent} boxSize={boxSize} color="currentColor" />
        </Tooltip>
    );
}
