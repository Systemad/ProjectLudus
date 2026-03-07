import { Tooltip } from "@packages/ui";
import { platformIconMap } from "./platformIconMap";

interface PlatformIconProps {
    type: string;
}

export function PlatformIcon({ type }: PlatformIconProps) {
    const icon = platformIconMap[type.toLocaleLowerCase()];
    if (!icon) return null;

    return (
        <Tooltip content={type} placement="start">
            {icon}
        </Tooltip>
    );
}
