import { Host } from "@expo/ui";
import {
  AssistChip,
  Box,
  Column,
  HorizontalDivider,
  IconButton,
  RNHostView,
  Row,
  Spacer,
  Surface,
  Text,
  TextButton,
} from "@expo/ui/jetpack-compose";
import {
  align,
  clickable,
  clip,
  fillMaxSize,
  fillMaxWidth,
  height,
  offset,
  padding,
  paddingAll,
  verticalScroll,
  weight,
  width,
} from "@expo/ui/jetpack-compose/modifiers";
import type { ModifierConfig } from "@expo/ui/jetpack-compose/modifiers";
import { useLocalSearchParams, useRouter } from "expo-router";
import { Image } from "expo-image";
import { Share } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import type { ReactNode } from "react";
import { useState } from "react";

import { getIgdbImageUrl } from "@/entities/game/game-image";
import { GameListActions } from "@/features/lists/game-list-actions";
import { ContentState, type ContentStateStatus } from "@/shared/ui/content-state";
import { GameFactGrid } from "./game-fact-grid";
import { GameLinkList } from "./game-link-list";
import { GameScreenshotGallery } from "./game-screenshot-gallery";
import { GameSummaryCard } from "./game-summary-card";
import { RelatedGameRail } from "./related-game-rail";
import { SteamChart } from "./steam-chart";
import { SteamSummary } from "./steam-summary";
import { useGameDetailData } from "./use-game-detail-data";

const TABS = ["overview", "media", "links"] as const;
type GameDetailTab = (typeof TABS)[number];
const TAB_LABELS: Record<GameDetailTab, string> = {
  overview: "Overview",
  media: "Media",
  links: "Links",
};

