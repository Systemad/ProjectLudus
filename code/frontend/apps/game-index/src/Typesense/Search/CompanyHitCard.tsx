import { Box, Card, Flex, Heading, Image, Text } from "ui";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import type { CompanySearchHit } from "../utils/hits";
import { getCompanyStatusLabel } from "../utils/searchUtils";

type CompanyHitCardProps = {
    hit: CompanySearchHit;
};

export function CompanyHitCard({ hit }: CompanyHitCardProps) {
    const imageUrl = getIGDBImageUrl(hit.logo_url, "logo_med");

    return (
        <Card.Root h="full" w="full" rounded="lg" bg="bg.surface" border="none">
            <Card.Header>
                <Box aspectRatio="3/2" overflow="hidden" rounded="lg" bg="bg.subtle">
                    {imageUrl ? (
                        <Image
                            src={imageUrl}
                            alt={hit.name ? `${hit.name} logo` : "Company logo"}
                            w="full"
                            h="full"
                            objectFit="contain"
                            p={{ base: "xs", md: "sm" }}
                        />
                    ) : (
                        <Flex w="full" h="full" align="center" justify="center">
                            <Text fontSize="sm" color="fg.muted">
                                No logo available
                            </Text>
                        </Flex>
                    )}
                </Box>
            </Card.Header>

            <Card.Body p={{ base: "xs", md: "sm" }}>
                <Flex direction="column" gap="xs">
                    <Heading size="sm" lineClamp={2} minH="2.5rem" color="fg.base">
                        {hit.name ?? "Unknown company"}
                    </Heading>

                    <Text fontSize="sm" color="fg.subtle">
                        Status: {getCompanyStatusLabel(hit.status)}
                    </Text>

                    <Text fontSize="sm" fontWeight="semibold" color="fg.base">
                        Developed: {hit.games_developed_count ?? 0}
                    </Text>

                    <Text fontSize="sm" fontWeight="semibold" color="fg.base">
                        Published: {hit.games_published_count ?? 0}
                    </Text>
                </Flex>
            </Card.Body>
        </Card.Root>
    );
}
