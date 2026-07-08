import { Button } from "@astryxdesign/core/Button";
import { Text } from "@astryxdesign/core/Text";
import { useRefinementList } from "react-instantsearch";

type SearchFacetFilterGroupProps = {
    title: string;
    attribute: string;
    index: number;
};
export function SearchFacetFilterGroup({ title, attribute }: SearchFacetFilterGroupProps) {
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
        <div style={{marginBottom: "0.75rem"}}>
            <details>
                <summary style={{cursor: "pointer", fontWeight: 600, marginBottom: "0.25rem", padding: "0.25rem 0"}}>
                    {title}
                </summary>
                <div style={{ padding: "0.25rem 0" }}>
                {canRefine ? (
                    <>
                        <div style={{ display: "flex", flexDirection: "column", gap: "0.25rem" }}>
                            {items.map((item) => {
                                const isChecked = currentValues.includes(item.value);
                                return (
                                    <label
                                        key={item.value}
                                        style={{
                                            display: "flex",
                                            alignItems: "center",
                                            gap: "0.5rem",
                                            padding: "0.25rem 0.5rem",
                                            borderRadius: "var(--radius-sm)",
                                            cursor: "pointer",
                                            fontSize: "0.875rem",
                                            background: isChecked ? "var(--bg-subtle)" : "transparent",
                                        }}
                                    >
                                        <input
                                            type="checkbox"
                                            checked={isChecked}
                                            onChange={() => {
                                                const next = isChecked
                                                    ? currentValues.filter((v) => v !== item.value)
                                                    : [...currentValues, item.value];
                                                handleChange(next);
                                            }}
                                            style={{ accentColor: "var(--color-primary)" }}
                                        />
                                        <Text
                                            as="span"
                                            style={{
                                                fontSize: "0.875rem",
                                                WebkitLineClamp: 1,
                                                overflow: "hidden",
                                                textOverflow: "ellipsis",
                                                minWidth: 0,
                                                flex: "1",
                                            }}
                                        >
                                            {item.label}
                                        </Text>
                                        <Text
                                            as="span"
                                            color="secondary"
                                            style={{ fontSize: "0.75rem", whiteSpace: "nowrap", marginLeft: "auto" }}
                                        >
                                            {item.count}
                                        </Text>
                                    </label>
                                );
                            })}
                        </div>

                        {canToggleShowMore && (
                            <Button size="sm" variant="ghost" label={isShowingMore ? "Show less" : "Show more"} onClick={toggleShowMore} />
                        )}
                    </>
                ) : (
                    <Text color="secondary" style={{fontSize: "0.875rem"}}>
                        No options found
                    </Text>
                )}
                </div>
            </details>
        </div>
    );
}
