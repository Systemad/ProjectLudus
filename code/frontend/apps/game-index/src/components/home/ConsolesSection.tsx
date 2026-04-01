import { Box, Flex, Heading, SimpleGrid, Text, VStack } from "ui";

function ControllerSvg() {
    return (
        <svg width="40" height="40" viewBox="0 0 40 40" fill="none" aria-hidden="true">
            <rect
                x="2"
                y="9"
                width="36"
                height="23"
                rx="11.5"
                stroke="white"
                strokeWidth="2"
                fill="rgba(255,255,255,0.12)"
            />
            <rect x="8" y="18.5" width="7" height="2.5" rx="1.25" fill="white" />
            <rect x="10.75" y="15.75" width="2.5" height="8" rx="1.25" fill="white" />
            <circle cx="29" cy="17.5" r="2" fill="white" />
            <circle cx="33.5" cy="21" r="2" fill="white" />
            <circle cx="29" cy="24.5" r="2" fill="white" />
            <circle cx="24.5" cy="21" r="2" fill="white" />
        </svg>
    );
}

function XboxButtonsSvg() {
    return (
        <svg width="40" height="40" viewBox="0 0 40 40" fill="none" aria-hidden="true">
            <circle cx="20" cy="11" r="5.5" fill="rgba(255,255,255,0.9)" />
            <circle cx="29" cy="20" r="5.5" fill="rgba(255,255,255,0.75)" />
            <circle cx="20" cy="29" r="5.5" fill="rgba(255,255,255,0.6)" />
            <circle cx="11" cy="20" r="5.5" fill="rgba(255,255,255,0.45)" />
        </svg>
    );
}

function DPadSvg() {
    return (
        <svg width="40" height="40" viewBox="0 0 40 40" fill="none" aria-hidden="true">
            <rect x="16" y="2" width="8" height="36" rx="3" fill="white" />
            <rect x="2" y="16" width="36" height="8" rx="3" fill="white" />
        </svg>
    );
}

function KeyboardMouseSvg() {
    return (
        <svg width="48" height="36" viewBox="0 0 48 36" fill="none" aria-hidden="true">
            <rect
                x="0.5"
                y="1.5"
                width="33"
                height="21"
                rx="2.5"
                stroke="currentColor"
                strokeWidth="1.5"
            />
            {[4, 9, 14, 19, 24, 28.5].map((x) => (
                <rect
                    key={x}
                    x={x}
                    y="5.5"
                    width="3"
                    height="3"
                    rx="0.75"
                    fill="currentColor"
                    opacity="0.55"
                />
            ))}
            {[4, 9, 14, 19, 24].map((x) => (
                <rect
                    key={x}
                    x={x}
                    y="11"
                    width="3"
                    height="3"
                    rx="0.75"
                    fill="currentColor"
                    opacity="0.55"
                />
            ))}
            <rect x="8" y="18" width="18" height="2.5" rx="1" fill="currentColor" opacity="0.55" />
            <rect
                x="37.5"
                y="0.5"
                width="9"
                height="15"
                rx="4.5"
                stroke="currentColor"
                strokeWidth="1.5"
            />
            <line x1="42" y1="1" x2="42" y2="8" stroke="currentColor" strokeWidth="1.5" />
            <circle cx="42" cy="5" r="1.25" fill="currentColor" opacity="0.5" />
        </svg>
    );
}

const consolePlatforms = [
    {
        id: "playstation",
        label: "PlayStation",
        sublabel: "EXPLORE PS5 TITLES",
        bg: "#003087",
        color: "white",
        icon: <ControllerSvg />,
    },
    {
        id: "xbox",
        label: "Xbox",
        sublabel: "GAME PASS INCLUDED",
        bg: "#107C10",
        color: "white",
        icon: <XboxButtonsSvg />,
    },
    {
        id: "nintendo",
        label: "Nintendo Switch",
        sublabel: "HANDHELD FAVORITES",
        bg: "#E60012",
        color: "white",
        icon: <DPadSvg />,
    },
    {
        id: "pc",
        label: "PC",
        sublabel: "ULTIMATE PERFORMANCE",
        bg: "#EBEBEB",
        color: "#111",
        icon: <KeyboardMouseSvg />,
    },
];

export function ConsolesSection() {
    return (
        <VStack align="stretch" gap="8">
            <Heading fontFamily="heading" fontSize={{ base: "3xl", md: "4xl" }}>
                Browse by Console
            </Heading>
            <SimpleGrid columns={{ base: 2, md: 4 }} gap="4">
                {consolePlatforms.map((c) => (
                    <Flex
                        key={c.id}
                        direction="column"
                        justify="space-between"
                        rounded="2xl"
                        bg={c.bg}
                        color={c.color}
                        p={{ base: "5", md: "6" }}
                        minH={{ base: "36", md: "44" }}
                        cursor="pointer"
                        transitionProperty="opacity"
                        transitionDuration="moderate"
                        _hover={{ opacity: 0.9 }}
                    >
                        <Box>{c.icon}</Box>
                        <VStack align="start" gap="1">
                            <Text
                                fontFamily="heading"
                                fontWeight="black"
                                fontSize={{ base: "md", md: "xl" }}
                                letterSpacing="tight"
                            >
                                {c.label}
                            </Text>
                            <Text
                                fontSize="xs"
                                letterSpacing="widest"
                                fontWeight="bold"
                                opacity={0.65}
                            >
                                {c.sublabel}
                            </Text>
                        </VStack>
                    </Flex>
                ))}
            </SimpleGrid>
        </VStack>
    );
}
