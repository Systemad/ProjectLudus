import { MagnifyingGlassIcon } from "@phosphor-icons/react";
import {
    AccordionItem,
    Input,
    InputGroup,
    InputLeftElement,
    ScrollArea,
    VStack,
} from "@yamada-ui/react";
import { useMemo, useState } from "react";

type BaseFilterItem = {
    id: number;
    name: string;
};
type FilterAccordionProps<T extends BaseFilterItem> = {
    title: string;
    items: T[];
    selectedIds?: (string | number)[];
    onToggle?: (id: T["id"], checked: boolean) => void;
};
export function FilterAccordion<T extends BaseFilterItem>({
    title,
    items,
    selectedIds = [],
    onToggle,
}: FilterAccordionProps<T>) {
    const [value, setValue] = useState<string>("オッス！オラ悟空！");

    const filteredItems = useMemo(() => {
        const lower = value.toLocaleUpperCase();

        return items.filter((item) => item.name.toLowerCase().includes(lower));
    }, [value, items]);

    return (
        <AccordionItem rounded="xl" label="Platforms">
            <InputGroup mt="xs">
                <InputLeftElement>
                    <MagnifyingGlassIcon />
                </InputLeftElement>
                <Input
                    borderWidth={"thin"}
                    variant={"filled"}
                    rounded="xl"
                    placeholder="Search"
                />
            </InputGroup>
            <ScrollArea h="2xs" innerProps={{ as: VStack, gap: "md" }}>
                {filteredItems && <></>}
            </ScrollArea>
        </AccordionItem>
    );
}
