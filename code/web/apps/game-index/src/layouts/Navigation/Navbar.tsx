"use client";

import {
    NavigationMenu,
    NavigationMenuList,
    NavigationMenuTrigger,
    NavigationMenuItem,
    NavigationMenuContent,
} from "./nav-menu";

import {
    Box,
    Separator,
    Flex,
    Text,
    HStack,
    MenuIcon,
    Grid,
    Card,
    Heading,
    List,
    Link,
    Accordion,
    useDisclosure,
    Drawer,
    Input,
    InputGroup,
    Button,
    GlobeIcon,
    SwordIcon,
    MonitorIcon,
    GamepadIcon,
    UsersIcon,
    ChartNoAxesCombinedIcon,
    SearchIcon,
} from "@packages/ui";
import { ThemeButton } from "./ThemeButton";
import { RouterLink } from "../Links/YamadaLink.tsx";

const MENU_DATA = {
    GENRES: [
        { name: "RPG", href: "/rpg", icon: <SwordIcon /> },
        { name: "Open-world", href: "/open-world", icon: <GlobeIcon /> },
    ],
    PLATFORMS: [
        { name: "PC", href: "/pc", icon: <MonitorIcon /> },
        { name: "Console", href: "/console", icon: <GamepadIcon /> },
    ],
    OTHER: [
        { name: "Top Sellers", href: "/top-sellers", icon: <MonitorIcon /> },
        { name: "Most Played", href: "/most-played", icon: <UsersIcon /> },
        {
            name: "Top New Releases",
            href: "/top-new-releases",
            icon: <ChartNoAxesCombinedIcon />,
        },
    ],
    Browse: [
        {
            name: "Search",
            href: "/search",
            icon: <MonitorIcon />,
            description: "Search games within IGDB",
        },
    ],
} as const;

const overviewLinks = [
    {
        href: "/react/overview/quick-start",
        title: "Quick Start",
        description: "Install and assemble your first component.",
    },
    {
        href: "/react/overview/accessibility",
        title: "Accessibility",
        description: "Learn how we build accessible components.",
    },
    {
        href: "/react/overview/releases",
        title: "Releases",
        description: "See whats new in the latest Base UI versions.",
    },
    {
        href: "/react/overview/about",
        title: "About",
        description: "Learn more about Base UI and our mission.",
    },
] as const;

const handbookLinks = [
    {
        href: "/react/handbook/styling",
        title: "Styling",
        description:
            "Base UI components can be styled with plain CSS, Tailwind CSS, CSS-in-JS, or CSS Modules.",
    },
    {
        href: "/react/handbook/animation",
        title: "Animation",
        description:
            "Base UI components can be animated with CSS transitions, CSS animations, or JavaScript libraries.",
    },
    {
        href: "/react/handbook/composition",
        title: "Composition",
        description:
            "Base UI components can be replaced and composed with your own existing components.",
    },
] as const;

