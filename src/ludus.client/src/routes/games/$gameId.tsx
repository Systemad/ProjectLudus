import {
    Stack,
    Flex,
    Image,
    Box,
    Text,
    Divider,
    Title,
    Badge,
    SimpleGrid,
    Rating,
    Paper,
    Group,
} from "@mantine/core";
import { createFileRoute } from "@tanstack/react-router";
import { Surface } from "~/features/shared/components/Surface/Surface";
import { useHover } from "@mantine/hooks";
import { GameCard } from "~/features/components/GameCard/GameCard";
import {
    gameStatusEnum,
    useMeCollectionsGetEndpoint,
    usePublicGamesGetGamesByIdEndpoint,
    usePublicGamesGetSimilarGamesEndpoint,
    type UpdateUserGameDataRequest,
} from "~/api";
import { IGDBImage } from "~/features/components/IGDBImage/IGDBImage";
export const Route = createFileRoute("/games/$gameId")({
    component: RouteComponent,
});

function RouteComponent() {
    const { gameId } = Route.useParams();
    const { hovered, ref } = useHover();
    const id = Number(gameId);
    const { isPending, isError, data, error } =
        usePublicGamesGetGamesByIdEndpoint(id);

    const {
        isPending: isCollectionPending,
        isError: isCollectionError,
        data: collectionData,
        error: collectionError,
    } = useMeCollectionsGetEndpoint(id);

    const {
        isPending: isSimilarGamesPending,
        isError: isSimilarGamesError,
        data: similarGames,
        error: similarGamesError,
    } = usePublicGamesGetSimilarGamesEndpoint(id);

    if (isPending) {
        return <span>Loading...</span>;
    }

    if (isError) {
        return <span>Error: {error.message}</span>;
    }
    const collectionDoesntExist = collectionError?.status === 404;
    /*
export type UpdateUserGameDataRequest = {
  id: string
  status: GameStatus
  startDate?: string | null
  endDate?: string | null
  rating?: number | null
  notes?: string | null
}
*/
    //const updateGameItem: UpdateUserGameDataRequest = {};
    const expandText = hovered ? undefined : 4;
    const scoreOutOf10 = data.data.game.rating / 10;
    const truncatedScore = Math.floor(scoreOutOf10 * 1000) / 1000;
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
                    <IGDBImage
                        fit="cover"
                        size="1080p"
                        imageId={data.data.game.cover.image_id}
                        h={300}
                    />

                    <Surface>
                        <Stack gap="xs">
                            <Text size="lg" fw={700}>
                                Game Details
                            </Text>

                            <Text size="sm">
                                <strong>Format:</strong>{" "}
                                {data.data.game.game_type.type}
                            </Text>

                            <Text size="sm">
                                <strong>Release Date:</strong>{" "}
                                {new Date(
                                    data.data.game.first_release_date * 1000
                                ).toLocaleDateString()}
                            </Text>

                            <Divider label="Themes" labelPosition="center" />
                            <Stack gap={3}>
                                {data.data.game.themes.map((item, index) => (
                                    <Text key={index} size="sm">
                                        {item.name}
                                    </Text>
                                ))}
                            </Stack>

                            <Divider label="Platforms" labelPosition="center" />
                            <Stack gap={3}>
                                {data.data.game.platforms.map((item, index) => (
                                    <Group
                                        justify="space-between"
                                        wrap="nowrap"
                                    >
                                        <Text key={index} size="sm">
                                            {item.name}
                                        </Text>
                                        <Text size="sm">
                                            {
                                                data.data.game.release_dates.find(
                                                    (x) =>
                                                        x.platform.name ==
                                                        item.name
                                                )?.human
                                            }
                                        </Text>
                                    </Group>
                                ))}
                            </Stack>
                        </Stack>
                    </Surface>
                </Stack>
            </Box>

            <Stack gap="md" w="100%">
                <Surface w="100%">
                    <Stack>
                        <Title order={2}>{data.data.game.name}</Title>
                        <Flex wrap="wrap" gap="sm">
                            {data.data.game.genres.map((item, index) => (
                                <Badge key={index} size="lg">
                                    {item.name}
                                </Badge>
                            ))}
                        </Flex>

                        <Text lineClamp={expandText} ref={ref}>
                            {data.data.game.storyline}
                        </Text>
                        <Group>
                            <Paper radius="md" p="sm">
                                <Stack gap="xs">
                                    <Text ta="center">IGDB SCORE</Text>
                                    <Rating
                                        value={truncatedScore}
                                        fractions={10}
                                        count={10}
                                        readOnly
                                    />
                                </Stack>
                            </Paper>
                            <Paper radius="md" p="sm">
                                {collectionDoesntExist && (
                                    <Stack gap="xs">
                                        <Text ta="center">YOUR SCORE</Text>
                                        <Rating defaultValue={2} count={10} />
                                    </Stack>
                                )}
                            </Paper>
                        </Group>
                    </Stack>
                </Surface>
                <Surface>
                    <Title order={3}>Recommended Games</Title>
                    <SimpleGrid
                        mt="md"
                        cols={{ base: 1, sm: 2, lg: 5 }}
                        spacing="lg"
                    >
                        {similarGames?.data.similarGames.map((item, index) => (
                            <GameCard
                                key={index}
                                fontSize="md"
                                height={175}
                                Game={item}
                            ></GameCard>
                        ))}
                    </SimpleGrid>
                </Surface>
            </Stack>
        </Flex>
    );
}
