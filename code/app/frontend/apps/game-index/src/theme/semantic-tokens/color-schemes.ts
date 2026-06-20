import { defineSemanticTokens } from "ui";

export const colorSchemes = defineSemanticTokens.colorSchemes({
    danger: "red",
    error: "red",
    info: "blue",
    link: "blue",
    mono: ["neutral", "white"],
    primary: ["neutral", "white"],
    secondary: "gray",
    success: "green",
    warning: "orange",
});
