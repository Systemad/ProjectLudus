import { createFileRoute } from "@tanstack/react-router";
import { HeartIcon } from "@phosphor-icons/react";
import {
    SimpleGrid,
    Card,
    Image,
    Text,
    Group,
    ActionIcon,
} from "@mantine/core";
export const Route = createFileRoute("/")({
    component: Index,
});

function Index() {
    return (
        <SimpleGrid cols={5}>
            {Array.from({ length: 18 }, (_, i) => (
                <Card key={i} radius={"md"} withBorder>
                    <Card.Section>
                        <Image
                            src="https://images.unsplash.com/photo-1579227114347-15d08fc37cae?ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&ixlib=rb-1.2.1&auto=format&fit=crop&w=2550&q=80"
                            h={160}
                            alt="No way!"
                        />
                    </Card.Section>

                    <Group justify="space-between" mt="md">
                        <Text fw={500} size="lg">
                            Game title
                        </Text>

                        <ActionIcon variant="transparent" aria-label="Favorite">
                            <HeartIcon
                                weight="fill"
                                color="#904b40"
                                size={48}
                            />
                        </ActionIcon>
                    </Group>
                </Card>
            ))}
        </SimpleGrid>
    );
}
