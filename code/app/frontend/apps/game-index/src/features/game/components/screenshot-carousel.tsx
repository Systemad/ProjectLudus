"use client";

import { Section } from "@astryxdesign/core/Section";
import { Carousel } from "@astryxdesign/core/Carousel";
import { Button } from "@astryxdesign/core/Button";
import { Text } from "@astryxdesign/core/Text";
import { LuChevronRight } from "react-icons/lu";
import { HiSquares2X2 } from "react-icons/hi2";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

type Props = {
    screenshots: string[];
    visible?: boolean;
    onViewAll: () => void;
};

export function ScreenshotPreview({ screenshots, visible = true, onViewAll }: Props) {
    return (
        <Section
            variant="muted"
            padding={4}
            style={{
                display: visible ? "block" : "none",
                width: "100%",
                minWidth: 0,
                borderRadius: "var(--radius-2xl)",
                background: "var(--bg-panel)",
                border: "none",
            }}
        >
            <div style={{ marginBottom: "1rem" }}>
                <Text style={{fontSize: "1.25rem", fontWeight: 600, textTransform: "uppercase", letterSpacing: "0.05em"}}>
                    Screenshots
                </Text>
            </div>

            <div style={{ width: "100%", overflow: "hidden" }}>
                {screenshots.length > 0 ? (
                    <Carousel hasButtons hasSnap gap={2}>
                        {screenshots.slice(0, 3).map((screenshot, index) => (
                            <img
                                key={screenshot}
                                src={getIGDBImageUrl(screenshot, "1080p")}
                                alt={`Screenshot ${index + 1}`}
                                style={{ width: "100%", height: "auto", flexShrink: 0, scrollSnapAlign: "start" }}
                            />
                        ))}
                    </Carousel>
                ) : (
                    <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: "1rem", padding: "2rem", color: "var(--fg-muted)" }}>
                        <HiSquares2X2 style={{ width: "2rem", height: "2rem" }} />
                        <Text style={{fontSize: "0.875rem"}}>No screenshots available</Text>
                    </div>
                )}
                <div style={{ textAlign: "right", marginTop: "0.5rem" }}>
                    <Button
                        variant="ghost"
                        size="sm"
                        label="View all"
                        endContent={<LuChevronRight style={{ width: "1rem", height: "1rem" }} />}
                        onClick={onViewAll}
                    />
                </div>
            </div>
        </Section>
    );
}
