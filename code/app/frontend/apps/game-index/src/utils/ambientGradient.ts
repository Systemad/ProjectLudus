export function toSubtleRgba(color: string | null | undefined, alpha: number, fallback: string) {
    if (!color) return fallback;

    const normalized = color.trim();
    const hexMatch = normalized.match(/^#([\da-f]{3}|[\da-f]{6})$/i);

    if (hexMatch) {
        const hex = hexMatch[1];
        const expanded =
            hex.length === 3
                ? hex
                      .split("")
                      .map((char) => `${char}${char}`)
                      .join("")
                : hex;

        const red = Number.parseInt(expanded.slice(0, 2), 16);
        const green = Number.parseInt(expanded.slice(2, 4), 16);
        const blue = Number.parseInt(expanded.slice(4, 6), 16);

        return `rgba(${red}, ${green}, ${blue}, ${alpha})`;
    }

    const rgbMatch = normalized.match(/^rgba?\(([^)]+)\)$/i);
    if (rgbMatch) {
        const channels = rgbMatch[1]
            .split(",")
            .slice(0, 3)
            .map((value) => value.trim());

        if (channels.length === 3) {
            return `rgba(${channels[0]}, ${channels[1]}, ${channels[2]}, ${alpha})`;
        }
    }

    return fallback;
}

export function buildAmbientGradient(colors: {
    dominantColor?: string | null;
    darkerColor?: string | null;
    lighterColor?: string | null;
}) {
    const ambientStart = toSubtleRgba(
        colors.darkerColor ?? colors.dominantColor,
        0.09,
        "rgba(255, 255, 255, 0.045)",
    );
    const ambientMiddle = toSubtleRgba(colors.dominantColor, 0.06, "rgba(255, 255, 255, 0.035)");
    const ambientEnd = toSubtleRgba(
        colors.lighterColor ?? colors.dominantColor,
        0.045,
        "rgba(255, 255, 255, 0.028)",
    );

    return (
        `radial-gradient(1050px 560px at 12% 8%, ${ambientStart} 0%, transparent 65%), ` +
        `radial-gradient(980px 520px at 88% 18%, ${ambientEnd} 0%, transparent 62%), ` +
        `radial-gradient(1200px 700px at 50% 52%, ${ambientMiddle} 0%, transparent 70%), ` +
        `radial-gradient(1000px 620px at 20% 92%, ${ambientEnd} 0%, transparent 68%), ` +
        `radial-gradient(1100px 640px at 82% 88%, ${ambientStart} 0%, transparent 70%), ` +
        `linear-gradient(180deg, ${ambientMiddle} 0%, rgba(0, 0, 0, 0) 100%)`
    );
}
