import { useCallback } from "react";
import { Link } from "@tanstack/react-router";
import { Box, Button, Drawer, Flex, Heading, MenuIcon, Text, VStack, useDisclosure } from "ui";

import { RouterLink, RouterLinkButton } from "@src/components/router-link";
import {
    ChevronDownIcon,
    NavArrowSvg,
    NavMenuArrow,
    NavMenuContent,
    NavMenuIcon,
    NavMenuItem,
    NavMenuLink,
    NavMenuList,
    NavMenuPopup,
    NavMenuPortal,
    NavMenuPositioner,
    NavMenuRoot,
    NavMenuTrigger,
    NavMenuViewport,
} from "@src/components/navigation-menu";

type NavigationBarProps = {
    active?: string;
};

const subNavItems = [
    {
        id: "search" as const,
        label: "Search",
        subItems: [
            { label: "Games", to: "/games/search" },
            { label: "Companies", to: "/companies/search" },
        ],
    },
    {
        id: "events" as const,
        label: "Events",
        subItems: [{ label: "Upcoming Events", to: "/events" }],
    },
    { id: "calendar" as const, label: "Calendar", to: "/calendar" },

    { id: "companies" as const, label: "Companies", to: "/companies/search" },
];

type SubItem = { label: string; to: string };

function SubLinkList({ items, onNav }: { items?: SubItem[]; onNav: () => void }) {
    if (!items) return null;
    return (
        <VStack align="stretch" gap="1">
            {items.map((item) => (
                <NavMenuLink
                    key={item.to}
                    render={<Link to={item.to} onClick={onNav} />}
                    closeOnClick
                    px="4"
                    py="2"
                    rounded="md"
                    whiteSpace="nowrap"
                    color="fg.base"
                    textDecoration="none"
                    _hover={{ bg: "whiteAlpha.100" }}
                >
                    {item.label}
                </NavMenuLink>
            ))}
        </VStack>
    );
}

export function NavigationBar({ active: _active = "home" }: NavigationBarProps) {
    const { open, onOpen, onClose } = useDisclosure();
    const closeDrawer = useCallback(() => onClose(), [onClose]);

    return (
        <>
            <Box as="nav" borderBottom="1px" borderColor="border" bg="bg.panel">
                <Flex
                    maxW="7xl"
                    mx="auto"
                    px={{ base: "4", md: "6" }}
                    align="center"
                    justify="space-between"
                    h="14"
                >
                    <Flex align="center" gap={{ base: "2", md: "4" }} minW="0">
                        <Button
                            display={{ base: "inline-flex", md: "none" }}
                            aria-label="Open navigation"
                            variant="ghost"
                            color="fg.base"
                            onClick={onOpen}
                        >
                            <MenuIcon boxSize="4" />
                        </Button>

                        <RouterLink to="/" style={{ color: "inherit", textDecoration: "none" }}>
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

                    <NavMenuRoot>
                        <NavMenuList display={{ base: "none", md: "flex" }}>
                            {subNavItems.map((item) => (
                                <NavMenuItem key={item.id}>
                                    {"subItems" in item ? (
                                        <>
                                            <NavMenuTrigger>
                                                {item.label}
                                                <NavMenuIcon>
                                                    <ChevronDownIcon />
                                                </NavMenuIcon>
                                            </NavMenuTrigger>
                                            <NavMenuContent>
                                                <SubLinkList
                                                    items={item.subItems}
                                                    onNav={() => {}}
                                                />
                                            </NavMenuContent>
                                        </>
                                    ) : (
                                        <NavMenuLink render={<Link to={item.to!} />} closeOnClick>
                                            {item.label}
                                        </NavMenuLink>
                                    )}
                                </NavMenuItem>
                            ))}
                        </NavMenuList>

                        <NavMenuPortal>
                            <NavMenuPositioner
                                sideOffset={10}
                                collisionPadding={{ top: 5, bottom: 5, left: 20, right: 20 }}
                            >
                                <NavMenuPopup>
                                    <NavMenuArrow>
                                        <NavArrowSvg />
                                    </NavMenuArrow>
                                    <NavMenuViewport />
                                </NavMenuPopup>
                            </NavMenuPositioner>
                        </NavMenuPortal>
                    </NavMenuRoot>
                </Flex>
            </Box>

            <Drawer.Root
                open={open}
                onClose={onClose}
                placement="inline-start"
                withCloseButton={false}
            >
                <Drawer.Overlay zIndex="beerus" />
                <Drawer.Content zIndex="beerus">
                    <Drawer.CloseButton />
                    <Drawer.Header>
                        <Heading
                            as="span"
                            fontFamily="heading"
                            fontSize="xl"
                            fontWeight="black"
                            letterSpacing="tight"
                            textTransform="uppercase"
                            color="fg.base"
                        >
                            Game-Index
                        </Heading>
                    </Drawer.Header>

                    <Drawer.Body>
                        <VStack align="stretch" gap="2">
                            {subNavItems.map((item) =>
                                "subItems" in item ? (
                                    item.subItems?.map((sub) => (
                                        <RouterLinkButton
                                            key={`drawer-${sub.to}`}
                                            to={sub.to}
                                            variant="ghost"
                                            color="fg.base"
                                            colorScheme="blackAlpha"
                                            justifyContent="start"
                                            rounded="lg"
                                            px="3"
                                            py="2"
                                            w="full"
                                            h="auto"
                                            onClick={closeDrawer}
                                            _hover={{ bg: "rgba(255,255,255,0.12)" }}
                                            activeProps={{
                                                bg: "rgba(255,255,255,0.18)",
                                                color: "fg.base",
                                            }}
                                        >
                                            <Text
                                                as="span"
                                                fontFamily="heading"
                                                fontWeight="bold"
                                                letterSpacing="tight"
                                                color="inherit"
                                            >
                                                {sub.label}
                                            </Text>
                                        </RouterLinkButton>
                                    ))
                                ) : (
                                    <RouterLinkButton
                                        key={`drawer-${item.id}`}
                                        to={item.to!}
                                        variant="ghost"
                                        color="fg.base"
                                        colorScheme="blackAlpha"
                                        justifyContent="start"
                                        rounded="lg"
                                        px="3"
                                        py="2"
                                        w="full"
                                        h="auto"
                                        onClick={closeDrawer}
                                        _hover={{ bg: "rgba(255,255,255,0.12)" }}
                                        activeProps={{
                                            bg: "rgba(255,255,255,0.18)",
                                            color: "fg.base",
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
                                ),
                            )}
                        </VStack>
                    </Drawer.Body>

                    <Drawer.Footer>
                        <Drawer.CloseTrigger>
                            <Button variant="ghost">Close</Button>
                        </Drawer.CloseTrigger>
                    </Drawer.Footer>
                </Drawer.Content>
            </Drawer.Root>
        </>
    );
}
