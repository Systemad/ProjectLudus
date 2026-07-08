import { HStack } from "@astryxdesign/core/HStack";
import { iconMap } from "./platformIconMap";
import { PlatformIcon } from "./PlatformIcon";

interface Website {
    id: number;
    url: string;
    type?: {
        id: number;
        type: string;
    };
}

interface PlatformIconsContainerProps {
    websites: Website[];
}

export function PlatformIcons({ websites }: PlatformIconsContainerProps) {
    const uniqueTypes = Array.from(
        new Set(
            websites
                .filter((site) => site.type && iconMap[site.type.type.toLowerCase()])
                .map((site) => site.type!.type.toLowerCase()),
        ),
    );

    return (
        <HStack gap={2}>
            {uniqueTypes.map((type) => (
                <PlatformIcon key={type} type={type} tooltip={type} />
            ))}
        </HStack>
    );
}
