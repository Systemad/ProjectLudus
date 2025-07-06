import { BookmarkSimpleIcon, HeartIcon } from "@phosphor-icons/react";
import { Link as TanstackLink } from "@tanstack/react-router";
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
    LinkOverlay,
} from "@yamada-ui/react";
import { CustomLinkOverlay } from "~/layouts/CustomLink/CustomLinkOverlay";
// https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg

/*
                <LinkOverlay
                    as={TanstackLink}
                    to="/games/$gameId"
                    params={{ gameId: "a" }}
                >
                    <TanstackLink
                        to="/games/$gameId"
                        params={{ gameId: "a" }}
                    ></TanstackLink>
                </LinkOverlay>
*/
type Props = {
    key: number;
};
export const HoverGameCard = ({ key }: Props) => {
    return (
        <Motion whileHover={{ scale: 1.025 }}>
            <Card
                as={LinkBox}
                h="md"
                variant="outline"
                breakInside="avoid"
                rounded="xl"
            >
                <CustomLinkOverlay
                    to="/games/$gameId"
                    params={{ gameId: "10" }}
                ></CustomLinkOverlay>
                <CardHeader
                    as={Image}
                    src="https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg"
                    objectFit="cover"
                    overflow="hidden"
                    p="0"
                    roundedTop="xl"
                ></CardHeader>

                <CardBody gap="xs">
                    <Flex w="100%" align="center" justify="space-between">
                        <Heading as="h3" size="md">
                            Yamada UI
                        </Heading>
                        <HStack gap={"xs"}>
                            <IconButton
                                colorScheme="primary"
                                variant="primary"
                                size="xs"
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
                                size="xs"
                                onClick={(e) => e.stopPropagation()}
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
