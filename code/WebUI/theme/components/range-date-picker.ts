import { mergeMultiStyle } from "@yamada-ui/react";
import type { ComponentMultiStyle } from "@yamada-ui/react";
import { DatePicker } from "./date-picker";

export const RangeDatePicker: ComponentMultiStyle<"RangeDatePicker"> =
  mergeMultiStyle(DatePicker)();
