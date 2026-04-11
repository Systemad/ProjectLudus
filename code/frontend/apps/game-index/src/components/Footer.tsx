import { Badge, Box, Heading, Text, VStack, Wrap, Flex } from "ui";
import { EU } from "country-flag-icons/react/3x2";
export function Footer() {
    return (
        <Box borderTopWidth="1px" borderColor="border.subtle" mt="20" bg="bg.panel">
            <Box
                maxW="7xl"
                mx="auto"
                px={{ base: "4", md: "6", xl: "8" }}
                py={{ base: "5", md: "7" }}
            >
                <VStack align="center" gap="3" textAlign="center">
                    <Heading as="h2" size="md" fontFamily="heading">
                        GAME-INDEX
                    </Heading>
                    <Text color="fg.muted" maxW="md" fontSize="xs">
                        game-index.app is a fan-made website and is not affiliated with IGDB. <br />
                        All the logos, images, trademarks and creatives are property of their
                        respective owners.
                    </Text>
                    <Wrap gap="2" justify="center">
                        <Badge rounded="full" px="3" py="1" bg="bg.subtle" color="fg.base">
                            <Flex align="center" gap="1.5">
                                <EU title="EU" style={{ width: "1.2em", height: "auto" }} />
                                Made in EU
                            </Flex>
                        </Badge>
                        <Badge
                            rounded="full"
                            px="3"
                            py="1"
                            bg="colorScheme.bg"
                            color="colorScheme.fg"
                        >
                            Github
                        </Badge>
                    </Wrap>
                </VStack>
            </Box>
        </Box>
    );
}

export default Footer;
