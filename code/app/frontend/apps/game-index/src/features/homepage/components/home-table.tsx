import type { ReactNode } from "react";
import { Link } from "@tanstack/react-router";
import { Box, Image, NativeTable, Text } from "ui";

import type { GameBrowseDto } from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import {
    PreviewCardArrow,
    PreviewCardArrowSvg,
    PreviewCardPopup,
    PreviewCardPortal,
    PreviewCardPositioner,
    PreviewCardRoot,
    PreviewCardTrigger,
    createPreviewCardHandle,
} from "@src/components/preview-card";
import { GamePreviewCard } from "@src/components/game-preview-card";

type Column = {
    header: string;
    render: (game: GameBrowseDto) => ReactNode;
    numeric?: boolean;
};

type Props = {
    title: string;
    icon?: ReactNode;
    games: GameBrowseDto[];
    columns: Column[];
};

export function HomeTable({ title, icon, games, columns }: Props) {
    const previewHandle = createPreviewCardHandle<GameBrowseDto>();

    return (
        <Box rounded="xl" bg="bg.panel" padding="sm">
            <NativeTable.Root variant="line" highlightOnHover={true}>
                <NativeTable.Thead>
                    <NativeTable.Tr>
                        <NativeTable.Th colSpan={2} ps="0" fontWeight={400} fontSize="lg">
                            <Box display="inline-flex" alignItems="center" gap="2">
                                {icon}
                                <Text>{title}</Text>
                            </Box>
                        </NativeTable.Th>
                        {columns.map((col) => (
                            <NativeTable.Th
                                key={col.header}
                                numeric={col.numeric}
                                textAlign="center"
                            >
                                {col.header}
                            </NativeTable.Th>
                        ))}
                    </NativeTable.Tr>
                </NativeTable.Thead>
                <NativeTable.Tbody>
                    {games.map((game) => (
                        <NativeTable.Tr key={game.id}>
                            <NativeTable.Td p={0} verticalAlign="middle" w="1">
                                <PreviewCardTrigger
                                    handle={previewHandle}
                                    payload={game}
                                    style={{ display: "inline-flex", alignItems: "center" }}
                                >
                                    <Image
                                        src={
                                            game.steam?.headerUrl ??
                                            getIGDBImageUrl(game.coverUrl, "cover_small")
                                        }
                                        h="40px"
                                        display="block"
                                        alt={game.name ?? "Game cover"}
                                        w="28"
                                        backgroundSize={"contain"}
                                        objectFit="cover"
                                        rounded="sm"
                                    />
                                </PreviewCardTrigger>
                            </NativeTable.Td>
                            <NativeTable.Td verticalAlign="middle">
                                <Link
                                    to="/games/$gameId"
                                    params={{ gameId: String(game.id) }}
                                    style={{
                                        color: "inherit",
                                        textDecoration: "none",
                                    }}
                                >
                                    <Text>{game.name}</Text>
                                </Link>
                            </NativeTable.Td>
                            {columns.map((col) => (
                                <NativeTable.Td
                                    key={col.header}
                                    numeric={col.numeric}
                                    textAlign="center"
                                    verticalAlign="middle"
                                >
                                    {col.render(game)}
                                </NativeTable.Td>
                            ))}
                        </NativeTable.Tr>
                    ))}
                </NativeTable.Tbody>
            </NativeTable.Root>

            <PreviewCardRoot handle={previewHandle}>
                {({ payload }) => (
                    <PreviewCardPortal>
                        <PreviewCardPositioner sideOffset={8}>
                            <PreviewCardPopup>
                                <PreviewCardArrow>
                                    <PreviewCardArrowSvg />
                                </PreviewCardArrow>
                                {payload && <GamePreviewCard game={payload} />}
                            </PreviewCardPopup>
                        </PreviewCardPositioner>
                    </PreviewCardPortal>
                )}
            </PreviewCardRoot>
        </Box>
    );
}
