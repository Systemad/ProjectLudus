import type { ComponentStyle } from "@yamada-ui/react";

export const SlideFade: ComponentStyle<"SlideFade"> = {
  baseStyle: {
    w: "100%",
  },
  defaultProps: {
    offsetX: 0,
    offsetY: 8,
    reverse: true,
  },
};
