import { mergeMultiStyle } from "@yamada-ui/react";
import type { ComponentMultiStyle } from "@yamada-ui/react";
import { DatePicker } from "./date-picker";

export const MonthPicker: ComponentMultiStyle<"MonthPicker"> =
  mergeMultiStyle(DatePicker)();
