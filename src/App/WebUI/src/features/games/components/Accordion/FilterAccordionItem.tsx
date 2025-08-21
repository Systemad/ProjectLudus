import { MagnifyingGlassIcon } from "@phosphor-icons/react";
import {
    AccordionItem,
    Checkbox,
    CheckboxGroup,
    Input,
    InputGroup,
    InputLeftElement,
    ScrollArea,
    VStack,
} from "@yamada-ui/react";
import { useMemo, useState } from "react";
import type { FilterItem } from "~/gen";

type FilterAccordionProps = {
    title: string;
    items: FilterItem[];
    selected: number[];
    onChange: (value: number[]) => void;
};
export function FilterAccordion({
    title,
    items,
    selected,
    onChange,
}: FilterAccordionProps) {
    const [value, setValue] = useState("");

    const filteredItems = useMemo(() => {
        const lower = value.toLocaleLowerCase();
        return items.filter((item) => item.name.toLowerCase().includes(lower));
    }, [items, value]);

    return (
        <AccordionItem rounded="xl" label={title}>
            <InputGroup mt="xs">
                <InputLeftElement>
                    <MagnifyingGlassIcon />
                </InputLeftElement>
                <Input
                    placeholder="Search"
                    value={value}
                    onChange={(e) => setValue(e.currentTarget.value)}
                />
            </InputGroup>

            <ScrollArea
                mt="2"
                h="2xs"
                innerProps={{
                    as: VStack,
                    gap: "md",
                }}
            >
                {filteredItems && (
                    <>
                        <CheckboxGroup value={selected} onChange={onChange}>
                            {filteredItems.map((filter) => (
                                <Checkbox key={filter.id} value={filter.id}>
                                    {filter.name}
                                </Checkbox>
                            ))}
                        </CheckboxGroup>
                    </>
                )}
            </ScrollArea>
        </AccordionItem>
    );
}
