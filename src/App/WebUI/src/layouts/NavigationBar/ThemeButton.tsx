import { MoonIcon, SunIcon } from "@phosphor-icons/react";
import {
    IconButton,
    Menu,
    useBreakpointValue,
    useColorMode,
} from "@yamada-ui/react";

export const ThemeButton = () => {
    const padding = useBreakpointValue({ base: 32, md: 16 });
    const { changeColorMode, colorMode } = useColorMode();

    return (
        <Menu
            modifiers={[
                {
                    name: "preventOverflow",
                    options: {
                        padding: {
                            bottom: padding,
                            left: padding,
                            right: padding,
                            top: padding,
                        },
                    },
                },
            ]}
            placement="bottom"
            restoreFocus={false}
        >
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
        </Menu>
    );
};
