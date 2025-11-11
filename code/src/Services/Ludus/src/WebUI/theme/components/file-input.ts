import { mergeMultiStyle } from "@yamada-ui/react";
import type { ComponentMultiStyle } from "@yamada-ui/react";
import { Input } from "./input";

export const FileInput: ComponentMultiStyle<"FileInput"> =
  mergeMultiStyle(Input)();
