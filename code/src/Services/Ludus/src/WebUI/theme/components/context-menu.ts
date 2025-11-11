import { mergeMultiStyle } from "@yamada-ui/react";
import type { ComponentMultiStyle } from "@yamada-ui/react";
import { Menu } from "./menu";

export const ContextMenu: ComponentMultiStyle<"ContextMenu"> = mergeMultiStyle(
  Menu,
  {
    baseStyle: {
      trigger: {},
    },
  },
)();
