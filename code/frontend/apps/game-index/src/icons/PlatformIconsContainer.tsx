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

export function PlatformIcons({ websites }: PlatformIconsContainerProps) {
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
