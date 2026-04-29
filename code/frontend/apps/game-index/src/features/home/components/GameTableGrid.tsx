import { Link } from "@tanstack/react-router";
import { Container, List, HStack, Image, Text, Format, SimpleGrid, GridItem, VStack } from "ui";
import { PreviewCard } from "@base-ui/react/preview-card";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { isTbaReleaseDate } from "@src/utils/dateUtils";
import { GamePreviewCard } from "@src/components/home/GamePreviewCard/GamePreviewCard";
import type { GameDto } from "@src/gen/catalogApi";
import styles from "@src/components/layout/previewImage.module.css";

type TableConfig = {
    title: string;
    games: GameDto[];
    showReleaseDate?: boolean;
};

export function GameTableGrid({ tables }: { tables: TableConfig[] }) {
    return (
        <SimpleGrid columns={{ base: 1, md: 2, lg: 3 }} gap="6">
            {tables.map((table) => (
                <GridItem key={table.title}>
                    <Container.Root rounded="2xl">
                        <Container.Header>
                            <Text fontWeight="semibold" color="fg.base">
                                {table.title}
                            </Text>
                        </Container.Header>
                        <Container.Body>
                            <List.Root>
                                {table.games.slice(0, 10).map((game, index) => (
                                    <List.Item key={game.id ?? `game-fallback-${index}`}>
                                        <HStack gap="3" align="center" w="full">
                                            <Text
                                                fontSize="sm"
                                                color="fg.subtle"
                                                fontWeight="medium"
                                                minW="4"
                                                textAlign="right"
                                            >
                                                {index + 1}
                                            </Text>
                                            <Image
                                                src={getIGDBImageUrl(game.coverUrl, "cover_big")}
                                                alt={game?.name ?? ""}
                                                w={{ base: "9", md: "10" }}
                                                h={{ base: "9", md: "10" }}
                                                rounded="md"
                                                objectFit="cover"
                                                flexShrink={0}
                                            />
                                        <VStack gap="0" flex={1} minW="0">
                                            <PreviewCard.Root>
                                                <PreviewCard.Trigger>
                                                    <Link
                                                        to="/games/$gameId"
                                                        params={{ gameId: String(game.id) }}
                                                        style={{
                                                            color: "inherit",
                                                            textDecoration: "none",
                                                        }}
                                                    >
                                                        <Text
                                                            fontSize="sm"
                                                            fontWeight="medium"
                                                            lineClamp={1}
                                                        >
                                                            {game.name ?? "Unknown"}
                                                        </Text>
                                                    </Link>
                                                </PreviewCard.Trigger>
                                                <PreviewCard.Portal>
                                                    <PreviewCard.Positioner sideOffset={8}>
                                                        <PreviewCard.Popup className={styles.Popup}>
                                                            <GamePreviewCard game={game} />
                                                        </PreviewCard.Popup>
                                                    </PreviewCard.Positioner>
                                                </PreviewCard.Portal>
                                            </PreviewCard.Root>
                                            {table.showReleaseDate &&
                                                (!isTbaReleaseDate(game.firstReleaseDate) &&
                                                game.firstReleaseDate ? (
                                                    <Format.DateTime
                                                        value={new Date(game.firstReleaseDate)}
                                                        month="short"
                                                        day="2-digit"
                                                        year="numeric"
                                                        fontSize="xs"
                                                        color="fg.subtle"
                                                    />
                                                ) : (
                                                    <Text fontSize="xs" color="fg.subtle">
                                                        TBA
                                                    </Text>
                                                ))}
                                        </VStack>
                                        </HStack>
                                    </List.Item>
                                ))}
                            </List.Root>
                        </Container.Body>
                    </Container.Root>
                </GridItem>
            ))}
        </SimpleGrid>
    );
}
