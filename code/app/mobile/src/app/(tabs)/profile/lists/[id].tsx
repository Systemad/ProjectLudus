import { useLocalSearchParams } from "expo-router";

import { ListDetail } from "@/features/lists";

export default function ListDetailRoute() {
  const { id } = useLocalSearchParams<{ id: string }>();
  return <ListDetail id={id} />;
}
