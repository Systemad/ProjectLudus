import { Button, Host } from "@expo/ui";

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
    <Host matchContents style={{ height: 48, width: "100%" }}>
      <Button disabled={disabled} label={label} onPress={onPress} variant={variant} />
    </Host>
  );
}
