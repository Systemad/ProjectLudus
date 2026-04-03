import { Accordion, Button, CheckboxCardGroup, ScrollArea, Text } from "ui";
import { useRefinementList } from "react-instantsearch";

type SearchFacetFilterGroupProps = {
    title: string;
    attribute: string;
    index: number;
};

export function SearchFacetFilterGroup({ title, attribute, index }: SearchFacetFilterGroupProps) {
    const { items, refine, canRefine, canToggleShowMore, isShowingMore, toggleShowMore } =
        useRefinementList({
            attribute,
            limit: 12,
            showMore: true,
            showMoreLimit: 30,
            sortBy: ["isRefined:desc", "count:desc", "name:asc"],
        });

    const selectedValues = items.filter((item) => item.isRefined).map((item) => item.value);

    return (
        <Accordion.Item button={title} index={index}>
            <Accordion.Panel px="xs" pb="sm">
                {canRefine ? (
                    <>
                        <ScrollArea maxH="sm" pr="xs">
                            <CheckboxCardGroup.Root
                                orientation="vertical"
                                size="sm"
                                value={selectedValues}
                                onChange={(values: string[]) => {
                                    const nextSet = new Set(values);

                                    items.forEach((item) => {
                                        const shouldBeRefined = nextSet.has(item.value);

                                        if (shouldBeRefined !== item.isRefined) {
                                            refine(item.value);
                                        }
                                    });
                                }}
                            >
                                {items.map((item) => (
                                    <CheckboxCardGroup.Item.Root
                                        key={`${attribute}:${item.label}`}
                                        value={item.value}
                                        flexDirection="row"
                                        alignItems="center"
                                        gap="xs"
                                        w="full"
                                    >
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
                                    </CheckboxCardGroup.Item.Root>
                                ))}
                            </CheckboxCardGroup.Root>
                        </ScrollArea>

                        {canToggleShowMore && (
                            <Button
                                size="xs"
                                variant="ghost"
                                mt="xs"
                                onClick={() => toggleShowMore()}
                            >
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
