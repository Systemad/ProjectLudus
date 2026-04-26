import { GlobeIcon, Icon, Tooltip } from "ui";
import { iconMap } from "./platformIconMap";

interface PlatformIconProps {
    type?: string | null;
}

export function PlatformIcon({ type }: PlatformIconProps) {
    const IconComponent = iconMap[type?.toLowerCase() ?? ""] ?? GlobeIcon;

    return (
        <Tooltip content={type ?? "Link"} placement="start">
            <Icon as={IconComponent} boxSize="1rem" color="currentColor" />
        </Tooltip>
    );
}
