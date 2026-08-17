import type { PostHog } from "posthog-react-native";

type DisabledPostHog = Pick<PostHog, "capture" | "identify" | "reset">;

export const posthog: DisabledPostHog | null = null;
