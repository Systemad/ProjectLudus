import { Select } from "ui";

const RATING_OPTIONS = [
    { label: "Any", value: "0" },
    { label: "60+", value: "60" },
    { label: "70+", value: "70" },
    { label: "80+", value: "80" },
    { label: "90+", value: "90" },
];

type MinimumRatingControlProps = {
    value: number;
    onChange: (value: number) => void;
};

export function MinimumRatingControl({ value, onChange }: MinimumRatingControlProps) {
    return (
        <Select.Root
            size="sm"
            value={String(value)}
            onChange={(next) => onChange(Number(next))}
            items={RATING_OPTIONS}
            w="full"
        />
    );
}
