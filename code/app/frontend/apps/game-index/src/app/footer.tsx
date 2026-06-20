import { Tag, Box, VStack, Text } from "ui";
import { EU } from "country-flag-icons/react/3x2";

export function Footer() {
    return (
        <Box as="footer" marginTop={10} bgGradient="linear(to-b, transparent, neutral.800/20)">
            <Box
                maxWidth="1128px"
                mx="auto"
                paddingX={{ base: 4, md: 6 }}
                paddingY={{ base: 3, md: 4 }}
            >
                <VStack align="center" gap={2} textAlign="center">
                    <Text
                        as="h2"
                        fontSize="lg"
                        fontWeight="bold"
                        style={{
                            background: "linear-gradient(to left, #C6426E, #642B73)",
                            WebkitBackgroundClip: "text",
                            WebkitTextFillColor: "transparent",
                        }}
                    >
                        GAME-INDEX
                    </Text>
                    <Text color="fg.muted" maxWidth="448px" fontSize="xs">
                        game-index.app is a fan-made website and is not affiliated with IGDB. <br />
                        All the logos, images, trademarks and creatives are property of their
                        respective owners.
                    </Text>
                    <Tag borderRadius={9999} paddingX={3} paddingY={1} size="sm">
                        <Box display="flex" alignContent="center" gap={1}>
                            <EU style={{ width: "1em", height: "auto" }} />
                            Made in EU
                        </Box>
                    </Tag>
                </VStack>
            </Box>
        </Box>
    );
}
