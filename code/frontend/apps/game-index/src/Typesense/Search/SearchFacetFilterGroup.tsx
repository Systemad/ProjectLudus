import { Accordion, Button, CheckboxCardGroup, Text, Flex } from "ui";
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

    const currentValues = items.filter((item) => item.isRefined).map((item) => item.value);

    const handleChange = (nextValues: string[]) => {
        const toToggle = [
            ...nextValues.filter((v) => !currentValues.includes(v)),
            ...currentValues.filter((v) => !nextValues.includes(v)),
        ];
        for (const v of toToggle) refine(v);
    };

    return (
        <Accordion.Item button={title} index={index}>
            <Accordion.Panel px="xs" pb="sm">
                {canRefine ? (
                    <>
                        <CheckboxCardGroup.Root
                            orientation="vertical"
                            size="sm"
                            value={currentValues}
                            variant="subtle"
                            onChange={handleChange}
                        >
                            {items.map((item) => (
                                <CheckboxCardGroup.Item.Root key={item.value} value={item.value}>
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
