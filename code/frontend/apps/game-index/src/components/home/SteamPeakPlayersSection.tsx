import { Configure, Hits, InstantSearch } from "react-instantsearch";
import { Box, Heading, VStack } from "ui";
import { SEARCH_INDEX_NAME, steamPeakPlayersSearchClient } from "@src/Typesense/instantsearch";
import { HomeRailHitCard, homeRailHitClassNames, homeRailHitListCss } from "./HomeRailHitCard";

export function SteamPeakPlayersSection() {
    const railParams = {
        sort_by: "steam_24hr_peak_players:desc",
        filter_by: "steam_24hr_peak_players:!=null",
    } as const;

    return (
        <VStack align="stretch" gap="8">
            <Heading fontFamily="heading" fontSize={{ base: "3xl", md: "4xl" }}>
                Steam 24h Peak Players
            </Heading>

            <InstantSearch
                searchClient={steamPeakPlayersSearchClient}
                indexName={SEARCH_INDEX_NAME}
                future={{ preserveSharedStateOnUnmount: true }}
            >
                <Configure query="*" hitsPerPage={10} {...(railParams as object)} />

                <Box css={homeRailHitListCss}>
                    <Hits hitComponent={HomeRailHitCard} classNames={homeRailHitClassNames} />
                </Box>
            </InstantSearch>
        </VStack>
    );
}
