import { mergeStyle } from "@yamada-ui/react";
import type { ComponentStyle } from "@yamada-ui/react";
import { Button } from "./button";

export const FileButton: ComponentStyle<"FileButton"> = mergeStyle(Button)();
