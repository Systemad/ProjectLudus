import { BookmarkSimpleIcon, HeartIcon } from "@phosphor-icons/react";
import {
    Card,
    CardBody,
    CardHeader,
    Heading,
    Image,
    Motion,
    IconButton,
    HStack,
    Flex,
} from "@yamada-ui/react";
// https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg

export const HoverGameCard = () => {
    return (
        <Motion whileHover={{ scale: 1.025 }}>
            <Card variant="outline" breakInside="avoid" h="xl" rounded="xl">
                <CardHeader
                    as={Image}
                    src="https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg"
                    h="md"
                    objectFit="cover"
                    overflow="hidden"
                    p="0"
                    roundedTop="xl"
                />

                <CardBody gap="xs">
                    <Flex w="100%" align="center" justify="space-between">
                        <Heading as="h3" size="md">
                            Yamada UI
                        </Heading>
                        <HStack gap={"xs"}>
                            <IconButton
                                colorScheme="primary"
                                variant="primary"
                                icon={
                                    <BookmarkSimpleIcon
                                        size={24}
                                        weight="fill"
                                        color="var(--ui-colors-warning-500)"
                                    />
                                }
                            />
                            <IconButton
                                colorScheme="primary"
                                variant="primary"
                                icon={
                                    <HeartIcon
                                        size={24}
                                        weight="fill"
                                        color="var(--ui-colors-red-500)"
                                    />
                                }
                            />
                        </HStack>
                    </Flex>
                </CardBody>
            </Card>
        </Motion>
    );
};

HoverGameCard.displayName = "HoverGameCard";
