import z from "zod";

export const parseArraySearchParam = z.preprocess((value) => {
    if (typeof value === "string") {
        return value
            .split(",")
            .map((item) => item.trim())
            .filter(Boolean);
    }

    if (Array.isArray(value)) {
        return value.filter((item): item is string => typeof item === "string");
    }

    return [];
}, z.array(z.string()));
