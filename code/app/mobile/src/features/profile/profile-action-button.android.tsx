import { Button, Host } from "@expo/ui";
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
  return (
    <Host style={{ height: 48, width: "100%" }}>
      <Button
        disabled={disabled}
        label={label}
        modifiers={[fillMaxWidth()]}
        onPress={onPress}
        variant={variant}
      />
    </Host>
  );
}
