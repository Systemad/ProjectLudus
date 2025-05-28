import {
    Stack,
    Flex,
    Image,
    Box,
    Text,
    Divider,
    Title,
    Badge,
    Transition,
    Rating,
    Paper,
    Group,
} from "@mantine/core";
import { createFileRoute } from "@tanstack/react-router";
import { Surface } from "~/features/shared/components/Surface/Surface";
import { useHover } from "@mantine/hooks";
export const Route = createFileRoute("/games/$gameId")({
    component: RouteComponent,
});

function RouteComponent() {
    const { gameId } = Route.useParams();
    const { hovered, ref } = useHover();
    const expandText = hovered ? undefined : 4;
    return (
        <Flex
            mih={50}
            gap="md"
            align="flex-start"
            justify="flex-start"
            wrap="nowrap"
        >
            <Box w={300}>
                <Stack gap="md" align="stretch" justify="flex-start">
                    <Image
                        fit="cover"
                        height={300}
                        radius="md"
                        src="https://raw.githubusercontent.com/mantinedev/mantine/master/.demo/images/bg-7.png"
                    />
                    <Surface>
                        <Stack gap="sm">
                            <Text size="lg" fw={700}>
                                Game Details
                            </Text>

                            <Text size="sm">
                                <strong>Format:</strong> Main game
                            </Text>

                            <Text size="sm">
                                <strong>Release Date:</strong> 2009-02-26
                            </Text>

                            <Divider
                                label="Themes"
                                labelPosition="center"
                                my="xs"
                            />
                            <Stack gap={2}>
                                <Text size="sm">Actions</Text>
                                <Text size="sm">Drama</Text>
                                <Text size="sm">Open World</Text>
                            </Stack>

                            <Divider
                                label="Platforms"
                                labelPosition="center"
                                my="xs"
                            />
                            <Stack gap={2}>
                                <Text size="sm">PlayStation</Text>
                                <Text size="sm">Xbox</Text>
                                <Text size="sm">Open World</Text>
                            </Stack>
                        </Stack>
                    </Surface>
                </Stack>
            </Box>

            <Stack gap="md" w="100%">
                <Surface w="100%">
                    <Stack>
                        <Title order={2}>Title</Title>
                        <Flex wrap="wrap" gap="sm">
                            <Badge size="lg">Hello</Badge>
                            <Badge size="lg">World</Badge>
                            <Badge size="lg">Responsive</Badge>
                        </Flex>

                        <Text lineClamp={expandText} ref={ref}>
                            From Bulbapedia: Bulbasaur is a small, quadrupedal
                            Pokémon that has blue-green skin with darker
                            patches. It has red eyes with white pupils, pointed,
                            ear-like structures on top of its head, and a short,
                            blunt snout with a wide mouth. A pair of small,
                            pointed teeth are visible in the upper jaw when its
                            mouth is open. Each of its thick legs ends with
                            three sharp claws. On Bulbasaur's back is a green
                            plant bulb, which is grown from a seed planted there
                            at birth. The bulb also conceals two slender,
                            tentacle-like vines and provides it with energy
                        </Text>
                        <Group>
                            <Paper radius="md" p="sm">
                                <Stack>
                                    <Text ta="center">IGDB SCORE</Text>
                                    <Rating
                                        defaultValue={2}
                                        count={10}
                                        readOnly
                                    />
                                </Stack>
                            </Paper>
                            <Paper radius="md" p="sm">
                                <Stack>
                                    <Text ta="center">YOUR SCORE</Text>
                                    <Rating defaultValue={2} count={10} />
                                </Stack>
                            </Paper>
                        </Group>
                    </Stack>
                </Surface>
                <Surface>
                    <Text>Other stuff</Text>
                </Surface>
            </Stack>
        </Flex>
    );
}
