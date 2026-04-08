import { Configure, Hits, InstantSearch } from "react-instantsearch";
import { Box, Heading, VStack } from "ui";
import {
    SEARCH_INDEX_NAME,
    steamWishlistedUpcomingSearchClient,
} from "@src/Typesense/instantsearch";
import { HomeRailHitCard, homeRailHitClassNames, homeRailHitListCss } from "./HomeRailHitCard";

export function SteamWishlistedUpcomingSection() {
    const railParams = {
        sort_by: "steam_most_wishlisted_upcoming:desc",
        filter_by: "steam_most_wishlisted_upcoming:!=null",
    } as const;

    return (
        <VStack align="stretch" gap="8">
            <Heading fontFamily="heading" fontSize={{ base: "3xl", md: "4xl" }}>
                Steam Most Wishlisted Upcoming
            </Heading>

            <InstantSearch
                searchClient={steamWishlistedUpcomingSearchClient}
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
