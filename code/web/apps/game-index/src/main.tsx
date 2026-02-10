import { StrictMode } from "react";
import ReactDOM from "react-dom/client";
import { UIProvider, defineConfig, extendTheme } from "@packages/ui";
// import { my_theme } from "@packages/theme";
import { RouterProvider, createRouter } from "@tanstack/react-router";
// Import the generated route tree
import { routeTree } from "./routeTree.gen";
import "./styles.css";
// Create a new router instance

/*
createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <UIProvider>
            <App />
        </UIProvider>
    </StrictMode>
);
*/

const extendedTheme = extendTheme({
    styles: {
        globalStyle: {
            body: {
                "--root-header-height": "sizes.14",
                "--space": { base: "spaces.lg", md: "spaces.md" },
                //scrollbarGutter: "stable",
                "--sticky-offset": {
                    base: "calc({sizes.14} + {sizes.13} + {spaces.lg})",
                    md: "{sizes.14}",
                },
                colorScheme: "emerald",
            },
            html: {
                scrollBehavior: "smooth",
                //scrollbarGutter: "stable",
            },
        },
    },
});
export const config = defineConfig({
    css: { varPrefix: "ui" },
    breakpoint: { direction: "up", identifier: "@media screen" },
    defaultColorMode: "light",
    defaultThemeScheme: "base",
    //notice: { duration: 5000 },
    theme: { responsive: true },
});

// Register the router instance for type safety
declare module "@tanstack/react-router" {
    interface Register {
        router: typeof router;
    }
}

const router = createRouter({
    routeTree,
    defaultPreload: "intent",
    scrollRestoration: true,
    defaultViewTransition: {
        types: ({ fromLocation, toLocation }) => {
            let direction = "none";

            if (fromLocation) {
                const fromIndex = fromLocation.state.__TSR_index;
                const toIndex = toLocation.state.__TSR_index;
                if (fromIndex !== toIndex) {
                    direction = fromIndex > toIndex ? "right" : "left";
                }
            }

            if (
                fromLocation?.state.__TSR_index ===
                toLocation?.state.__TSR_index
            ) {
                direction = "none";
            }

            return [`slide-${direction}`];
        },
    },
});

const rootElement = document.getElementById("root")!;
if (!rootElement.innerHTML) {
    const root = ReactDOM.createRoot(rootElement);
    root.render(
        <StrictMode>
            <UIProvider config={config} theme={extendedTheme}>
                <RouterProvider router={router} />
            </UIProvider>
        </StrictMode>
    );
}
