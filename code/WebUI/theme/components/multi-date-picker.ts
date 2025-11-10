import { mergeMultiStyle } from "@yamada-ui/react";
import type { ComponentMultiStyle } from "@yamada-ui/react";
import { DatePicker } from "./date-picker";

export const MultiDatePicker: ComponentMultiStyle<"MultiDatePicker"> =
  mergeMultiStyle(DatePicker)();
