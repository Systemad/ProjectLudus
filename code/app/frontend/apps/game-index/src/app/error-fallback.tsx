import { Box, Heading, Text, VStack } from "ui";

export function ErrorComponent({ error }: { error: Error }) {
    return (
        <Box minH="60vh" display="flex" alignItems="center" justifyContent="center" px="4">
            <VStack gap="4" textAlign="center" maxW="xl">
                <Heading size="2xl">Something went wrong</Heading>
                <Text color="fg.muted">{error.message}</Text>
                <Text color="fg.muted">Try refreshing the page.</Text>
            </VStack>
        </Box>
    );
}
