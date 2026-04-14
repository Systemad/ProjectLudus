import { Box, Flex, Heading, Image, Text } from "ui";
import {
    getCompanyLogoUrl,
    getCompanyStatusLabel,
    type CompanySearchHit,
} from "./companySearchUtils";
import { SearchHitCardFrame } from "./SearchHitCardFrame";

type CompanyHitCardProps = {
    hit: CompanySearchHit;
};

export function CompanyHitCard({ hit }: CompanyHitCardProps) {
    const imageUrl = getCompanyLogoUrl(hit.logo_url);

    return (
        <SearchHitCardFrame>
            <Box aspectRatio="3/2" overflow="hidden" rounded="lg" bg="blackAlpha.400">
                {imageUrl ? (
                    <Image
                        src={imageUrl}
                        alt={hit.name ? `${hit.name} logo` : "Company logo"}
                        w="full"
                        h="full"
                        objectFit="contain"
                        p="sm"
                    />
                ) : (
                    <Flex w="full" h="full" align="center" justify="center">
                        <Text fontSize="sm" color="fg.muted">
                            No logo available
                        </Text>
                    </Flex>
                )}
            </Box>

            <Flex direction="column" gap="xs">
                <Heading size="sm" lineClamp={2} minH="2.75rem">
                    {hit.name ?? "Unknown company"}
                </Heading>

                <Text fontSize="sm" color="fg.muted">
                    Status: {getCompanyStatusLabel(hit.status)}
                </Text>

                <Text fontSize="sm" fontWeight="semibold">
                    Developed: {hit.games_developed_count ?? 0}
                </Text>

                <Text fontSize="sm" fontWeight="semibold">
                    Published: {hit.games_published_count ?? 0}
                </Text>
            </Flex>
        </SearchHitCardFrame>
    );
}
