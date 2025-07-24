import type { ComponentStyle } from "@yamada-ui/react";

export const Container: ComponentStyle<"Container"> = {
  baseStyle: {
    display: "flex",
    flexDirection: "column",
    gap: { base: "lg", sm: "md" },
    p: { base: "lg", sm: "md" },
    w: "100%",
  },
};
