import { useColorScheme } from "react-native";

export type AppTheme = {
  background: string;
  surface: string;
  surfaceHigh: string;
  primary: string;
  onPrimary: string;
  primaryContainer: string;
  onPrimaryContainer: string;
  text: string;
  textMuted: string;
  outline: string;
};

const fallback = {
  light: {
    background: "#FFFBFE",
    surface: "#FFFBFE",
    surfaceHigh: "#ECE6F0",
    primary: "#6750A4",
    onPrimary: "#FFFFFF",
    primaryContainer: "#EADDFF",
    onPrimaryContainer: "#21005D",
    text: "#1D1B20",
    textMuted: "#49454F",
    outline: "#79747E",
  },
  dark: {
    background: "#141218",
    surface: "#141218",
    surfaceHigh: "#2B2930",
    primary: "#D0BCFF",
    onPrimary: "#381E72",
    primaryContainer: "#4F378B",
    onPrimaryContainer: "#EADDFF",
    text: "#E6E1E5",
    textMuted: "#CAC4D0",
    outline: "#938F99",
  },
} satisfies Record<"light" | "dark", AppTheme>;

export function useAppTheme(): AppTheme {
  const scheme = useColorScheme() === "dark" ? "dark" : "light";
  return fallback[scheme];
}
