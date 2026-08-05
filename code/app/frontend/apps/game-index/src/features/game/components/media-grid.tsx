"use client";

import { Grid } from "@astryxdesign/core/Grid";
import { Text } from "@astryxdesign/core/Text";
import { AspectRatio } from "@astryxdesign/core/AspectRatio";
import { useLightbox } from "@astryxdesign/core/Lightbox";
import { HiSquares2X2 } from "react-icons/hi2";
import type { GameMediaVideoDto } from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

type Props = {
    screenshots: string[];
    videos: GameMediaVideoDto[];
};

export function MediaGrid({ screenshots, videos }: Props) {
    const mediaItems = screenshots.map((id, i) => ({
        id,
        src: getIGDBImageUrl(id, "1080p"),
        alt: `Screenshot ${i + 1}`,
        caption: `Screenshot ${i + 1}`,
    }));

    const lightbox = useLightbox({
        media: mediaItems.map(({ src, alt, caption }) => ({ src, alt, caption })),
    });

    return (
        <div>
            {lightbox.element}
            <div style={{marginBottom: "1rem"}}>
                <details open>
                    <summary style={{cursor: "pointer", fontWeight: 600, marginBottom: "0.5rem"}}>Screenshots</summary>
                    <Grid columns={{minWidth: 280}} gap={2}>
                        {screenshots.length > 0 ? (
                            mediaItems.map((item, index) => (
                                <AspectRatio key={item.id} ratio={16 / 9}>
                                    <img
                                        src={item.src}
                                        alt={item.alt}
                                        onClick={() => lightbox.open(index)}
                                        style={{
                                            width: "100%",
                                            height: "100%",
                                            objectFit: "cover",
                                            borderRadius: "var(--radius-element)",
                                            cursor: "pointer",
                                            display: "block",
                                        }}
                                    />
                                </AspectRatio>
                            ))
                        ) : (
                            <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: "1rem", padding: "2rem", color: "var(--fg-muted)" }}>
                                <HiSquares2X2 style={{ width: "2rem", height: "2rem" }} />
                                <Text style={{fontSize: "0.875rem"}}>There are no items to show</Text>
                            </div>
                        )}
                    </Grid>
                </details>
            </div>
            <div>
                <details>
                    <summary style={{cursor: "pointer", fontWeight: 600, marginBottom: "0.5rem"}}>Videos</summary>
                    <Grid columns={{minWidth: 280}} gap={4}>
                        {videos.length > 0 ? (
                            videos.map(({ name, videoId }) => (
                                <AspectRatio key={videoId} ratio={16 / 9}>
                                    <iframe
                                        src={`https://www.youtube.com/embed/${videoId}`}
                                        title={name ?? "Video"}
                                        width="100%"
                                        height="100%"
                                        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                        allowFullScreen
                                        style={{ borderRadius: "var(--radius-element)", border: "none" }}
                                    />
                                </AspectRatio>
                            ))
                        ) : (
                            <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: "1rem", padding: "2rem", color: "var(--fg-muted)" }}>
                                <HiSquares2X2 style={{ width: "2rem", height: "2rem" }} />
                                <Text style={{fontSize: "0.875rem"}}>There are no items to show</Text>
                            </div>
                        )}
                    </Grid>
                </details>
            </div>
        </div>
    );
}
