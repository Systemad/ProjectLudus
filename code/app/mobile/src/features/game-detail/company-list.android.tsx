import { Host } from "@expo/ui";
import {
  AnimatedVisibility,
  Card,
  Column,
  EnterTransition,
  ExitTransition,
  ListItem,
  RNHostView,
  Row,
  Text,
} from "@expo/ui/jetpack-compose";
import {
  animateContentSize,
  clickable,
  fillMaxWidth,
  padding,
  weight,
} from "@expo/ui/jetpack-compose/modifiers";
import { type Href, useRouter } from "expo-router";
import { Image } from "expo-image";
import { useState } from "react";

import { getIgdbImageUrl } from "@/entities/game/game-image";
import type { InvolvedCompanyDto } from "@/gen/types/InvolvedCompanyDto";
import { useAppTheme } from "@/hooks/use-app-theme";

const COLLAPSED_COMPANY_COUNT = 3;

export function CompanyList({ companies }: { companies: InvolvedCompanyDto[] }) {
  const colors = useAppTheme();
  const router = useRouter();
  const [expanded, setExpanded] = useState(false);
  const remainingCount = Math.max(companies.length - COLLAPSED_COMPANY_COUNT, 0);
  const visibleCompanies = companies.slice(0, COLLAPSED_COMPANY_COUNT);
  const hiddenCompanies = companies.slice(COLLAPSED_COMPANY_COUNT);

  const renderCompany = (company: InvolvedCompanyDto) => {
    const href = {
      pathname: "../companies/[slug]",
      params: { slug: String(company.companyId) },
    } satisfies Href;

    return (
      <ListItem
        key={String(company.id)}
        colors={{
          containerColor: "transparent",
          contentColor: colors.text,
          supportingContentColor: colors.textMuted,
        }}
        modifiers={[clickable(() => router.push(href))]}
      >
        <ListItem.LeadingContent>
          <RNHostView matchContents>
            <Image
              source={getIgdbImageUrl(company.companyLogoImageId, "logo_med")}
              style={{ width: 40, height: 40, borderRadius: 8 }}
              contentFit="contain"
            />
          </RNHostView>
        </ListItem.LeadingContent>
        <ListItem.HeadlineContent>
          <Text style={{ typography: "titleSmall", fontWeight: "700" }}>{company.companyName}</Text>
        </ListItem.HeadlineContent>
        <ListItem.SupportingContent>
          <Text style={{ typography: "bodySmall" }}>{getCompanyRole(company)}</Text>
        </ListItem.SupportingContent>
      </ListItem>
    );
  };

  return (
    <Host matchContents={{ vertical: true }} seedColor={colors.primary} style={{ width: "100%" }}>
      <Card
        border={{ width: 1, color: colors.outline }}
        colors={{ containerColor: colors.surfaceHigh, contentColor: colors.text }}
        elevation={0}
        modifiers={[fillMaxWidth()]}
      >
        <Column modifiers={[fillMaxWidth(), animateContentSize()]}>
          {visibleCompanies.map(renderCompany)}
          <AnimatedVisibility
            enterTransition={EnterTransition.fadeIn().plus(EnterTransition.expandVertically())}
            exitTransition={ExitTransition.fadeOut().plus(ExitTransition.shrinkVertically())}
            visible={expanded}
          >
            <Column modifiers={[fillMaxWidth()]}>{hiddenCompanies.map(renderCompany)}</Column>
          </AnimatedVisibility>
          {remainingCount > 0 ? (
            <Row
              verticalAlignment="center"
              modifiers={[
                fillMaxWidth(),
                clickable(() => setExpanded((value) => !value)),
                padding(16, 12, 16, 12),
              ]}
            >
              <Text modifiers={[weight(1)]} style={{ typography: "labelLarge", fontWeight: "700" }}>
                {expanded ? "Show fewer" : `+ ${remainingCount} more`}
              </Text>
              <Text style={{ typography: "titleMedium", fontWeight: "700" }}>
                {expanded ? "−" : "+"}
              </Text>
            </Row>
          ) : null}
        </Column>
      </Card>
    </Host>
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
