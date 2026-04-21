import {
    Box,
    Button,
    Drawer,
    Flex,
    Heading,
    HStack,
    MenuIcon,
    Text,
    VStack,
    useDisclosure,
} from "ui";
import { useState } from "react";
import { useMotionValueEvent, useScroll } from "motion/react";
import { appShellNavInset } from "@src/components/AppShell/layout.constants";
import { RouterLink, RouterLinkButton } from "@src/components/YamadaLink/YamadaLink";
import { linkStyle } from "@src/utils/sectionTextStyles";

type NavigationBarProps = {
    active?: string;
};

const navItems = [
    { id: "search" as const, label: "Search", to: "/games/search" },
    { id: "calendar" as const, label: "Calendar", to: "/calendar" },
    { id: "events" as const, label: "Events", to: "/events" },
    { id: "companies" as const, label: "Companies", to: "/companies/search" },
];

export function NavigationBar({ active: _active = "home" }: NavigationBarProps) {
    const { open, onOpen, onClose } = useDisclosure();
    const { scrollY } = useScroll();
    const [isScrolled, setIsScrolled] = useState(() => scrollY.get() > 18);

    useMotionValueEvent(scrollY, "change", (latest) => {
        setIsScrolled((previous) => {
            const next = latest > 18;
            return previous === next ? previous : next;
        });
    });

    return (
        <Box as="nav" position="fixed" insetX="0" top="0" zIndex="zarbon" pointerEvents="none">
            <Flex
                w="full"
                maxW="7xl"
                mx="auto"
                px={{ base: "4", md: "6", xl: "8" }}
                pt={appShellNavInset}
                pointerEvents="none"
            >
                <Flex
                    w="full"
                    pointerEvents="auto"
                    colorScheme="gray"
                    rounded="full"
                    borderWidth={isScrolled ? "1px" : "0"}
                    borderColor={isScrolled ? "border.subtle" : "transparent"}
                    bg={isScrolled ? "bg.surface" : "transparent"}
                    backdropBlur="xl"
                    boxShadow={isScrolled ? "lg" : "none"}
                    px={{ base: "3", md: "4", xl: "5" }}
                    py={{ base: "1.5", md: "2" }}
                    gap={{ base: "3", md: "6" }}
                    transitionProperty="background-color, border-color, box-shadow"
                    transitionDuration="slower"
                    transitionTimingFunction="ease-out"
                    align="center"
                    justify="space-between"
                >
                    <Flex align="center" gap={{ base: "2", md: "4" }} flex="1" minW="0">
                        <Button
                            display={{ base: "inline-flex", md: "none" }}
                            aria-label="Open navigation"
                            variant="ghost"
                            color="gray.fg"
                            onClick={onOpen}
                        >
                            <MenuIcon boxSize="4" />
                        </Button>

                        <Drawer.Root open={open} onClose={onClose} placement="inline-start">
                            <Drawer.Overlay zIndex="beerus" />
                            <Drawer.Content bg="bg.surface" colorScheme="gray" zIndex="beerus">
                                <Drawer.CloseButton />
                                <Drawer.Header>
                                    <Heading
                                        as="span"
                                        fontFamily="heading"
                                        fontSize="xl"
                                        fontWeight="black"
                                        letterSpacing="tight"
                                        textTransform="uppercase"
                                        bgClip="text"
                                        bgGradient="linear(to-l, #C6426E, #642B73)"
                                    >
                                        Game-Index
                                    </Heading>
                                </Drawer.Header>

                                <Drawer.Body>
                                    <VStack align="stretch" gap="2">
                                        {navItems.map((item) => (
                                            <RouterLinkButton
                                                key={`drawer-${item.id}`}
                                                to={item.to}
                                                variant="ghost"
                                                color="gray.fg"
                                                justifyContent="start"
                                                rounded="lg"
                                                px="3"
                                                py="2"
                                                w="full"
                                                h="auto"
                                                onClick={onClose}
                                                activeProps={{
                                                    bg: "gray.subtle",
                                                    color: "gray.fg",
                                                }}
                                            >
                                                <Text
                                                    as="span"
                                                    fontFamily="heading"
                                                    fontWeight="bold"
                                                    letterSpacing="tight"
                                                    color="inherit"
                                                >
                                                    {item.label}
                                                </Text>
                                            </RouterLinkButton>
                                        ))}
                                    </VStack>
                                </Drawer.Body>

                                <Drawer.Footer>
                                    <Drawer.CloseTrigger>
                                        <Button variant="ghost">とじる</Button>
                                    </Drawer.CloseTrigger>
                                </Drawer.Footer>
                            </Drawer.Content>
                        </Drawer.Root>

                        <RouterLink to="/" style={{ ...linkStyle, color: "inherit" }}>
                            <Heading
                                as="span"
                                fontFamily="heading"
                                fontSize={{ base: "xl", md: "2xl" }}
                                fontWeight="black"
                                letterSpacing="tight"
                                textTransform="uppercase"
                                bgClip="text"
                                bgGradient="linear(to-l, #C6426E, #642B73)"
                                whiteSpace="nowrap"
                            >
                                Game-Index
                            </Heading>
                        </RouterLink>
                    </Flex>

                    <HStack
                        display={{ base: "none", md: "flex" }}
                        gap={{ base: "2", lg: "3" }}
                        flex="1"
                        justify="center"
                    >
                        {navItems.map((item) => (
                            <RouterLinkButton
                                key={item.id}
                                to={item.to}
                                variant="ghost"
                                color="gray.fg"
                                rounded="full"
                                px={{ md: "3", xl: "4" }}
                                py="1"
                                minW="0"
                                h="auto"
                                activeProps={{ bg: "gray.subtle", color: "gray.fg" }}
                            >
                                <Text
                                    as="span"
                                    fontFamily="heading"
                                    fontSize={{ base: "md", xl: "lg" }}
                                    fontWeight="bold"
                                    letterSpacing="tight"
                                    color="inherit"
                                >
                                    {item.label}
                                </Text>
                            </RouterLinkButton>
                        ))}
                    </HStack>

                    <Flex flex="1" justify="flex-end" minW="0">
                        <RouterLinkButton
                            to="/games/search"
                            display={{ base: "none", md: "inline-flex" }}
                            variant="outline"
                            colorScheme="gray"
                            rounded="full"
                            px={{ md: "4", xl: "5" }}
                            py="1.5"
                            fontSize={{ md: "md", xl: "lg" }}
                            borderColor={isScrolled ? "border.emphasized" : "border.subtle"}
                            bg={isScrolled ? "gray.subtle" : "transparent"}
                            activeProps={{ bg: "gray.subtle", color: "gray.fg" }}
                        >
                            Browse games
                        </RouterLinkButton>
                    </Flex>
                </Flex>
            </Flex>
        </Box>
    );
}

export default NavigationBar;
