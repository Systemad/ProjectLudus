import { Link } from "@tanstack/react-router";
import { Badge, Box, Heading, SimpleGrid, Text, VStack, Wrap } from "ui";

const linkStyle = { color: "inherit", textDecoration: "none" };

export function Footer() {
    return (
        <Box borderTopWidth="1px" borderColor="border.subtle" mt="20" bg="bg.panel">
            <Box
                maxW="7xl"
                mx="auto"
                px={{ base: "4", md: "6", xl: "8" }}
                py={{ base: "10", md: "14" }}
            >
                <SimpleGrid columns={{ base: 1, md: 3 }} gap="8">
                    <VStack align="stretch" gap="3">
                        <Heading as="h2" size="md" fontFamily="heading">
                            GAME-INDEX
                        </Heading>
                        <Text color="fg.muted" maxW="md">
                            Data-centric gaming discovery with a cinematic editorial surface.
                        </Text>
                        <Wrap gap="2">
                            <Badge rounded="full" px="3" py="1" bg="bg.subtle" color="fg.base">
                                Yamada UI
                            </Badge>
                            <Badge
                                rounded="full"
                                px="3"
                                py="1"
                                bg="colorScheme.bg"
                                color="colorScheme.fg"
                            >
                                TanStack Router
                            </Badge>
                        </Wrap>
                    </VStack>

                    <VStack align="stretch" gap="3">
                        <Text
                            fontFamily="heading"
                            fontWeight="bold"
                            textTransform="uppercase"
                            letterSpacing="widest"
                            color="fg.muted"
                            fontSize="xs"
                        >
                            Browse
                        </Text>
                        <Link to="/" style={linkStyle}>
                            <Text _hover={{ color: "fg.base" }} color="fg.muted">
                                Home
                            </Text>
                        </Link>
                        <Link to="/search" style={linkStyle}>
                            <Text _hover={{ color: "fg.base" }} color="fg.muted">
                                Search
                            </Text>
                        </Link>
                        <Link
                            to="/games/$gameId"
                            params={{ gameId: "hollow-knight" }}
                            style={linkStyle}
                        >
                            <Text _hover={{ color: "fg.base" }} color="fg.muted">
                                Featured game
                            </Text>
                        </Link>
                    </VStack>

                    <VStack align="stretch" gap="3">
                        <Text
                            fontFamily="heading"
                            fontWeight="bold"
                            textTransform="uppercase"
                            letterSpacing="widest"
                            color="fg.muted"
                            fontSize="xs"
                        >
                            Signals
                        </Text>
                        <Text color="fg.muted">Trending velocity</Text>
                        <Text color="fg.muted">Cross-platform heat</Text>
                        <Text color="fg.muted">Editorial picks</Text>
                    </VStack>
                </SimpleGrid>
            </Box>
        </Box>
    );
}

export default Footer;
