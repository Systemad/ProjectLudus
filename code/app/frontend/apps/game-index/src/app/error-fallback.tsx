import { Heading, Text } from "@astryxdesign/core/Text";
import { VStack } from "@astryxdesign/core/VStack";

export function ErrorComponent({ error }: { error: Error }) {
    return (
        <div style={{ minHeight: "60vh", display: "flex", alignItems: "center", justifyContent: "center", padding: "0 1rem" }}>
            <VStack gap={4} style={{textAlign: "center", maxWidth: "36rem"}}>
                <Heading level={1}>Something went wrong</Heading>
                <Text color="secondary">{error.message}</Text>
                <Text color="secondary">Try refreshing the page.</Text>
            </VStack>
        </div>
    );
}
