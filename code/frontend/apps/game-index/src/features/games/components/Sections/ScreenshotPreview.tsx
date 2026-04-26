"use client";

import {
    Box,
    Button,
    Text,
    ChevronRightIcon,
    For,
    EmptyState,
    BoxIcon,
    Container,
    Carousel,
    Image,
} from "ui";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

type Props = {
    screenshots: string[];
    visible?: boolean;
    onViewAll: () => void;
};

export function ScreenshotPreview({ screenshots, visible = true, onViewAll }: Props) {
    return (
        <Container.Root
            rounded="2xl"
            variant="surface"
            colorScheme="neutral"
            bg="bg.panel"
            display={visible ? "block" : "none"}
            w="full"
            minW={0}
            border="none"
        >
            <Container.Header>
                <Text
                    fontSize="xl"
                    fontWeight="semibold"
                    textTransform="uppercase"
                    letterSpacing="wide"
                    color="colorScheme.fg"
                >
                    Screenshots
                </Text>
            </Container.Header>

            <Container.Body w="full" overflow="hidden" gap="md">
                <Carousel.Root size="sm" w="full" autoplay={true} delay={5000}>
                    <Carousel.List>
                        <For
                            each={screenshots}
                            limit={3}
                            fallback={
                                <EmptyState.Root
                                    description="No screenshots available"
                                    indicator={<BoxIcon />}
                                />
                            }
                        >
                            {(screenshot, index) => (
                                <Carousel.Item index={index}>
                                    <Image
                                        key={`${screenshot}-${index}`}
                                        src={getIGDBImageUrl(screenshot, "1080p")}
                                        alt={`Screenshot ${index + 1}`}
                                        w="full"
                                        h="auto"
                                    />
                                </Carousel.Item>
                            )}
                        </For>
                    </Carousel.List>
                    <Carousel.PrevTrigger />
                    <Carousel.NextTrigger />

                    <Carousel.Indicators />
                </Carousel.Root>
                <Box textAlign="right">
                    <Button
                        variant="ghost"
                        colorScheme="neutral"
                        size="sm"
                        endIcon={<ChevronRightIcon boxSize="4" />}
                        onClick={onViewAll}
                    >
                        View all
                    </Button>
                </Box>
            </Container.Body>
        </Container.Root>
    );
}
