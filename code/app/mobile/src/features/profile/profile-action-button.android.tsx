import { Host } from "@expo/ui";
import { Button, OutlinedButton, Text } from "@expo/ui/jetpack-compose";
import { fillMaxWidth } from "@expo/ui/jetpack-compose/modifiers";

type ProfileActionButtonProps = {
  disabled: boolean;
  label: string;
  onPress: () => void;
  variant: "filled" | "outlined";
};

export function ProfileActionButton({
  disabled,
  label,
  onPress,
  variant,
}: ProfileActionButtonProps) {
  const ActionButton = variant === "filled" ? Button : OutlinedButton;

  return (
    <Host style={{ height: 48, width: "100%" }}>
      <ActionButton enabled={!disabled} modifiers={[fillMaxWidth()]} onClick={onPress}>
        <Text>{label}</Text>
      </ActionButton>
    </Host>
  );
}
