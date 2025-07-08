import type { ThemeConfig } from "@yamada-ui/react";

export const config: ThemeConfig = {
  breakpoint: { direction: "down", identifier: "@media screen" },
  initialColorMode: "light",
  initialThemeScheme: "base",
  var: { prefix: "ui" },
};
