import { colorsTuple, createTheme } from "@mantine/core";
import { DEFAULT_THEME, mergeMantineTheme } from "@mantine/core";
export const themeOverride = createTheme({
    primaryColor: "m3_primary",
    colors: {
        m3_primary: colorsTuple("#625690"),
        m3_onPrimary: colorsTuple("#33275e"),
        m3_secondary: colorsTuple("#cac3dc"),
        m3_onSecondary: colorsTuple("#322e41"),

        m3_primaryContainer: colorsTuple("#4a3e76"),
        m3_onPrimaryContainer: colorsTuple("#e7deff"),
        m3_secondaryContainer: colorsTuple("#494458"),
        m3_onSecondaryContainer: colorsTuple("#e7dff8"),

        m3_tertiary: colorsTuple("#eeb8ca"),
        m3_onTertiary: colorsTuple("#492534"),
        m3_tertiaryContainer: colorsTuple("#623b4a"),
        m3_ontertiaryContainer: colorsTuple("#ffd9e4"),

        m3_error: colorsTuple("#ffb4ab"),
        m3_onError: colorsTuple("#690005"),
        m3_ErrorContainer: colorsTuple("#93000a"),
        m3_onErrorContainer: colorsTuple("#ffdad6"),

        m3_surfaceDim: colorsTuple("#141318"),
        m3_surface: colorsTuple("#141318"),
        m3_surfaceBright: colorsTuple("#3a383e"),
        m3_surfaceLowestContainer: colorsTuple("#0f0d13"),

        m3_surfaceContainerLow: colorsTuple("#1c1b20"),
        m3_surfaceContainer: colorsTuple("#201f24"),
        m3_surfaceContainerHigher: colorsTuple("#2b292f"),
        m3_surfaceContainerHighest: colorsTuple("#36343a"),

        m3_onSurface: colorsTuple("#e6e1e9"),
        m3_onSurfaceVariant: colorsTuple("#cac4cf"),
        m3_outline: colorsTuple("#938f99"),
        m3_outlineVariant: colorsTuple("#48454e"),
    },
});

export const myTheme = mergeMantineTheme(DEFAULT_THEME, themeOverride);
