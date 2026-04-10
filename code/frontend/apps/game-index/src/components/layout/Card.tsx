import { Box } from "ui";
import React from "react";

type CardVariant = "opaque" | "translucent" | "panel";

interface CardSurface extends React.ComponentProps<typeof Box> {
    variant?: CardVariant;
}

export function CardSurface({ children, variant = "opaque", ...props }: CardSurface) {
    const variantMap: Record<CardVariant, string> = {
        opaque: "opaqueCard",
        translucent: "translucentCard",
        panel: "translucentPanel",
    };

    return (
        <Box p={6} rounded="2xl" layerStyle={variantMap[variant]} {...props}>
            {children}
        </Box>
    );
}
