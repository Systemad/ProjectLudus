"use client";

import { AspectRatio, Grid, Image, Accordion, For, EmptyState, BoxIcon } from "ui";
import type { GameMediaVideoDto } from "@src/gen/catalogApi";

type Props = {
    screenshots: string[];
    videos: GameMediaVideoDto[];
};

function MediaGrid({ screenshots, videos }: Props) {
    return (
        <Accordion.Root defaultIndex={0} multiple toggle>
            <Accordion.Item index={0} button="Screenshots">
                <Grid templateColumns={{ base: "1fr", md: "1fr 1fr" }} gap={4}>
                    <For
                        each={screenshots}
                        fallback={
                            <EmptyState.Root
                                description="There are no items to show"
                                indicator={<BoxIcon />}
                            />
                        }
                    >
                        {(screenshot, index) => {
                            <Image
                                key={`${screenshot}-${index}`}
                                src={screenshot}
                                alt={`Screenshot ${index + 1}`}
                                objectFit="cover"
                                w="full"
                                h="full"
                                borderRadius="xl"
                            />;
                        }}
                    </For>
                </Grid>
            </Accordion.Item>
            <Accordion.Item index={1} button="Videos">
                <Grid templateColumns={{ base: "1fr", md: "1fr 1fr" }} gap={4}>
                    <For
                        each={videos}
                        fallback={
                            <EmptyState.Root
                                description="There are no items to show"
                                indicator={<BoxIcon />}
                            />
                        }
                    >
                        {({ name, videoId }, index) => {
                            <AspectRatio key={`${name}-${index}`} ratio={16 / 9}>
                                <iframe
                                    src={`https://www.youtube.com/embed/${videoId}`}
                                    title={name ?? "Video"}
                                    width="100%"
                                    height="100%"
                                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                    allowFullScreen
                                    style={{ borderRadius: "12px", border: "none" }}
                                />
                            </AspectRatio>;
                        }}
                    </For>
                </Grid>
            </Accordion.Item>
        </Accordion.Root>
    );
}

export default MediaGrid;
