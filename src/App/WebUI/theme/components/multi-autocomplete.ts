import { mergeMultiStyle } from "@yamada-ui/react";
import type { ComponentMultiStyle } from "@yamada-ui/react";
import { MultiSelect } from "./multi-select";

export const MultiAutocomplete: ComponentMultiStyle<"MultiAutocomplete"> =
  mergeMultiStyle(MultiSelect)();
