import { useNavigate } from "@tanstack/react-router";
import { Section } from "@astryxdesign/core/Section";
import { Heading, Text } from "@astryxdesign/core/Text";
import { VStack } from "@astryxdesign/core/VStack";
import { AspectRatio } from "@astryxdesign/core/AspectRatio";
import { HoverCard } from "@astryxdesign/core/HoverCard";
import { Overlay } from "@astryxdesign/core/Overlay";
import { MediaTheme } from "@astryxdesign/core/theme";
import { Table, proportional } from "@astryxdesign/core/Table";
import type { TableColumn } from "@astryxdesign/core/Table";
import type { GameBrowseDto } from "@src/gen/catalogApi";
import { DataCard } from "@src/components/data-card";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

type Column = {
    header: string;
    render: (game: GameBrowseDto) => React.ReactNode;
    numeric?: boolean;
};

type Props = {
    title: string;
    icon?: React.ReactNode;
    games: GameBrowseDto[];
    columns: Column[];
};

export function HomeTable({ title, icon, games, columns }: Props) {
    const navigate = useNavigate();

    const tableColumns: TableColumn<GameBrowseDto>[] = [
        {
            key: "name",
            header: "Game",
            width: proportional(3),
            renderCell: (game) => {
                const coverSrc =
                    game.steam?.headerUrl ?? getIGDBImageUrl(game.coverUrl, "cover_small");
                const players = game.steam?.currentPlayers?.toLocaleString();
                const releaseDate = game.firstReleaseDate
                    ? new Date(game.firstReleaseDate).toLocaleDateString("en-US", {
                          month: "short",
                          day: "numeric",
                          year: "numeric",
                      })
                    : "TBA";

                return (
                    <HoverCard
                        placement="end"
                        delay={200}
                        content={
                            <Overlay
                                position="bottom"
                                align="start"
                                content={
                                    <MediaTheme mode="dark">
                                        <Section variant="transparent" padding={3}>
                                            <VStack gap={1}>
                                                <Heading level={5} maxLines={2}>
                                                    {game.name}
                                                </Heading>
                                                {players && <Text type="supporting">👥 {players} current players</Text>}
                                                <Text type="supporting">📅 {releaseDate}</Text>
                                                {game.steam?.pricing?.finalFormatted && (
                                                    <Text type="supporting">💰 {game.steam.pricing.finalFormatted}</Text>
                                                )}
                                            </VStack>
                                        </Section>
                                    </MediaTheme>
                                }
                            >
                                <AspectRatio
                                    ratio={3 / 4}
                                    style={{
                                        width: 240,
                                        maxWidth: "100%",
                                        overflow: "hidden",
                                        backgroundColor: "var(--color-background-surface)",
                                        isolation: "isolate",
                                    }}
                                >
                                    <img
                                        src={coverSrc}
                                        alt=""
                                        aria-hidden
                                        style={{
                                            position: "absolute",
                                            inset: 0,
                                            width: "100%",
                                            height: "100%",
                                            objectFit: "cover",
                                            filter: "blur(18px) brightness(0.35)",
                                            transform: "scale(1.12)",
                                            zIndex: 0,
                                        }}
                                    />
                                    <img
                                        src={coverSrc}
                                        alt={game.name}
                                        style={{
                                            position: "relative",
                                            width: "100%",
                                            height: "100%",
                                            objectFit: "contain",
                                            display: "block",
                                            zIndex: 1,
                                        }}
                                    />
                                </AspectRatio>
                            </Overlay>
                        }
                    >
                        <span
                            role="link"
                            tabIndex={0}
                            onClick={() =>
                                navigate({
                                    to: "/games/$gameId",
                                    params: { gameId: String(game.id) },
                                })
                            }
                            onKeyDown={(e) =>
                                e.key === "Enter" &&
                                navigate({
                                    to: "/games/$gameId",
                                    params: { gameId: String(game.id) },
                                })
                            }
                            style={{
                                display: "flex",
                                alignItems: "center",
                                gap: "0.75rem",
                                cursor: "pointer",
                            }}
                        >
                            <img
                                src={coverSrc}
                                alt={game.name}
                                style={{
                                    height: "50px",
                                    width: "72px",
                                    objectFit: "cover",
                                    borderRadius: "var(--radius-sm)",
                                    display: "block",
                                    flexShrink: 0,
                                }}
                            />
                            <Text>{game.name}</Text>
                        </span>
                    </HoverCard>
                );
            },
        },
        ...columns.map((col) => ({
            key: col.header,
            header: col.header,
            width: proportional(1),
            align: col.numeric ? ("end" as const) : ("center" as const),
            renderCell: col.render,
        })),
    ];

    const tableContent = (
        <>
            <Text
                type="label"
                color="secondary"
                style={{
                    display: "flex",
                    alignItems: "center",
                    gap: "0.5rem",
                    marginBottom: "0.75rem",
                }}
            >
                {icon}
                {title}
            </Text>
            <Table<GameBrowseDto>
                data={games}
                columns={tableColumns}
                idKey="id"
                density="compact"
                dividers="none"
                hasHover
                verticalAlign="middle"
            />
        </>
    );

    return (
        <DataCard padding={4}>
            {tableContent}
        </DataCard>
    );
}
