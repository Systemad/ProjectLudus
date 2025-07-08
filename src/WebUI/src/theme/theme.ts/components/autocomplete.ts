import { mergeMultiStyle } from "@yamada-ui/react";
import type { ComponentMultiStyle } from "@yamada-ui/react";
import { Select } from "./select";

export const Autocomplete: ComponentMultiStyle<"Autocomplete"> =
  mergeMultiStyle(Select)();
