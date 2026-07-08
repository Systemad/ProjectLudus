import { createFileRoute } from "@tanstack/react-router";
import { PageWrapper } from "@src/app/page-wrapper";
import { Text } from "@astryxdesign/core/Text";

export const Route = createFileRoute("/about")({
    component: About,
});

function About() {
    return (
        <PageWrapper paddingBlock="clamp(1rem, 3vw, 1.5rem)">
            <Text>Hello from About!</Text>
        </PageWrapper>
    );
}