export function GameDetail() {
  const { slug } = useLocalSearchParams<{ slug: string }>();
  const gameId = String(slug);
  const router = useRouter();
  const game = useGameDetailData(gameId);
  const [activeTab, setActiveTab] = useState<GameDetailTab>("overview");

  if (!game.hero || !game.overview) {
    return (
      <ContentState
        status={game.isCorePending ? "loading" : game.hasCoreError ? "error" : "empty"}
        fullScreen
        loading={{ label: "Loading game…" }}
        error={{ onRetry: () => void game.retryCore() }}
        empty={{ title: "Game not found", message: "The API did not return this game." }}
      />
    );
  }

  const title = game.hero.name;
  const summary = game.hero.summary ?? game.overview.storyline ?? "No summary is available yet.";
  const steamAppId = game.steam?.steamAppId;
  const coverUrl = getIgdbImageUrl(game.hero.cover, "cover_big", true);
  const heroUrl = steamAppId
    ? `https://cdn.akamai.steamstatic.com/steam/apps/${steamAppId}/header.jpg`
    : coverUrl;
  const firstGenre = game.hero.genres[0]?.name;
  const facts = [
    { label: "Platforms", values: game.hero.platforms.map((item) => item.name) },
    { label: "Genres", values: game.hero.genres.map((item) => item.name) },
    { label: "Game modes", values: game.hero.gameModes.map((item) => item.name) },
    { label: "Player perspective", values: game.hero.playerPerspectives.map((item) => item.name) },
    { label: "Themes", values: game.hero.themes.map((item) => item.name) },
    { label: "Companies", values: game.hero.companies.map((item) => item.companyName) },
  ].filter((fact) => fact.values.length > 0);

  return (
    <SafeAreaView edges={["top"]} style={{ flex: 1 }}>
      <Host useViewportSizeMeasurement style={{ flex: 1 }}>
        <Surface modifiers={[fillMaxSize()]}>
          <Column modifiers={[fillMaxSize(), verticalScroll()]}>
            <TopBar
              title={title}
              onBack={router.back}
              onOverflow={() => void Share.share({ message: `Open this game in Ludus: ${gameId}` })}
            />
            <TabBar activeTab={activeTab} onTabChange={setActiveTab} />
            <Column modifiers={[fillMaxWidth(), padding(16, 8, 16, 112)]}>
              <Box modifiers={[fillMaxWidth(), height(342)]}>
                <HeroBanner
                  modifier={align("topStart")}
                  imageUrl={heroUrl}
                  chips={[
                    game.hero.firstReleaseDate?.slice(0, 4) ?? "TBA",
                    steamAppId ? "Steam" : "Catalog",
                    firstGenre ?? "Game",
                  ]}
                />
                <Record
                  modifier={[align("topStart"), offset(0, 192)]}
                  imageUrl={coverUrl}
                  title={title}
                  recordMeta={`${game.hero.companies[0]?.companyName ?? "Unknown studio"} · ${firstGenre?.toLowerCase() ?? "game"}`}
                />
              </Box>
              {activeTab === "overview" ? (
                <>
                  <GameListActions gameId={gameId} />
                  <GameSummaryCard summary={summary} />
                  <SteamSummary steam={game.steam} reviews={game.reviews} pricing={game.pricing} />
                  <GameFactGrid facts={facts} />
                  <RNHostView matchContents>
                    <SteamChart gameId={gameId} />
                  </RNHostView>
                  {game.similar.length > 0 || game.isSimilarPending || game.hasSimilarError ? (
                    <NativeSection title="Related games">
                      <NativeSectionState
                        status={getStatus(
                          game.isSimilarPending,
                          game.hasSimilarError,
                          game.similar.length === 0,
                        )}
                        onRetry={() => void game.retrySimilar()}
                        errorMessage="Related games could not be loaded."
                      >
                        <RelatedGameRail
                          games={game.similar}
                          getHref={(relatedGameId) => ({
                            pathname: "./[slug]",
                            params: { slug: relatedGameId },
                          })}
                        />
                      </NativeSectionState>
                    </NativeSection>
                  ) : null}
                </>
              ) : activeTab === "media" ? (
                <NativeSection title="Media">
                  <NativeSectionState
                    status={getStatus(
                      game.isMediaPending,
                      game.hasMediaError,
                      game.screenshots.length === 0,
                    )}
                    onRetry={() => void game.retryMedia()}
                    errorMessage="Media could not be loaded."
                    emptyMessage="This game has no screenshots yet."
                  >
                    <GameScreenshotGallery screenshotIds={game.screenshots} />
                  </NativeSectionState>
                </NativeSection>
              ) : (
                <NativeSection title="Links">
                  <NativeSectionState
                    status={getStatus(
                      game.isLinksPending,
                      game.hasLinksError,
                      game.websites.length === 0,
                    )}
                    onRetry={() => void game.retryLinks()}
                    errorMessage="Links could not be loaded."
                    emptyMessage="This game has no external links yet."
                  >
                    <GameLinkList websites={game.websites} />
                  </NativeSectionState>
                </NativeSection>
              )}
            </Column>
          </Column>
        </Surface>
      </Host>
    </SafeAreaView>
  );
}

function TopBar({
  title,
  onBack,
  onOverflow,
}: {
  title: string;
  onBack: () => void;
  onOverflow: () => void;
}) {
  return (
    <Row modifiers={[fillMaxWidth(), height(57), padding(4, 0, 4, 0)]} verticalAlignment="center">
      <IconButton onClick={onBack}>
        <Text style={{ typography: "headlineSmall" }}>‹</Text>
      </IconButton>
      <Text
        modifiers={[weight(1)]}
        style={{ typography: "titleLarge", textAlign: "center" }}
        maxLines={1}
        overflow="ellipsis"
      >
        {title}
      </Text>
      <IconButton onClick={onOverflow}>
        <Text style={{ typography: "headlineSmall" }}>⋮</Text>
      </IconButton>
    </Row>
  );
}

function TabBar({
  activeTab,
  onTabChange,
}: {
  activeTab: GameDetailTab;
  onTabChange: (tab: GameDetailTab) => void;
}) {
  return (
    <Column modifiers={[fillMaxWidth(), padding(16, 0, 16, 0)]}>
      <Row modifiers={[fillMaxWidth()]}>
        {TABS.map((tab) => (
          <Column
            key={tab}
            modifiers={[weight(1), clickable(() => onTabChange(tab))]}
            horizontalAlignment="center"
          >
            <Text style={{ typography: "titleSmall" }}>{TAB_LABELS[tab]}</Text>
            {activeTab === tab ? (
              <HorizontalDivider
                thickness={3}
                modifiers={[fillMaxWidth(), padding(20, 10, 20, 0)]}
              />
            ) : (
              <Spacer modifiers={[height(13)]} />
            )}
          </Column>
        ))}
      </Row>
      <HorizontalDivider />
    </Column>
  );
}

