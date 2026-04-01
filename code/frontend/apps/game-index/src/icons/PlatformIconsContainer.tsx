import { HStack } from "ui";
import { platformIconMap } from "./platformIconMap";
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
// TODO: USE LATER

export function PlatformIcons({ websites }: PlatformIconsContainerProps) {
    // Filter and deduplicate platform types
    const uniqueTypes = Array.from(
        new Set(
            websites
                .filter((site) => site.type && platformIconMap[site.type.type])
                .map((site) => site.type!.type),
        ),
    );

    return (
        <HStack gap="xs">
            {uniqueTypes.map((type) => (
                <PlatformIcon key={type} type={type} />
            ))}
        </HStack>
    );
}
