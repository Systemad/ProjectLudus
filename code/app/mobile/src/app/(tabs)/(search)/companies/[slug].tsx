import { useLocalSearchParams } from "expo-router";

import { CompanyDetail } from "@/features/company-detail";

export default function CompanyRoute() {
  const { slug } = useLocalSearchParams<{ slug: string }>();
  return <CompanyDetail slug={slug} />;
}
