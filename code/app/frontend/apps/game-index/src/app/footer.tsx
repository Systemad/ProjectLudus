import { Section } from "@astryxdesign/core/Section";
import { Divider } from "@astryxdesign/core/Divider";
import { VStack } from "@astryxdesign/core/VStack";
import { Heading, Text } from "@astryxdesign/core/Text";
import { Button } from "@astryxdesign/core/Button";
import { EU } from "country-flag-icons/react/3x2";

export function Footer() {
    return (
        <Section role="contentinfo" padding={6} style={{ paddingTop: "var(--spacing-10)" }}>
            <VStack gap={4} hAlign="center">
                <Heading level={4}>
                    GAME-INDEX
                </Heading>
                <Text
                    type="supporting"
                    color="secondary"
                    justify="center"
                    style={{ maxWidth: 448 }}
                >
                    game-index.app is a fan-made website and is not affiliated
                    with IGDB. All the logos, images, trademarks and creatives
                    are property of their respective owners.
                </Text>
                <Button variant="secondary" size="sm" href="#" label="Made in EU">
                    <EU style={{ width: "1em", height: "auto" }} />
                </Button>
                <Divider />
                <Text type="supporting" color="secondary">
                    &copy;{new Date().getFullYear()} Game-Index
                </Text>
            </VStack>
        </Section>
    );
}
