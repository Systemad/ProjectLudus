import { BookmarkSimpleIcon, HeartIcon } from "@phosphor-icons/react";
import {
    Card,
    CardBody,
    CardHeader,
    Image,
    Text,
    IconButton,
    HStack,
    Flex,
    LinkBox,
} from "@yamada-ui/react";
import type { GameDto } from "~/api";
import { getIGDBImageUrl } from "~/features/utilities/ImageHelper";
import { CustomLinkOverlay } from "~/layouts/CustomLink/CustomLinkOverlay";

type Props = {
    item: GameDto;
    height?: "4xs" | "3xs" | "2xs" | "xs" | "sm" | "md" | "lg" | "xl";
    iconSize?: "xs" | "sm" | "md" | "lg";
};

export const HoverGameCard = ({
    item,
    height = "md",
    iconSize = "xs",
}: Props) => {
    const url = getIGDBImageUrl(item.coverImageId, "1080p", false);

    return (
        <Card
            _hover={{ transform: "scale(1.025)" }}
            transition="transform 0.2s cubic-bezier(.4,1,.4,1)"
            as={LinkBox}
            height={height}
            variant="outline"
            breakInside="avoid"
            rounded="xl"
        >
            <CustomLinkOverlay
                to="/games/$gameId"
                params={{ gameId: item.id.toString() }}
            ></CustomLinkOverlay>
            <CardHeader
                as={Image}
                src={url}
                objectFit="cover"
                overflow="hidden"
                p="0"
                roundedTop="xl"
            />

            <CardBody gap="xs">
                <Flex w="100%" align="center" justify="space-between">
                    <Text size="md" isTruncated>
                        {item.name}
                    </Text>
                    <HStack gap={"xs"}>
                        <IconButton
                            colorScheme="primary"
                            variant="primary"
                            size={iconSize}
                            icon={
                                <BookmarkSimpleIcon
                                    size="full"
                                    weight={
                                        item.isWishlisted ? "fill" : "regular"
                                    }
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
                                    weight={item.isHyped ? "fill" : "regular"}
                                    color="var(--ui-colors-red-500)"
                                />
                            }
                        />
                    </HStack>
                </Flex>
            </CardBody>
        </Card>
    );
};
