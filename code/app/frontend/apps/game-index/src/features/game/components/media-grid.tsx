"use client";

import { Grid } from "@astryxdesign/core/Grid";
import { Text } from "@astryxdesign/core/Text";
import { AspectRatio } from "@astryxdesign/core/AspectRatio";
import { HiSquares2X2 } from "react-icons/hi2";
import type { GameMediaVideoDto } from "@src/gen/catalogApi";
import { HoverImage } from "@src/components/hover-image";

type Props = {
    screenshots: string[];
    videos: GameMediaVideoDto[];
};

export function MediaGrid({ screenshots, videos }: Props) {
    return (
        <div>
            <div style={{marginBottom: "1rem"}}>
                <details open>
                    <summary style={{cursor: "pointer", fontWeight: 600, marginBottom: "0.5rem"}}>Screenshots</summary>
                    <Grid columns={{minWidth: 280}} gap={4}>
                        {screenshots.length > 0 ? (
                            screenshots.map((screenshot, index) => (
                                <HoverImage
                                    key={screenshot}
                                    src={screenshot}
                                    size="1080p"
                                    alt={`Screenshot ${index + 1}`}
                                />
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
                                        style={{ borderRadius: "12px", border: "none" }}
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
