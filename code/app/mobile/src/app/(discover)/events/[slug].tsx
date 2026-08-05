import { useLocalSearchParams } from "expo-router";

import { EventDetail } from "@/features/event-detail";

export default function EventRoute() {
  const { slug } = useLocalSearchParams<{ slug: string }>();
  return <EventDetail slug={slug} />;
}
