import { mergeMultiStyle } from "@yamada-ui/react";
import type { ComponentMultiStyle } from "@yamada-ui/react";
import { Modal } from "./modal";

export const Dialog: ComponentMultiStyle<"Dialog"> = mergeMultiStyle(Modal)();
