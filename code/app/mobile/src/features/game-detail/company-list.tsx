import { type Href, useRouter } from "expo-router";
import { useState } from "react";
import { Image, Pressable, StyleSheet, Text, View } from "react-native";

import { getIgdbImageUrl } from "@/entities/game/game-image";
import type { InvolvedCompanyDto } from "@/gen/types/InvolvedCompanyDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { commonStyles } from "@/shared/ui/common-styles";

const COLLAPSED_COMPANY_COUNT = 3;

export function CompanyList({ companies }: { companies: InvolvedCompanyDto[] }) {
  const colors = useAppTheme();
  const router = useRouter();
  const [expanded, setExpanded] = useState(false);
  const remainingCount = Math.max(companies.length - COLLAPSED_COMPANY_COUNT, 0);
  const visibleCompanies = expanded ? companies : companies.slice(0, COLLAPSED_COMPANY_COUNT);

  return (
    <View
      style={[
        commonStyles.surfaceCard,
        styles.card,
        { backgroundColor: colors.surfaceHigh, borderColor: colors.outline },
      ]}
    >
      {visibleCompanies.map((company) => {
        const href = {
          pathname: "../companies/[slug]",
          params: { slug: String(company.companyId) },
        } satisfies Href;

        return (
          <Pressable
            key={String(company.id)}
            onPress={() => router.push(href)}
            style={[commonStyles.row, styles.row]}
          >
            <Image
              source={{ uri: getIgdbImageUrl(company.companyLogoImageId, "logo_med") }}
              style={styles.logo}
            />
            <View style={styles.text}>
              <Text style={[styles.name, { color: colors.text }]}>{company.companyName}</Text>
              <Text style={[styles.role, { color: colors.textMuted }]}>
                {getCompanyRole(company)}
              </Text>
            </View>
          </Pressable>
        );
      })}
      {remainingCount > 0 ? (
        <Pressable onPress={() => setExpanded((value) => !value)} style={styles.toggle}>
          <Text style={[styles.name, { color: colors.text }]}>
            {expanded ? "Show fewer" : `+ ${remainingCount} more`}
          </Text>
          <Text style={[styles.symbol, { color: colors.text }]}>{expanded ? "−" : "+"}</Text>
        </Pressable>
      ) : null}
    </View>
  );
}

function getCompanyRole(company: InvolvedCompanyDto) {
  const roles = [
    company.developer ? "Developer" : undefined,
    company.publisher ? "Publisher" : undefined,
    company.porting ? "Porting" : undefined,
    company.supporting ? "Supporting" : undefined,
  ].filter((role): role is string => role !== undefined);

  return roles.length > 0 ? roles.join(" · ") : "Contributor";
}

const styles = StyleSheet.create({
  card: {
    borderWidth: StyleSheet.hairlineWidth,
  },
  row: {
    minHeight: 64,
    paddingHorizontal: 16,
    paddingVertical: 10,
  },
  logo: {
    width: 40,
    height: 40,
    resizeMode: "contain",
    borderRadius: 8,
  },
  text: {
    flex: 1,
    gap: 2,
  },
  name: {
    fontSize: 14,
    fontWeight: "700",
  },
  role: {
    fontSize: 12,
  },
  toggle: {
    minHeight: 48,
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    paddingHorizontal: 16,
  },
  symbol: {
    fontSize: 20,
    fontWeight: "700",
  },
});
