import { IconButton, SunIcon, useColorMode, MoonIcon } from "@packages/ui";

export const ThemeButton = () => {
    const { changeColorMode, colorMode } = useColorMode();

    return (
        <IconButton
            variant="ghost"
            aria-label="Toggle theme"
            color="muted"
            icon={
                colorMode === "dark" ? (
                    <SunIcon fontSize="1.5rem" />
                ) : (
                    <MoonIcon fontSize="1.5rem" />
                )
            }
            rounded={"xl"}
            _hover={{ bg: [`blackAlpha.100`, `whiteAlpha.50`] }}
            onClick={() =>
                changeColorMode(colorMode == "dark" ? "light" : "dark")
            }
        />
    );
};
