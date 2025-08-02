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
import { useDebouncedState } from "@mantine/hooks";
import { useMemo } from "react";
import React from "react";

type BaseFilterItem = {
    id: number;
    name: string;
};
type FilterAccordionProps<T extends BaseFilterItem> = {
    title: string;
    items: T[];
    selectedItems: number[];
    onChange?: (items: number[]) => void;
};
export function FilterAccordion<T extends BaseFilterItem>({
    title,
    items,
    selectedItems,
    onChange,
}: FilterAccordionProps<T>) {
    const [value, setValue] = useDebouncedState<string>("", 200, {
        leading: true,
    });

    const filteredItems = useMemo(() => {
        const lower = value.toLocaleLowerCase();
        return items.filter((item) => item.name.toLowerCase().includes(lower));
    }, [value, items]);

    const MemoCheckboxItem = React.memo(
        ({ id, name }: { id: string; name: string }) => (
            <Checkbox key={id} value={id}>
                {name}
            </Checkbox>
        )
    );

    return (
        <AccordionItem rounded="xl" label={title}>
            <InputGroup mt="xs">
                <InputLeftElement>
                    <MagnifyingGlassIcon />
                </InputLeftElement>
                <Input
                    onChange={(ev) => setValue(ev.target.value)}
                    borderWidth={"thin"}
                    variant={"filled"}
                    rounded="xl"
                    placeholder="Search"
                />
            </InputGroup>
            <ScrollArea h="2xs" innerProps={{ as: VStack, gap: "md" }}>
                {filteredItems && (
                    <>
                        <CheckboxGroup
                            value={selectedItems.map((id) => id.toString())}
                            onChange={(valueStrings) => {
                                if (onChange) {
                                    onChange(
                                        valueStrings.map((v) => Number(v))
                                    );
                                }
                            }}
                        >
                            {filteredItems.map((filter) => (
                                <Checkbox
                                    key={filter.id}
                                    value={filter.id.toString()}
                                >
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
//                             value={selectedIds.map((id) => id.toString())}
