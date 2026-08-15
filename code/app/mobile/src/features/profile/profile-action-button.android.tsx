import { Host } from "@expo/ui";
import { Button, Text } from "@expo/ui/jetpack-compose";
import { fillMaxWidth } from "@expo/ui/jetpack-compose/modifiers";

import { useAppTheme } from "@/hooks/use-app-theme";

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
  const colors = useAppTheme();

  return (
    <Host style={{ height: 48, width: "100%" }}>
      <Button
        colors={{
          containerColor: (variant === "filled" ? colors.primary : colors.surface) as string,
          contentColor: (variant === "filled" ? colors.onPrimary : colors.text) as string,
        }}
        enabled={!disabled}
        modifiers={[fillMaxWidth()]}
        onClick={onPress}
      >
        <Text color={(variant === "filled" ? colors.onPrimary : colors.text) as string}>
          {label}
        </Text>
      </Button>
    </Host>
  );
}
