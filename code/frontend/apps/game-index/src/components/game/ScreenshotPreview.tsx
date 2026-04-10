"use client";

import { Box, Button, Grid, Text, ChevronRightIcon } from "ui";
import { CardSurface } from "@src/components/layout/Card";
import { sectionLabelStyle } from "@src/utils/sectionTextStyles";
import HoverImage from "../layout/HoverImage";

type Props = {
    sources: string[];
    visible?: boolean;
    onViewAll: () => void;
};

export function ScreenshotPreview({ sources, visible = true, onViewAll }: Props) {
    return (
        <CardSurface p={6} display={visible ? "block" : "none"}>
            <Text {...sectionLabelStyle} mb={4}>
                Screenshots
            </Text>

            {sources.length > 0 ? (
                <Grid
                    templateColumns={{ base: "1fr 1fr", md: "repeat(3, minmax(0,1fr))" }}
                    gap={3}
                    mb={4}
                >
                    {sources.slice(0, 3).map((src, index) => (
                        <HoverImage
                            key={`${src}-${index}`}
                            src={src}
                            alt={`Screenshot ${index + 1}`}
                        />
                    ))}
                </Grid>
            ) : (
                <Text color="fg.subtle" mb={4}>
                    No screenshot preview available.
                </Text>
            )}

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
