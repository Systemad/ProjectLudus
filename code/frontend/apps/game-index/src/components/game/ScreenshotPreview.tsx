"use client";

import { Box, Button, Grid, Text, ChevronRightIcon, For, EmptyState, BoxIcon } from "ui";
import { CardSurface } from "@src/components/layout/Card";
import { sectionLabelStyle } from "@src/utils/sectionTextStyles";
import HoverImage from "../layout/HoverImage";

type Props = {
    screenshots: string[];
    visible?: boolean;
    onViewAll: () => void;
};

export function ScreenshotPreview({ screenshots, visible = true, onViewAll }: Props) {
    return (
        <CardSurface p={6} display={visible ? "block" : "none"}>
            <Text {...sectionLabelStyle} mb={4}>
                Screenshots
            </Text>

            <Grid
                templateColumns={{ base: "1fr 1fr", md: "repeat(3, minmax(0,1fr))" }}
                gap={3}
                mb={4}
            >
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
                    {(screenshot, index) => {
                        <HoverImage
                            key={`${screenshot}-${index}`}
                            src={screenshot}
                            alt={`Screenshot ${index + 1}`}
                        />;
                    }}
                </For>
            </Grid>
            <Box textAlign="right">
                <Button
                    variant="ghost"
                    colorScheme="gray"
                    size="sm"
                    endIcon={<ChevronRightIcon boxSize="4" />}
                    onClick={onViewAll}
                >
                    View all
                </Button>
            </Box>
        </CardSurface>
    );
}
