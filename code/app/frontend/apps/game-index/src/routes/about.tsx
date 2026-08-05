import { createFileRoute } from "@tanstack/react-router";
import { Text } from "@astryxdesign/core/Text";

export const Route = createFileRoute("/about")({
    component: About,
});

function About() {
    return <Text>Hello from About!</Text>;
}