import type {
    CenterProps,
    DrawerProps,
    IconButtonProps,
    MenuProps,
    UseDisclosureReturn,
} from "@yamada-ui/react";
import type { FC } from "react";

import { ListIcon as MenuIcon, MoonIcon, SunIcon } from "@phosphor-icons/react";
import {
    Box,
    Center,
    CloseButton,
    Collapse,
    Drawer,
    DrawerHeader,
    forwardRef,
    HStack,
    IconButton,
    mergeRefs,
    Spacer,
    useBreakpoint,
    useBreakpointValue,
    useColorMode,
    useDisclosure,
    useMotionValueEvent,
    useScroll,
    VStack,
    Menu,
    ButtonGroup as YButtonGroup,
    Link as YamadaLink,
} from "@yamada-ui/react";
import { memo, useEffect, useRef, useState } from "react";
import GithubIcon from "~/icons/GitHubIcon";
import { Link } from "@tanstack/react-router";
import { CustomYamadaLink } from "../CustomLink/CustomYamadaLink";
import { HeaderSearchbar } from "./Searchbar";
import { AuthButton } from "~/features/auth/AuthButton";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
export interface HeaderProps extends CenterProps {}

export const Header = memo(
    forwardRef<HeaderProps, "div">(({ ...rest }, ref) => {
        const headerRef = useRef<HTMLHeadingElement>(null);
        const { scrollY } = useScroll();
        const [y, setY] = useState<number>(0);
        const menuControls = useDisclosure();
        const searchControls = useDisclosure();
        const { height = 0 } = headerRef.current?.getBoundingClientRect() ?? {};

        useMotionValueEvent(scrollY, "change", setY);

        const isScroll = y > height;

        return (
            <>
                <Center
                    ref={mergeRefs(ref, headerRef)}
                    as="header"
                    left="0"
                    position="sticky"
                    right="0"
                    top="3"
                    w="full"
                    zIndex="yamcha"
                    {...rest}
                >
                    <Center
                        maxW="7xl"
                        rounded="3xl"
                        px={{ base: "lg", lg: "0" }}
                        w="full"
                    >
                        <VStack
                            backdropBlur="10px"
                            backdropFilter="auto"
                            backdropSaturate="120%"
                            bg={["blackAlpha.50", "whiteAlpha.100"]}
                            gap="0"
                            px={{ base: "lg", lg: "md" }}
                            py="3"
                            rounded="3xl"
                            shadow={isScroll ? ["base", "dark-sm"] : undefined}
                            transitionDuration="slower"
                            transitionProperty="common"
                            w="full"
                        >
                            <HStack gap={{ base: "md", sm: "sm" }}>
                                <Link to="/">Ludus</Link>
                                <YButtonGroup
                                    as="nav"
                                    size="sm"
                                    fontWeight="normal"
                                    gap="md"
                                    variant="ghost"
                                >
                                    <CustomYamadaLink to="/games" rounded="lg">
                                        Games
                                    </CustomYamadaLink>
                                    <CustomYamadaLink
                                        to="/calendar"
                                        rounded="lg"
                                    >
                                        Calender
                                    </CustomYamadaLink>
                                    <CustomYamadaLink
                                        to="/popular"
                                        rounded="lg"
                                    >
                                        Popular
                                    </CustomYamadaLink>
                                </YButtonGroup>
                                <Spacer />
                                <HeaderSearchbar />
                                <AuthButton />
                                {/*
                                    <ButtonGroup {...menuControls} />                            
                                */}
                            </HStack>

                            <Collapse open={searchControls.open}>
                                <Box p="1"></Box>
                            </Collapse>
                        </VStack>
                    </Center>
                </Center>

                <MobileMenu {...menuControls} />
            </>
        );
    })
);
// external={href.startsWith("https://")}
interface ButtonGroupProps extends Partial<UseDisclosureReturn> {
    isMobile?: boolean;
}

const ButtonGroup: FC<ButtonGroupProps> = memo(
    ({ isMobile, open, onClose, onOpen }) => {
        return (
            <HStack gap="sm">
                <IconButton
                    as={YamadaLink}
                    href="https://github.com/Systemad/vrs-standings"
                    variant="ghost"
                    aria-label="GitHub repository"
                    color="muted"
                    display={{
                        base: "inline-flex",
                        lg: !isMobile ? "none" : undefined,
                    }}
                    icon={<GithubIcon />}
                    isExternal
                    rounded="xl"
                    _hover={{ bg: [`blackAlpha.100`, `whiteAlpha.50`] }}
                />

                <ColorModeButton
                    display={{
                        base: "inline-flex",
                        sm: !isMobile ? "none" : undefined,
                    }}
                />

                {!open ? (
                    <IconButton
                        variant="ghost"
                        aria-label="Open navigation menu"
                        color="muted"
                        display={{ base: "none", lg: "inline-flex" }}
                        icon={<MenuIcon fontSize="1.5rem" />}
                        rounded="xl"
                        _hover={{ bg: [`blackAlpha.100`, `whiteAlpha.50`] }}
                        onClick={onOpen}
                    />
                ) : (
                    <CloseButton
                        size="lg"
                        aria-label="Close navigation menu"
                        color="muted"
                        display={{ base: "none", lg: "inline-flex" }}
                        rounded="xl"
                        _hover={{ bg: [`blackAlpha.100`, `whiteAlpha.50`] }}
                        onClick={onClose}
                    />
                )}
            </HStack>
        );
    }
);

ButtonGroup.displayName = "ButtonGroup";

interface ColorModeButtonProps extends IconButtonProps {
    menuProps?: MenuProps;
}

const ColorModeButton: FC<ColorModeButtonProps> = memo(
    ({ menuProps, ...rest }) => {
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
                {...menuProps}
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
                    {...rest}
                />
            </Menu>
        );
    }
);

ColorModeButton.displayName = "ColorModeButton";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface MobileMenuProps extends DrawerProps {}

const MobileMenu: FC<MobileMenuProps> = memo(({ open, onClose }) => {
    const breakpoint = useBreakpoint();

    useEffect(() => {
        if (!["lg", "md", "sm"].includes(breakpoint)) onClose?.();
    }, [breakpoint, onClose]);

    return (
        <Drawer
            fullHeight={true}
            open={open}
            roundedLeft="xl"
            w="auto"
            withCloseButton={false}
            onClose={onClose}
        >
            <DrawerHeader
                fontSize="md"
                fontWeight="normal"
                justifyContent="flex-end"
                pt="sm"
            >
                <ButtonGroup isMobile {...{ open, onClose }} />
            </DrawerHeader>
        </Drawer>
    );
});

MobileMenu.displayName = "MobileMenu";
