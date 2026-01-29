import {
    NavigationMenu,
    NavigationMenuList,
    NavigationMenuTrigger,
    NavigationMenuItem,
    NavigationMenuContent,
    NavigationMenuLink,
} from "./nav-menu";

import {
    Box,
    Flex,
    Text,
    MenuIcon,
    Grid,
    Card,
    Heading,
    List,
    Link,
    Accordion,
    Drawer,
    Button,
    useDisclosure,
    IconButton,
    Gamepad2Icon,
    GlobeIcon,
} from "@packages/ui";

type Route = {
    name: string;
    href?: string;
    icon?: string;
    component?: () => React.ReactNode;
};

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
        description: "See what’s new in the latest Base UI versions.",
    },
    {
        href: "/react/overview/about",
        title: "About",
        description: "Learn more about Base UI and our mission.",
    },
] as const;

const handbookLinks = [
    {
        href: "/react/handbook/styling",
        title: "Styling",
        description:
            "Base UI components can be styled with plain CSS, Tailwind CSS, CSS-in-JS, or CSS Modules.",
    },
    {
        href: "/react/handbook/animation",
        title: "Animation",
        description:
            "Base UI components can be animated with CSS transitions, CSS animations, or JavaScript libraries.",
    },
    {
        href: "/react/handbook/composition",
        title: "Composition",
        description:
            "Base UI components can be replaced and composed with your own existing components.",
    },
] as const;

export const NavBar = () => {
    const { open, onOpen, onClose } = useDisclosure();
    return (
        <NavigationMenu>
            <MobileMenu />
        </NavigationMenu>
    );
};
const MobileMenu = () => {
    return (
        <Drawer.Root placement={"inline-start"} size="xs" duration={0.25}>
            <Drawer.OpenTrigger>
                <MenuIcon fontSize={"2rem"} />
            </Drawer.OpenTrigger>

            <Drawer.Content margin="0">
                <Drawer.Header>Game Index</Drawer.Header>

                <Drawer.Body padding={"xs"}>
                    <Box w="full">
                        <Accordion.Root toggle multiple={true}>
                            <Accordion.Item index={0}>
                                <Accordion.Button
                                    fontSize={"xl"}
                                    fontWeight={"semibold"}
                                >
                                    Explore
                                </Accordion.Button>
                                <Text
                                    casing="capitalize"
                                    textTransform={"uppercase"}
                                    fontWeight={"medium"}
                                    fontSize={"sm"}
                                    mb={"sm"}
                                >
                                    Genres
                                </Text>
                                <List.Root gap={"xs"}>
                                    <List.Item
                                        padding={"sm"}
                                        _hover={{
                                            backgroundColor: "bg.muted",
                                            borderRadius: "xl",
                                        }}
                                    >
                                        <List.Icon>
                                            <Gamepad2Icon />
                                        </List.Icon>
                                        RPG
                                    </List.Item>

                                    <List.Item
                                        padding={"sm"}
                                        _hover={{
                                            backgroundColor: "bg.muted",
                                            borderRadius: "xl",
                                        }}
                                    >
                                        <List.Icon>
                                            <GlobeIcon />
                                        </List.Icon>
                                        Travel
                                    </List.Item>
                                </List.Root>
                            </Accordion.Item>

                            <Accordion.Item button="Overview" index={2}>
                                Overview
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
        <NavigationMenu>
            <NavigationMenuList>
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
                                        <Heading size="lg">
                                            {item.title}
                                        </Heading>
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

                <NavigationMenuItem>
                    <NavigationMenuLink href="https://github.com/mui/base-ui">
                        GitHub
                    </NavigationMenuLink>
                </NavigationMenuItem>
            </NavigationMenuList>
        </NavigationMenu>
    );
};