export const NavBar = () => (
    <Box
        as="header"
        w="full"
        bg="bg.float"
        backdropBlur="xl"
        position="sticky"
        top="0"
        zIndex="50"
    >
        <Flex
            maxW="8xl"
            w="full"
            mx="auto"
            px={{ base: "sm", md: "8" }}
            py="2"
            align="center"
            justify="space-between"
            gap="4"
        >
            <HStack display={{ base: "flex", md: "none" }} w="full" gap="4">
                <MobileMenu />
                <Box flex="1">
                    <InputGroup.Root variant="filled" w="full">
                        <Input placeholder="Search 300,000 games" />
                        <InputGroup.Element>
                            <SearchIcon />
                        </InputGroup.Element>
                    </InputGroup.Root>
                </Box>
                <ThemeButton />
            </HStack>

            <Flex
                display={{ base: "none", md: "flex" }}
                align="center"
                w="full"
                gap="6"
            >
                <HStack gap="xs">
                    <Text fontSize={"1rem"} fontWeight={"500"} cursor="pointer">
                        Home
                    </Text>
                    <NavigationMenu>
                        <DesktopNavBar />
                    </NavigationMenu>
                </HStack>

                <Box flex="1">
                    <InputGroup.Root variant="filled" w="full">
                        <InputGroup.Element>
                            <SearchIcon />
                        </InputGroup.Element>
                        <Input placeholder="Search 500,000 games" />
                    </InputGroup.Root>
                </Box>

                <HStack>
                    <ThemeButton />
                </HStack>
            </Flex>
        </Flex>
    </Box>
);
export const MobileMenu = () => {
    const { open, onOpen, onClose } = useDisclosure();
    return (
        <Drawer.Root
            open={open}
            onClose={onClose}
            placement={"inline-start"}
            size="xs"
            duration={0.25}
        >
            <Drawer.OpenTrigger onClick={onOpen}>
                <MenuIcon fontSize={"xl"} />
            </Drawer.OpenTrigger>

            <Drawer.Content margin="0">
                <Drawer.Header fontSize={"2xl"}>GAME INDEX</Drawer.Header>

                <Drawer.Body padding={"xs"}>
                    <Box w="full">
                        <List.Root gap={"xs"}>
                            {MENU_DATA.Browse.map((item) => (
                                <List.Item
                                    padding={"sm"}
                                    _hover={{
                                        backgroundColor: "bg.muted",
                                        borderRadius: "xl",
                                    }}
                                >
                                    <RouterLink
                                        to="/"
                                        onClick={onClose}
                                        style={{
                                            textDecoration: "none",
                                            color: "inherit",
                                        }}
                                    >
                                        <List.Icon>{item.icon}</List.Icon>
                                        <Box>
                                            <Text
                                                fontSize="sm"
                                                fontWeight="semibold"
                                            >
                                                {item.name}
                                            </Text>
                                            <Text
                                                color="muted"
                                                fontSize="xs"
                                                fontWeight="normal"
                                            >
                                                {item.description}
                                            </Text>
                                        </Box>
                                    </RouterLink>
                                </List.Item>
                            ))}
                        </List.Root>
                        <Accordion.Root multiple>
                            <Accordion.Item index={0}>
                                <Accordion.Button
                                    fontSize={"md"}
                                    fontWeight={"bold"}
                                >
                                    Explore
                                </Accordion.Button>
                                <Text
                                    casing="capitalize"
                                    textTransform={"uppercase"}
                                    fontWeight={"bold"}
                                    fontSize={"xs"}
                                    mb={"sm"}
                                >
                                    Genres
                                </Text>

                                <List.Root gap="0.0625rem" mb={"lg"}>
                                    {MENU_DATA.GENRES.map((item) => (
                                        <List.Item
                                            padding={"sm"}
                                            borderRadius="xl"
                                            _hover={{
                                                backgroundColor: "bg.muted",
                                                borderRadius: "xl",
                                            }}
                                        >
                                            <List.Icon>{item.icon}</List.Icon>
                                            {item.name}
                                        </List.Item>
                                    ))}
                                </List.Root>

                                <Text
                                    casing="capitalize"
                                    textTransform={"uppercase"}
                                    fontWeight={"bold"}
                                    fontSize={"xs"}
                                    mb={"sm"}
                                >
                                    OTHER
                                </Text>

                                <List.Root gap="0.0625rem">
                                    {MENU_DATA.OTHER.map((item) => (
                                        <List.Item
                                            padding={"sm"}
                                            borderRadius="xl"
                                            _hover={{
                                                backgroundColor: "bg.muted",
                                                borderRadius: "xl",
                                            }}
                                        >
                                            <List.Icon>{item.icon}</List.Icon>
                                            {item.name}
                                        </List.Item>
                                    ))}
                                </List.Root>
                            </Accordion.Item>
                        </Accordion.Root>
                    </Box>
                </Drawer.Body>

                <Drawer.Footer>
                    <Drawer.CloseTrigger>
                        <Button variant="ghost">Close</Button>
                    </Drawer.CloseTrigger>
                </Drawer.Footer>
            </Drawer.Content>
        </Drawer.Root>
    );
};
export const DesktopNavBar = () => {
    return (
        <NavigationMenuList>
            <NavigationMenuItem>
                <NavigationMenuTrigger>Explore</NavigationMenuTrigger>
                <NavigationMenuContent>
                    <Grid w="lg" templateColumns="1fr 1fr" gap="md" p="md">
                        <Box
                            borderRight="1px solid"
                            borderColor="border"
                            pr="lg"
                        >
                            <Text
                                fontSize="xs"
                                fontWeight="semibold"
                                color="muted"
                                mb="md"
                                textTransform="uppercase"
                            >
                                Rankings
                            </Text>
                            <List.Root gap="xs" mb="lg">
                                {MENU_DATA.OTHER.map((item) => (
                                    <List.Item
                                        padding="sm"
                                        borderRadius="xl"
                                        _hover={{
                                            backgroundColor: "bg.muted",
                                            borderRadius: "xl",
                                        }}
                                    >
                                        <List.Icon>{item.icon}</List.Icon>
                                        <Text fontSize="sm" fontWeight="normal">
                                            {item.name}
                                        </Text>
                                    </List.Item>
                                ))}
                            </List.Root>
                            <Separator mb="md" />
                            <Text
                                fontSize="xs"
                                fontWeight="semibold"
                                color="muted"
                                mb="md"
                                textTransform="uppercase"
                            >
                                Others
                            </Text>
                            <List.Root gap="0.0625rem">
                                {/* Others List.Items */}
                            </List.Root>
                        </Box>

                        {/* Right Column */}
                        <Box>
                            <Text
                                fontSize="xs"
                                fontWeight="semibold"
                                color="muted"
                                mb="md"
                                textTransform="uppercase"
                            >
                                Genres
                            </Text>
                            <List.Root gap="0.0625rem">
                                {MENU_DATA.GENRES.map((item) => (
                                    <List.Item
                                        padding="sm"
                                        borderRadius="xl"
                                        _hover={{
                                            backgroundColor: "bg.muted",
                                            borderRadius: "xl",
                                        }}
                                    >
                                        <List.Icon>{item.icon}</List.Icon>
                                        <Text fontSize="sm" fontWeight="normal">
                                            {item.name}
                                        </Text>
                                    </List.Item>
                                ))}
                            </List.Root>
                        </Box>
                    </Grid>
                </NavigationMenuContent>
            </NavigationMenuItem>

            <NavigationMenuItem>
                <NavigationMenuTrigger>Overview</NavigationMenuTrigger>
                <NavigationMenuContent>
                    <Grid
                        templateColumns={{ base: "1fr", sm: "12rem 12rem" }}
                        listStyle={"none"}
                        padding={0}
                        margin={0}
                    >
                        {overviewLinks.map((item) => (
                            <Card.Root
                                border={"none"}
                                backgroundColor={"transparent"}
                                _hover={{
                                    backgroundColor: "bg.subtle",
                                }}
                                textDecoration={"none"}
                            >
                                <Card.Header>
                                    <Heading size="lg">{item.title}</Heading>
                                </Card.Header>

                                <Card.Body color="fg.muted">
                                    <Text fontSize={"md"}>
                                        {item.description}
                                    </Text>
                                </Card.Body>
                            </Card.Root>
                        ))}
                    </Grid>
                </NavigationMenuContent>
            </NavigationMenuItem>

            <NavigationMenuItem>
                <NavigationMenuTrigger>Handbook</NavigationMenuTrigger>
                <NavigationMenuContent>
                    {" "}
                    <List.Root
                        display={"flex"}
                        flexDirection={"column"}
                        justifyContent={"center"}
                        maxWidth={"400px"}
                        padding={0}
                        margin={0}
                        listStyle={"none"}
                    >
                        {handbookLinks.map((item) => (
                            <Link href={item.href}>
                                <Card.Root
                                    padding={"0.5rem"}
                                    borderRadius={"0.375rm"}
                                    color="inherit"
                                    border={"none"}
                                    backgroundColor={"transparent"}
                                    _hover={{
                                        backgroundColor: "gray.100",
                                    }}
                                    _focusVisible={{
                                        position: "relative",
                                        outline: "2px solid var(blue)",
                                        outlineOffset: "1px",
                                    }}
                                >
                                    <Text
                                        margin={"0 0 4px"}
                                        fontSize={"1rem"}
                                        fontWeight={500}
                                        lineHeight={"1.25rem"}
                                    >
                                        {item.title}
                                    </Text>
                                    <Text
                                        margin={0}
                                        fontSize={"0.875rem"}
                                        lineHeight={"1.25rem"}
                                        color={"gray.500"}
                                    >
                                        {item.description}
                                    </Text>
                                </Card.Root>
                            </Link>
                        ))}
                    </List.Root>
                </NavigationMenuContent>
            </NavigationMenuItem>
        </NavigationMenuList>
    );
};

/*
            <NavigationMenuItem>
                <CustomNavigationMenuLinkStyled to="/">
                    HOME AAA
                </CustomNavigationMenuLinkStyled>
            </NavigationMenuItem>

            <NavigationMenuItem>
                <CustomNavigationMenuLinkStyled to="/about">
                    Custom AAA
                </CustomNavigationMenuLinkStyled>
            </NavigationMenuItem>

*/
