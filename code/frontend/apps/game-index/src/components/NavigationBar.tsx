import {
    Box,
    Button,
    Flex,
    Heading,
    HStack,
    Input,
    InputGroup,
    MoonIcon,
    SearchIcon,
    SettingsIcon,
    Text,
} from "ui";
import { RouterLink, RouterLinkButton } from "./YamadaLink";

type NavigationBarProps = {
    active?: string;
};

const linkStyle = { color: "inherit", textDecoration: "none" };

const navItems = [
    { id: "home" as const, label: "Home", to: "/" },
    { id: "search" as const, label: "Explore", to: "/search" },
];

export function NavigationBar({ active: _active = "home" }: NavigationBarProps) {
    return (
        <Flex
            as="nav"
            position="fixed"
            insetX="0"
            top="0"
            zIndex="freeza"
            borderBottomWidth="1px"
            borderColor="border.subtle"
            bg="bg.float"
            backdropBlur="2xl"
            boxShadow="0 2xl 3xl rgba(0, 0, 0, 0.28)"
        >
            <Flex
                w="full"
                maxW="7xl"
                mx="auto"
                px={{ base: "4", md: "6", xl: "8" }}
                py="4"
                gap="6"
                align="center"
            >
                <Flex align="center" gap={{ base: "4", md: "10" }} flex="1">
                    <RouterLink to="/" style={linkStyle}>
                        <Heading
                            as="span"
                            fontFamily="heading"
                            fontSize={{ base: "xl", md: "2xl" }}
                            fontWeight="black"
                            letterSpacing="tight"
                            textTransform="uppercase"
                            color="colorScheme.solid"
                        >
                            GAMEX
                        </Heading>
                    </RouterLink>

                    <HStack display={{ base: "none", md: "flex" }} gap="8">
                        {navItems.map((item) => (
                            <RouterLinkButton
                                key={item.id}
                                to={item.to}
                                variant="ghost"
                                rounded="2xl"
                                px={{ base: "2", md: "3" }}
                                py="1"
                                minW="0"
                                h="auto"
                                activeProps={{ color: "fg.base" }}
                                _hover={{ bg: "bg.subtle", color: "fg.base" }}
                            >
                                <Text
                                    as="span"
                                    fontFamily="heading"
                                    fontWeight="bold"
                                    letterSpacing="tight"
                                    color="inherit"
                                    transitionProperty="color"
                                    transitionDuration="moderate"
                                >
                                    {item.label}
                                </Text>
                            </RouterLinkButton>
                        ))}
                        <RouterLinkButton
                            to="/search"
                            variant="ghost"
                            rounded="2xl"
                            px={{ base: "2", md: "3" }}
                            py="1"
                            minW="0"
                            h="auto"
                            activeProps={{ color: "fg.base" }}
                            _hover={{ bg: "bg.subtle", color: "fg.base" }}
                        >
                            <Text
                                as="span"
                                fontFamily="heading"
                                fontWeight="bold"
                                letterSpacing="tight"
                                color="inherit"
                                transitionProperty="color"
                                transitionDuration="moderate"
                            >
                                Search
                            </Text>
                        </RouterLinkButton>
                    </HStack>
                </Flex>

                <Box display={{ base: "none", lg: "block" }} position="relative" flex="1">
                    <InputGroup.Root>
                        <InputGroup.Element>
                            <SearchIcon
                                position="absolute"
                                insetInlineStart="4"
                                top="50%"
                                transform="translateY(-50%)"
                                color="fg.subtle"
                                pointerEvents="none"
                            />
                        </InputGroup.Element>
                        <Input
                            variant="filled"
                            rounded="xl"
                            placeholder="Search games and companies..."
                        />
                    </InputGroup.Root>
                </Box>

                <HStack gap="3">
                    <Button
                        aria-label="Theme control"
                        variant="ghost"
                        rounded="full"
                        minW="10"
                        h="10"
                        px="0"
                        borderWidth="1px"
                        borderColor="border.base"
                        bg="bg.subtle"
                        color="fg.muted"
                        _hover={{ bg: "bg.subtle", color: "fg.base" }}
                    >
                        <MoonIcon boxSize="4" />
                    </Button>
                    <Button
                        aria-label="Settings"
                        variant="ghost"
                        rounded="full"
                        minW="10"
                        h="10"
                        px="0"
                        borderWidth="1px"
                        borderColor="border.base"
                        bg="bg.subtle"
                        color="fg.muted"
                        _hover={{ bg: "bg.subtle", color: "fg.base" }}
                    >
                        <SettingsIcon boxSize="4" />
                    </Button>
                </HStack>
            </Flex>
        </Flex>
    );
}

export default NavigationBar;
