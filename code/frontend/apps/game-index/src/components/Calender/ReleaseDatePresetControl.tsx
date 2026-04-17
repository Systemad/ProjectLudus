import { SegmentedControl } from "ui";
import type { ReleaseDatePreset } from "@src/utils/dateUtils";

type ReleaseDatePresetControlProps = {
    value: ReleaseDatePreset;
    onChange: (value: ReleaseDatePreset) => void;
};

export function ReleaseDatePresetControl({ value, onChange }: ReleaseDatePresetControlProps) {
    return (
        <SegmentedControl.Root
            value={value}
            onChange={(next) => onChange(next as ReleaseDatePreset)}
            size="sm"
            w="full"
        >
            <SegmentedControl.Item value="week">Week</SegmentedControl.Item>
            <SegmentedControl.Item value="month">Month</SegmentedControl.Item>
        </SegmentedControl.Root>
    );
}
