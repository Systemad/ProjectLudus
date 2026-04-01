import { Box } from "ui";
import React from "react";

type CardVariant = "opaque" | "translucent" | "panel";

interface CardProps extends React.ComponentProps<typeof Box> {
    variant?: CardVariant;
}

export default function Card({ children, variant = "opaque", ...props }: CardProps) {
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
