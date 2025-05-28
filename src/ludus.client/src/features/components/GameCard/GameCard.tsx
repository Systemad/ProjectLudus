import { HeartIcon } from "@phosphor-icons/react";
import { Card, Image, Text, Group, ActionIcon } from "@mantine/core";
import { Link } from "@tanstack/react-router";
import classes from "./GameCard.module.css";

type Props = {
    Id: string;
};
export function GameCard({ Id }: Props) {
    return (
        <Card radius={"lg"} withBorder className={classes.item}>
            <Card.Section>
                <Link to={"/games/$gameId"} params={{ gameId: Id }}>
                    <Image
                        src="https://images.unsplash.com/photo-1579227114347-15d08fc37cae?ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&ixlib=rb-1.2.1&auto=format&fit=crop&w=2550&q=80"
                        h={200}
                        alt="No way!"
                    />
                </Link>
            </Card.Section>

            <Group justify="space-between" mt="md">
                <Text fw={500} size="lg">
                    Game title
                </Text>

                <ActionIcon variant="transparent" aria-label="Favorite">
                    <HeartIcon weight="fill" color="#904b40" size={48} />
                </ActionIcon>
            </Group>
        </Card>
    );
}
