const gradientCache: Record<string, string> = {};
const shadowCache: Record<string, string> = {};

const hashString = (value: string): number => {
    let hash = 0;
    for (let index = 0; index < value.length; index += 1) {
        hash = (hash << 5) - hash + value.charCodeAt(index);
        hash |= 0;
    }

    return Math.abs(hash);
};

export const getCachedCardStyles = (
    imageUrl: string
): { gradient: string; shadow: string } => {
    const cacheKey = imageUrl || "__default";
    const cachedGradient = gradientCache[cacheKey];
    const cachedShadow = shadowCache[cacheKey];

    if (cachedGradient && cachedShadow) {
        return {
            gradient: cachedGradient,
            shadow: cachedShadow,
        };
    }

    const hash = hashString(cacheKey);
    const hue = hash % 360;
    const accent = `hsla(${hue}, 55%, 46%, 0.45)`;
    const accentSoft = `hsla(${(hue + 24) % 360}, 45%, 34%, 0.4)`;

    const gradient = `
        radial-gradient(circle at 20% 16%, ${accent} 0%, transparent 45%),
        radial-gradient(circle at 82% 88%, ${accentSoft} 0%, transparent 55%),
        linear-gradient(180deg, rgba(24, 28, 40, 0.95), rgba(10, 12, 18, 0.98))
    `;

    const shadow = `0 18px 40px hsla(${hue}, 65%, 16%, 0.35)`;

    gradientCache[cacheKey] = gradient;
    shadowCache[cacheKey] = shadow;

    return { gradient, shadow };
};
