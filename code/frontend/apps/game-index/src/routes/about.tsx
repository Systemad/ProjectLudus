import { createFileRoute } from "@tanstack/react-router";
import { PageWrapper } from "@src/components/layout/PageWrapper";
import { Text } from "ui";

export const Route = createFileRoute("/about")({
    component: About,
});

function About() {
    return (
        <PageWrapper py={{ base: "4", md: "6" }}>
            <Text>Hello from About!</Text>
        </PageWrapper>
    );
}