function HeroBanner({
  imageUrl,
  chips,
  modifier,
}: {
  imageUrl?: string;
  chips: string[];
  modifier?: ModifierConfig;
}) {
  return (
    <Box
      modifiers={[
        fillMaxWidth(),
        height(222),
        clip({ type: "roundedCorner", radius: 16 }),
        ...(modifier ? [modifier] : []),
      ]}
    >
      {imageUrl ? (
        <RNHostView
          modifiers={[fillMaxSize(), clip({ type: "roundedCorner", radius: 16 })]}
          matchContents={false}
        >
          <Image source={imageUrl} style={{ width: "100%", height: "100%" }} contentFit="cover" />
        </RNHostView>
      ) : null}
      <Row
        modifiers={[align("bottomEnd"), padding(8, 8, 8, 8)]}
        horizontalArrangement={{ spacedBy: 8 }}
      >
        {chips.map((chip) => (
          <AssistChip key={chip}>
            <AssistChip.Label>
              <Text style={{ typography: "labelMedium" }}>{chip}</Text>
            </AssistChip.Label>
          </AssistChip>
        ))}
      </Row>
    </Box>
  );
}

function Record({
  imageUrl,
  title,
  recordMeta,
  modifier,
}: {
  imageUrl?: string;
  title: string;
  recordMeta: string;
  modifier?: ModifierConfig[];
}) {
  return (
    <Box modifiers={[fillMaxWidth(), height(150), ...(modifier ?? [])]}>
      {imageUrl ? (
        <RNHostView
          modifiers={[
            align("topStart"),
            offset(12, -30),
            width(96),
            height(137),
            clip({ type: "roundedCorner", radius: 12 }),
          ]}
          matchContents
        >
          <Image source={imageUrl} style={{ width: 96, height: 137 }} contentFit="cover" />
        </RNHostView>
      ) : (
        <Surface modifiers={[align("topStart"), offset(12, -30), width(96), height(137)]} />
      )}
      <Column
        modifiers={[fillMaxWidth(), align("bottomStart"), padding(122, 0, 0, 17)]}
        verticalArrangement={{ spacedBy: 4 }}
      >
        <Text style={{ typography: "headlineSmall" }} maxLines={2} overflow="ellipsis">
          {title}
        </Text>
        <Text style={{ typography: "bodyMedium" }} maxLines={1} overflow="ellipsis">
          {recordMeta}
        </Text>
      </Column>
    </Box>
  );
}

function NativeSection({ title, children }: { title: string; children: ReactNode }) {
  return (
    <Column
      modifiers={[fillMaxWidth(), padding(0, 14, 0, 0)]}
      verticalArrangement={{ spacedBy: 9 }}
    >
      <Text style={{ typography: "titleLarge" }}>{title}</Text>
      {children}
    </Column>
  );
}

function NativeSectionState({
  status,
  children,
  emptyMessage = "There is nothing to show yet.",
  errorMessage = "Please try again.",
  onRetry,
}: {
  status: ContentStateStatus;
  children: ReactNode;
  emptyMessage?: string;
  errorMessage?: string;
  onRetry: () => void;
}) {
  if (status === "ready") return children;
  return (
    <Column modifiers={[fillMaxWidth(), paddingAll(16)]} verticalArrangement={{ spacedBy: 8 }}>
      <Text style={{ typography: "bodyMedium" }}>
        {status === "loading" ? "Loading…" : status === "error" ? errorMessage : emptyMessage}
      </Text>
      {status === "error" ? (
        <TextButton onClick={onRetry}>
          <Text>Try again</Text>
        </TextButton>
      ) : null}
    </Column>
  );
}

function getStatus(isLoading: boolean, isError: boolean, isEmpty: boolean): ContentStateStatus {
  if (isLoading) return "loading";
  if (isError) return "error";
  if (isEmpty) return "empty";
  return "ready";
}
