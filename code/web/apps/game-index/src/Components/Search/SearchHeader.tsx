"use client";

import type { TypeOf } from "zod";
import { searchQueryParamsSchema } from "../../gen";
import { Box, Button, Flex, Input } from "@packages/ui";

export interface SearchHeaderProps {
    query: TypeOf<typeof searchQueryParamsSchema>;
    onNameChange: (value: string) => void;
}

export function SearchHeader({ query, onNameChange }: SearchHeaderProps) {
    return (
        <Flex w="full" gap="sm" align="center">
            <Box w="full">
                <Input
                    placeholder="Search games..."
                    value={query.Name || ""}
                    onChange={(event: any) => {
                        onNameChange(event?.target?.value ?? "");
                    }}
                    size="lg"
                    rounded="xl"
                />
            </Box>

            <Button
                variant="outline"
                size="lg"
                rounded="xl"
                aria-label="Grid view"
                disabled
            >
                ☐
            </Button>
            <Button
                variant="outline"
                size="lg"
                rounded="xl"
                aria-label="List view"
                disabled
            >
                ☰
            </Button>
        </Flex>
    );
}
