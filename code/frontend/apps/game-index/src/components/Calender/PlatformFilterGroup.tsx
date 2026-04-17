import { Accordion, CheckboxCardGroup, Flex, Text } from "ui";
import { useInstantSearch, useRefinementList } from "react-instantsearch";

type PlatformGroup = {
    id: "xbox" | "playstation" | "pc";
    label: string;
    keywords: string[];
};

const PLATFORM_GROUPS: PlatformGroup[] = [
    { id: "xbox", label: "Xbox", keywords: ["xbox"] },
    { id: "playstation", label: "PlayStation", keywords: ["playstation", "ps4", "ps5"] },
    { id: "pc", label: "PC", keywords: ["pc", "windows", "linux", "mac"] },
];

type PlatformFilterGroupProps = {
    title: string;
    index: number;
};

export function PlatformFilterGroup({ title, index }: PlatformFilterGroupProps) {
    const { items } = useRefinementList({
        attribute: "platforms",
        limit: 120,
        showMore: false,
        sortBy: ["isRefined:desc", "count:desc", "name:asc"],
    });
    const { indexUiState, setIndexUiState } = useInstantSearch();
    const selectedPlatformValues =
        (indexUiState.refinementList?.platforms as string[] | undefined) ?? [];

    const groups = PLATFORM_GROUPS.map((group) => {
        const matches = items.filter((item) => {
            const label = item.label.toLowerCase();
            return group.keywords.some((keyword) => label.includes(keyword));
        });

        const values = matches.map((item) => item.value);
        const count = matches.reduce((total, current) => total + current.count, 0);
        const isRefined = values.some((value) => selectedPlatformValues.includes(value));

        return {
            ...group,
            values,
            count,
            isRefined,
        };
    });

    const selectedValues = groups.filter((group) => group.isRefined).map((group) => group.id);

    const handleChange = (nextValues: string[]) => {
        const nextSet = new Set(nextValues);
        const nextPlatformValues = Array.from(
            new Set(
                groups.filter((group) => nextSet.has(group.id)).flatMap((group) => group.values),
            ),
        );

        setIndexUiState((prev) => {
            const nextRefinementList = { ...prev.refinementList };

            if (nextPlatformValues.length > 0) {
                nextRefinementList.platforms = nextPlatformValues;
            } else {
                delete nextRefinementList.platforms;
            }

            return {
                ...prev,
                refinementList: nextRefinementList,
            };
        });
    };

    return (
        <Accordion.Item button={title} index={index}>
            <Accordion.Panel px="xs" pb="sm">
                <CheckboxCardGroup.Root
                    orientation="vertical"
                    size="sm"
                    value={selectedValues}
                    onChange={handleChange}
                >
                    {groups.map((group) => (
                        <CheckboxCardGroup.Item.Root key={group.id} value={group.id}>
                            <Flex align="center" gap="xs">
                                <Text as="span" fontSize="sm" flex="1" minW={0}>
                                    {group.label}
                                </Text>
                                <Text as="span" fontSize="xs">
                                    {group.count}
                                </Text>
                            </Flex>
                        </CheckboxCardGroup.Item.Root>
                    ))}
                </CheckboxCardGroup.Root>
            </Accordion.Panel>
        </Accordion.Item>
    );
}
