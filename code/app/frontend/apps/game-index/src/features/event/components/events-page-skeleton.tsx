import { Card } from "@astryxdesign/core/Card";
import { Spinner } from "@astryxdesign/core/Spinner";
import { Skeleton } from "@astryxdesign/core/Skeleton";
import { Grid } from "@astryxdesign/core/Grid";
import { HStack } from "@astryxdesign/core/HStack";
import { VStack } from "@astryxdesign/core/VStack";
import { Text } from "@astryxdesign/core/Text";

function MonthCardSkeleton() {
    return (
        <Card padding={3}>
            <Skeleton width="100%" height="1.5rem" />

            <VStack gap={2} hAlign="stretch">
                {[0, 1, 2].map((index) => (
                    <HStack
                        key={index}
                        hAlign="between"
                        vAlign="start"
                        gap={3}
                        style={{
                            flexDirection: "column",
                            paddingLeft: "0.5rem",
                            paddingRight: "0.5rem",
                            paddingTop: "0.5rem",
                            paddingBottom: "0.75rem",
                            borderRadius: "var(--radius-md)",
                            background: "var(--bg-surface)",
                        }}
                    >
                        <HStack gap={3} style={{minWidth: 0, flex: 1, width: "100%"}}>
                            <Skeleton>
                                <div
                                    style={{
                                        width: "2.25rem",
                                        height: "2.75rem",
                                        borderRadius: "var(--radius-md)",
                                    }}
                                />
                            </Skeleton>

                            <VStack hAlign="start" gap={1} style={{ minWidth: 0, flex: "1" }}>
                                <Skeleton width="100%" />
                                <Skeleton width="4rem" height="0.75rem" />
                            </VStack>
                        </HStack>

                        <Skeleton width="8rem" height="0.75rem" />
                    </HStack>
                ))}
            </VStack>
        </Card>
    );
}

export function EventsPageLoadingState() {
    return (
        <VStack hAlign="stretch" gap={6}>
            <VStack hAlign="stretch" gap={3}>
                <HStack hAlign="between" wrap="wrap" gap={3} style={{alignItems: "baseline"}}>
                    <Skeleton width="8rem" height="2.25rem" />
                    <Skeleton width="5rem" height="1rem" />
                </HStack>

                <HStack gap={2} style={{maxWidth: "24rem"}}>
                    {[0, 1, 2].map((index) => (
                        <Skeleton key={index} width="100%" height="2rem" radius="rounded" />
                    ))}
                </HStack>

                <HStack gap={2} vAlign="center" style={{color: "var(--fg-tertiary)"}}>
                    <Spinner size="lg" />
                    <Text style={{fontSize: "0.875rem"}}>Loading events...</Text>
                </HStack>
            </VStack>

            <Grid columns={{minWidth: 280}} gap={4}>
                {[0, 1, 2, 3, 4, 5, 6, 7].map((index) => (
                    <div key={index}>
                        <MonthCardSkeleton />
                    </div>
                ))}
            </Grid>
        </VStack>
    );
}
