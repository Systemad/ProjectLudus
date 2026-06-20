const HUE_RANGES: [string, number, number][] = [
    ["red", 340, 360],
    ["red", 0, 15],
    ["orange", 15, 35],
    ["amber", 35, 50],
    ["yellow", 50, 65],
    ["lime", 65, 85],
    ["green", 85, 145],
    ["emerald", 145, 165],
    ["teal", 165, 190],
    ["cyan", 190, 210],
    ["blue", 210, 245],
    ["indigo", 245, 265],
    ["violet", 265, 285],
    ["purple", 285, 310],
    ["fuchsia", 310, 330],
    ["pink", 330, 345],
    ["rose", 345, 360],
];

function parseHsl(hsl: string): { h: number; s: number; l: number } | null {
    const m = hsl.match(/hsl\((\d+),\s*(\d+)%,\s*(\d+)%\)/);
    if (!m) return null;
    return { h: +m[1], s: +m[2], l: +m[3] };
}

export function hslToColorScheme(hsl: string | null, fallback = "blue"): string {
    if (!hsl) return fallback;
    const parsed = parseHsl(hsl);
    if (!parsed || parsed.s < 10) return fallback;
    for (const [name, min, max] of HUE_RANGES) {
        if (parsed.h >= min && parsed.h < max) return name;
    }
    return fallback;
}
