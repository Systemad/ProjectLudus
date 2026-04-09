import { Suspense } from "react";
import { Grid, Heading, Skeleton, VStack } from "ui";
import { SteamCard } from "../Steam/SteamCard";
import { useGetApiPopularityPopularitytypeidSuspenseHook } from "@src/gen/catalogApi/hooks";

export function SteamPopularitySection({
    title,
    popularityTypeId,
}: {
    title: string;
    popularityTypeId: number;
}) {
    const { data } = useGetApiPopularityPopularitytypeidSuspenseHook({
        popularityTypeId,
        params: { limit: 10 },
    });

    return (
        <VStack gap="8">
            <Heading fontFamily="heading" fontSize={{ base: "3xl", md: "4xl" }}>
                {title}
            </Heading>

            <Suspense
                fallback={
                    <Grid gap="md" templateColumns="repeat(4, 1fr)">
                        {Array.from({ length: 10 }).map((_, index) => (
                            <Skeleton key={index} rounded="2xl" minH="24" />
                        ))}
                    </Grid>
                }
            >
                <Grid templateColumns={{ base: "1fr", md: "repeat(4, minmax(0, 1fr))" }} gap="4">
                    {data.games.map((game, index) => (
                        <SteamCard key={game.id ?? index} game={game} />
                    ))}
                </Grid>
            </Suspense>
        </VStack>
    );
}
