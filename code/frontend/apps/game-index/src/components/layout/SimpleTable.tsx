import { Box, NativeTable } from "ui";
import { PreviewCard } from "@base-ui/react/preview-card";
import GamePreviewCard from "@src/components/home/GamePreviewCard/GamePreviewCard";
import type { GameDto } from "@src/gen/catalogApi";
import styles from "./previewImage.module.css";

export const SimpleTable = ({
    headers,
    rows,
    hoverCardGames,
}: {
    headers: (string | JSX.Element)[];
    rows: (string | number | JSX.Element)[][];
    hoverCardGames?: GameDto[];
}) => {
    const TableContent = (
        <Box overflowX="auto" overflowY="hidden" w="full">
            <NativeTable.Root minW={{ base: "36rem", md: "full" }}>
                <NativeTable.Thead>
                    <NativeTable.Tr>
                        {headers.map((h, index) => (
                            <NativeTable.Th key={typeof h === "string" ? h : `header-${index}`}>
                                {h}
                            </NativeTable.Th>
                        ))}
                    </NativeTable.Tr>
                </NativeTable.Thead>
                <NativeTable.Tbody>
                    {rows.map((c, rowIndex) => {
                        const [_id, ...cells] = c;
                        const hoverGame = hoverCardGames?.[rowIndex];

                        const TableRowContent = (
                            <NativeTable.Tr key={`row-${rowIndex}`}>
                                {cells.map((cell, j) => {
                                    const headerKey = `col-${j}`;
                                    const cellContent = cell;

                                    return hoverGame ? (
                                        <NativeTable.Td key={`row-${rowIndex}-${headerKey}`}>
                                            <PreviewCard.Root>
                                                <PreviewCard.Trigger>
                                                    <Box
                                                        display="flex"
                                                        alignItems="center"
                                                        h="full"
                                                    >
                                                        {cellContent}
                                                    </Box>
                                                </PreviewCard.Trigger>
                                                <PreviewCard.Portal>
                                                    <PreviewCard.Positioner sideOffset={8}>
                                                        <PreviewCard.Popup className={styles.Popup}>
                                                            <GamePreviewCard game={hoverGame} />
                                                        </PreviewCard.Popup>
                                                    </PreviewCard.Positioner>
                                                </PreviewCard.Portal>
                                            </PreviewCard.Root>
                                        </NativeTable.Td>
                                    ) : (
                                        <NativeTable.Td key={`row-${rowIndex}-${headerKey}`}>
                                            <Box display="flex" alignItems="center" h="full">
                                                {cellContent}
                                            </Box>
                                        </NativeTable.Td>
                                    );
                                })}
                            </NativeTable.Tr>
                        );

                        return TableRowContent;
                    })}
                </NativeTable.Tbody>
            </NativeTable.Root>
        </Box>
    );
    return TableContent;
};
