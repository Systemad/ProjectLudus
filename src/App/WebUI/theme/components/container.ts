import type { ComponentStyle } from "@yamada-ui/react";

export const Container: ComponentStyle<"Container"> = {
    baseStyle: {
        display: "flex",
        flexDirection: "column",
        gap: { base: "lg", sm: "md" },
        px: { base: "md", sm: "lg", lg: "2xl" },
        w: "100%",
    },
};
