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
    LinkBox,
} from "@yamada-ui/react";
import { CustomLinkOverlay } from "~/layouts/CustomLink/CustomLinkOverlay";
// https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg

type Props = {
    id: number;
    height?: "4xs" | "3xs" | "2xs" | "xs" | "sm" | "md" | "lg" | "xl";
    iconSize?: "xs" | "sm" | "md" | "lg";
};
export const HoverGameCard = ({
    id,
    height = "md",
    iconSize = "xs",
}: Props) => {
    return (
        <Motion whileHover={{ scale: 1.025 }} whileTap={{ scale: 1 }}>
            <Card
                as={LinkBox}
                height={height}
                variant="outline"
                breakInside="avoid"
                rounded="xl"
            >
                <CustomLinkOverlay
                    to="/games/$gameId"
                    params={{ gameId: id.toString() }}
                ></CustomLinkOverlay>
                <CardHeader
                    as={Image}
                    src="https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg"
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
                                size={iconSize}
                                icon={
                                    <BookmarkSimpleIcon
                                        size="full"
                                        weight="fill"
                                        color="var(--ui-colors-yellow-500)"
                                    />
                                }
                            />
                            <IconButton
                                colorScheme="primary"
                                variant="primary"
                                size={iconSize}
                                icon={
                                    <HeartIcon
                                        size="full"
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
