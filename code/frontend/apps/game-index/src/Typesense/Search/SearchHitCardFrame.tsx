import type { ReactNode } from "react";
import { Box } from "ui";

type SearchHitCardFrameProps = {
    children: ReactNode;
};

export function SearchHitCardFrame({ children }: SearchHitCardFrameProps) {
    return (
        <Box
            as="article"
            h="full"
            display="grid"
            gridTemplateRows="auto 1fr"
            gap="sm"
            p="sm"
            rounded="xl"
            layerStyle="translucentPanel"
        >
            {children}
        </Box>
    );
}
