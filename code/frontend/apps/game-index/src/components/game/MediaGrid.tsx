"use client";

import { AspectRatio, Grid, Image, Text } from "ui";
import { CardSurface } from "../layout/Card";
import type { GameMediaVideo } from "@src/gen/catalogApi";

type Props = {
    sources: string[];
    videos?: GameMediaVideo[];
};

function MediaGrid({ sources, videos = [] }: Props) {
    if (sources.length === 0 && videos.length === 0) {
        return (
            <CardSurface gridColumn={{ base: "span 1", md: "span 2" }} variant="translucent" p={6}>
                <Text color="fg.subtle">No media available.</Text>
            </CardSurface>
        );
    }

    return (
        <CardSurface variant="translucent" p={6}>
            {videos.length > 0 && (
                <Grid
                    templateColumns={{ base: "1fr", md: "1fr 1fr" }}
                    gap={4}
                    mb={sources.length > 0 ? 6 : 0}
                >
                    {videos.map((video) => (
                        <AspectRatio key={video.videoId} ratio={16 / 9}>
                            <iframe
                                src={`https://www.youtube.com/embed/${video.videoId}`}
                                title={video.name ?? "Video"}
                                width="100%"
                                height="100%"
                                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                allowFullScreen
                                style={{ borderRadius: "12px", border: "none" }}
                            />
                        </AspectRatio>
                    ))}
                </Grid>
            )}
            {sources.length > 0 && (
                <Grid templateColumns={{ base: "1fr", md: "1fr 1fr" }} gap={4}>
                    {sources.map((src, index) => (
                        <Image
                            key={`${src}-${index}`}
                            src={src}
                            alt={`Screenshot ${index + 1}`}
                            objectFit="cover"
                            w="full"
                            h="full"
                            borderRadius="xl"
                        />
                    ))}
                </Grid>
            )}
        </CardSurface>
    );
}

export default MediaGrid;
