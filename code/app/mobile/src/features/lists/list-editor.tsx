import { Button, Column, Host, Text, TextInput, useNativeState } from "@expo/ui";
import { useQueryClient } from "@tanstack/react-query";
import { router } from "expo-router";
import { useState } from "react";

import { getApiMeListsQueryKey, usePostApiMeLists } from "@/gen/play-api";
import { useAppTheme } from "@/hooks/use-app-theme";
import { posthog } from "@/lib/posthog";

export function ListEditor() {
  const colors = useAppTheme();
  const queryClient = useQueryClient();
  const create = usePostApiMeLists();
  const [name, setName] = useState("");
  const inputState = useNativeState(name);
  const canSubmit = Boolean(name.trim()) && !create.isPending;

  const submit = async () => {
    const value = name.trim();
    if (!value) return;
    await create.mutateAsync({ body: { name: value, description: null, visibility: "Private" } });
    posthog?.capture("game_list_created", { visibility: "private" });
    await queryClient.invalidateQueries({ queryKey: getApiMeListsQueryKey() });
    router.back();
  };

  return (
    <Host style={{ flex: 1, backgroundColor: colors.background }}>
      <Column spacing={16} style={{ padding: 20 }}>
        <Text textStyle={{ fontSize: 28, fontWeight: "800" }}>Create a list</Text>
        <TextInput
          autoFocus
          maxLength={120}
          onChangeText={(value) => {
            setName(value);
          }}
          placeholder="Weekend games"
          placeholderTextColor={colors.textMuted}
          style={{
            borderColor: colors.outline,
            borderRadius: 14,
            borderWidth: 1,
            height: 52,
            paddingHorizontal: 14,
          }}
          textStyle={{ fontSize: 16 }}
          value={inputState}
        />
        <Button
          disabled={!canSubmit}
          onPress={() => void submit()}
          label="Create list"
          style={{ borderRadius: 24, height: 48, opacity: canSubmit ? 1 : 0.5 }}
        />
      </Column>
    </Host>
  );
}
