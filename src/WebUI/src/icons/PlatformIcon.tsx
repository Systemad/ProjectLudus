import { Tooltip } from "@yamada-ui/react";
import { platformIconMap } from "./platformIconMap";

interface PlatformIconProps {
    type: string;
}

export function PlatformIcon({ type }: PlatformIconProps) {
    const icon = platformIconMap[type];
    if (!icon) return null;

    return (
        <Tooltip label={type} placement="top">
            {icon}
        </Tooltip>
    );
}
