import { HoverCard } from "@astryxdesign/core/HoverCard";
import { getIGDBImageUrl, type ImageSize } from "@src/utils/ImageHelper";

type Props = {
    src: string;
    size?: ImageSize;
    alt: string;
    fallback?: string;
};

export function HoverImage({ src, size = "thumb", alt }: Props) {
    const thumbUrl = getIGDBImageUrl(src, size);
    const largeUrl = getIGDBImageUrl(src, "1080p");
    return (
        <HoverCard
            placement="above"
            content={
                <img
                    src={largeUrl}
                    alt={alt}
                    style={{
                        maxWidth: 400,
                        borderRadius: "var(--radius-container)",
                        display: "block",
                    }}
                />
            }
        >
            <div style={{ width: "100%", height: "100%" }}>
                <img
                    src={thumbUrl}
                    alt={alt}
                    style={{
                        width: "100%",
                        height: "100%",
                        objectFit: "cover",
                        display: "block",
                    }}
                />
            </div>
        </HoverCard>
    );
}
