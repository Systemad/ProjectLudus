import { Accordion, Button, CheckboxCardGroup, ScrollArea, Text, Flex } from "ui";
import { useRefinementList } from "react-instantsearch";
import { Route } from "@src/routes/search";
import { useNavigate } from "@tanstack/react-router";

type SearchFacetFilterGroupProps = {
    title: string;
    attribute: string;
    index: number;
    currentValues: string[];
};
export function SearchFacetFilterGroup({
    title,
    attribute,
    index,
    currentValues,
}: SearchFacetFilterGroupProps) {
    const { items, refine, canRefine, canToggleShowMore, isShowingMore, toggleShowMore } =
        useRefinementList({
            attribute,
            limit: 12,
            showMore: true,
            showMoreLimit: 30,
            sortBy: ["isRefined:desc", "count:desc", "name:asc"],
        });

    const navigate = useNavigate({ from: Route.fullPath });

    const handleChange = (values: string[]) => {
        navigate({
            search: (prev) => ({
                ...prev,
                [attribute]: values,
                page: 1,
            }),
        });

        const nextSet = new Set(values);
        items.forEach((item) => {
            if (nextSet.has(item.value) !== item.isRefined) {
                refine(item.value);
            }
        });
    };

    return (
        <Accordion.Item button={title} index={index}>
            <Accordion.Panel px="xs" pb="sm">
                {canRefine ? (
                    <>
                        <ScrollArea maxH="sm" pr="xs">
                            <CheckboxCardGroup.Root
                                orientation="vertical"
                                size="sm"
                                value={currentValues}
                                onChange={handleChange}
                            >
                                {items.map((item) => (
                                    <CheckboxCardGroup.Item.Root
                                        key={item.value}
                                        value={item.value}
                                    >
                                        <Flex>
                                            <Text
                                                as="span"
                                                fontSize="sm"
                                                lineClamp={1}
                                                minW={0}
                                                flex="1"
                                            >
                                                {item.label}
                                            </Text>
                                            <Text
                                                as="span"
                                                fontSize="xs"
                                                color="fg.muted"
                                                whiteSpace="nowrap"
                                                ml="auto"
                                            >
                                                {item.count}
                                            </Text>
                                        </Flex>
                                    </CheckboxCardGroup.Item.Root>
                                ))}
                            </CheckboxCardGroup.Root>
                        </ScrollArea>

                        {canToggleShowMore && (
                            <Button size="xs" variant="ghost" mt="xs" onClick={toggleShowMore}>
                                {isShowingMore ? "Show less" : "Show more"}
                            </Button>
                        )}
                    </>
                ) : (
                    <Text color="fg.muted" fontSize="sm">
                        No options found
                    </Text>
                )}
            </Accordion.Panel>
        </Accordion.Item>
    );
}
