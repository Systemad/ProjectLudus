import type { ComponentStyle } from "@yamada-ui/react";

export const ScaleFade: ComponentStyle<"ScaleFade"> = {
  baseStyle: {
    w: "100%",
  },
  defaultProps: {
    reverse: true,
    scale: 0.95,
  },
};
