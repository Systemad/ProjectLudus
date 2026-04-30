import { Badge, Box, Heading, Text, VStack, Wrap, Flex } from "ui";
import { EU } from "country-flag-icons/react/3x2";

export function Footer() {
    return (
        <Box as="footer" mt="20" bg="bg.base">
            <Box
                maxW="7xl"
                mx="auto"
                px={{ base: "4", md: "6", xl: "8" }}
                py={{ base: "5", md: "7" }}
                bg="bg.base"
            >
                <VStack align="center" gap="3" textAlign="center">
                    <Heading
                        as="h2"
                        size="lg"
                        fontFamily="heading"
                        bgClip="text"
                        bgGradient="linear(to-l, #C6426E, #642B73)"
                    >
                        GAME-INDEX
                    </Heading>
                    <Text color="fg.muted" maxW="md" fontSize="xs">
                        game-index.app is a fan-made website and is not affiliated with IGDB. <br />
                        All the logos, images, trademarks and creatives are property of their
                        respective owners.
                    </Text>
                    <Wrap gap="2" justify="center">
                        <Badge
                            rounded="full"
                            px="sm"
                            py="xs"
                            colorScheme={"blue"}
                            variant="surface"
                        >
                            <Flex align="center" gap="sm">
                                <EU title="EU" style={{ width: "1.2em", height: "auto" }} />
                                Made in EU
                            </Flex>
                        </Badge>
                    </Wrap>
                </VStack>
            </Box>
        </Box>
    );
}
