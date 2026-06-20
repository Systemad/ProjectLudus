import { useId } from "react";
import { FaSteam } from "react-icons/fa";

export const SteamIcon = ({ boxSize = "1.25em" }: { boxSize?: string }) => {
    const id = useId();
    const gradientId = `steam-gradient-${id}`;

    return (
        <span
            style={{
                fontSize: boxSize,
                lineHeight: 1,
                display: "inline-flex",
                alignItems: "center",
            }}
        >
            <svg width="0" height="0" style={{ position: "absolute" }}>
                <linearGradient id={gradientId} x1="0%" y1="0%" x2="100%" y2="100%">
                    <stop stopColor="#6366f1" offset="0%" />
                    <stop stopColor="#7dd3fc" offset="100%" />
                </linearGradient>
            </svg>
            <FaSteam style={{ fill: `url(#${gradientId})` }} />
        </span>
    );
};
