import { LinearGradient } from "expo-linear-gradient";
import { Image } from "expo-image";
import { Stack, useLocalSearchParams } from "expo-router";
import { Share2 } from "lucide-react-native";
import { useRef, useState, type ReactNode } from "react";
import {
  ActivityIndicator,
  Pressable,
  ScrollView,
  Share,
  StyleSheet,
  Text,
  View,
} from "react-native";

import { getIgdbImageUrl } from "@/entities/game/game-image";
import { useAppTheme } from "@/hooks/use-app-theme";
import {
  ContentState,
  getContentStateStatus,
  type ContentStateStatus,
} from "@/shared/ui/content-state";
import { getGameDetailHref } from "@/utils/game-routes";

import { GameListActions } from "../lists/game-list-actions";
import { GameFactGrid } from "./game-fact-grid";
import { GameLinkList } from "./game-link-list";
import { GameScreenshotGallery } from "./game-screenshot-gallery";
import { GameSummaryText } from "./game-summary-text";
import { GameCarousel } from "@/entities/game/game-carousel";
import { SteamChart } from "./steam-chart";
import { SteamSummary } from "./steam-summary";
import { useGameDetailViewModel } from "./use-game-detail-view-model";

const TABS = [
  { value: "overview", label: "Overview" },
  { value: "media", label: "Media" },
  { value: "links", label: "Links" },
] as const;
type GameDetailTab = (typeof TABS)[number]["value"];

export function GameDetail() {
  const colors = useAppTheme();
  const { slug } = useLocalSearchParams<{ slug: string }>();
  const gameId = String(slug);
  const [activeTab, setActiveTab] = useState<GameDetailTab>("overview");
  const game = useGameDetailViewModel(gameId, { activeTab });
  const dismissSteamTooltipRef = useRef<() => void>(() => {});

  const title = game.hero?.name ?? "Game";
  const shareGame = () => void Share.share({ message: `Open this game in Ludus: ${gameId}` });

  return (
    <View style={[styles.screen, { backgroundColor: colors.background }]}>
      <Stack.Screen
        options={{
          title,
          headerShown: true,
          headerTransparent: false,
          headerStyle: { backgroundColor: colors.background },
          headerShadowVisible: false,
          headerTintColor: colors.text,
          headerRight: () => (
            <Pressable
              accessibilityRole="button"
              accessibilityLabel="Share game"
              hitSlop={10}
              onPress={shareGame}
              style={({ pressed }) => ({ opacity: pressed ? 0.6 : 1, padding: 4 })}
            >
              <Share2 color={colors.text} size={21} strokeWidth={2.2} />
            </Pressable>
          ),
        }}
      />
      {!game.hero || !game.overview ? (
        <View style={styles.loadingScreen}>
          <ContentState
            status={game.isCorePending ? "loading" : game.hasCoreError ? "error" : "empty"}
            fullScreen
            loading={{ label: "Loading game…" }}
            error={{ onRetry: () => void game.retryCore() }}
            empty={{ title: "Game not found", message: "The API did not return this game." }}
          />
        </View>
      ) : (
        <ScrollView
          contentInsetAdjustmentBehavior="automatic"
          onTouchStart={() => dismissSteamTooltipRef.current()}
          showsVerticalScrollIndicator={false}
          contentContainerStyle={{
            gap: 24,
            paddingBottom: 24,
            paddingHorizontal: 16,
            paddingTop: 12,
          }}
        >
          <MediaRecord game={game} />
          <DetailTabs activeTab={activeTab} onChange={setActiveTab} />
          <GameListActions gameId={gameId} />
          {activeTab === "overview" ? (
            <OverviewContent
              game={game}
              gameId={gameId}
              onSteamTooltipDismiss={(dismiss) => {
                dismissSteamTooltipRef.current = dismiss;
              }}
            />
          ) : (
            <SecondaryContent activeTab={activeTab} game={game} />
          )}
        </ScrollView>
      )}
    </View>
  );
}

type GameDetailViewModel = ReturnType<typeof useGameDetailViewModel>;

function OverviewContent({
  game,
  gameId,
  onSteamTooltipDismiss,
}: {
  game: GameDetailViewModel;
  gameId: string;
  onSteamTooltipDismiss: (dismiss: () => void) => void;
}) {
  const summary = game.hero?.summary ?? game.overview?.storyline ?? "No summary is available yet.";
  const facts = [
    { label: "Platforms", values: game.hero?.platforms.map((item) => item.name) ?? [] },
    { label: "Genres", values: game.hero?.genres.map((item) => item.name) ?? [] },
    { label: "Game modes", values: game.hero?.gameModes.map((item) => item.name) ?? [] },
    {
      label: "Player perspective",
      values: game.hero?.playerPerspectives.map((item) => item.name) ?? [],
    },
    { label: "Themes", values: game.hero?.themes.map((item) => item.name) ?? [] },
    {
      label: "Companies",
      values: game.hero?.companies.map((item) => item.companyName) ?? [],
    },
  ].filter((fact) => fact.values.length > 0);

  return (
    <View style={styles.sectionList}>
      <GameSummaryText summary={summary} />
      <SteamSummary steam={game.steam} reviews={game.reviews} pricing={game.pricing} />
      <GameFactGrid facts={facts} />
      <SteamChart gameId={gameId} onTooltipDismiss={onSteamTooltipDismiss} />
      {game.similar.length > 0 || game.isSimilarPending || game.hasSimilarError ? (
        <NativeSection title="Related games">
          <NativeSectionState
            status={getContentStateStatus(
              game.isSimilarPending,
              game.hasSimilarError,
              game.similar.length === 0,
            )}
            onRetry={() => void game.retrySimilar()}
            errorMessage="Related games could not be loaded."
          >
            <GameCarousel
              games={game.similar}
              getHref={(relatedGame) => getGameDetailHref(relatedGame.id)}
            />
          </NativeSectionState>
        </NativeSection>
      ) : null}
    </View>
  );
}

function SecondaryContent({
  activeTab,
  game,
}: {
  activeTab: Exclude<GameDetailTab, "overview">;
  game: GameDetailViewModel;
}) {
  return (
    <View style={styles.sectionList}>
      {activeTab === "media" ? (
        <NativeSection title="Media">
          <NativeSectionState
            status={getContentStateStatus(
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
            status={getContentStateStatus(
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
    </View>
  );
}

function MediaRecord({ game }: { game: GameDetailViewModel }) {
  const colors = useAppTheme();
  const hero = game.hero;
  if (!hero) return null;

  const coverUrl = getIgdbImageUrl(hero.cover ?? hero.coverUrl, "cover_big", true);
  const steamAppId = game.steam?.steamAppId;
  const heroUrl =
    game.steam?.headerUrl ??
    (steamAppId
      ? `https://cdn.akamai.steamstatic.com/steam/apps/${steamAppId}/header.jpg`
      : coverUrl);
  const firstGenre = hero.genres[0]?.name;
  const chips = [
    hero.firstReleaseDate?.slice(0, 4) ?? "TBA",
    steamAppId ? "Steam" : "Catalog",
    firstGenre ?? "Game",
  ];

  return (
    <View>
      <View style={styles.hero}>
        <Artwork source={heroUrl} label={`${hero.name} artwork`} style={StyleSheet.absoluteFill} />
        <LinearGradient
          colors={["transparent", "rgba(0, 0, 0, 0.72)"]}
          pointerEvents="none"
          style={StyleSheet.absoluteFill}
        />
        <View style={styles.chips}>
          {chips.map((chip) => (
            <View key={chip} style={[styles.chip, { backgroundColor: "rgba(0, 0, 0, 0.58)" }]}>
              <Text style={styles.chipText}>{chip}</Text>
            </View>
          ))}
        </View>
      </View>
      <View style={styles.recordDetails}>
        <View style={styles.coverWrap}>
          <Artwork source={coverUrl} label={`${hero.name} cover`} style={styles.cover} />
        </View>
        <View style={styles.recordCopy}>
          <Text selectable numberOfLines={2} style={[styles.recordTitle, { color: colors.text }]}>
            {hero.name}
          </Text>
          <Text numberOfLines={1} style={[styles.recordMeta, { color: colors.textMuted }]}>
            {hero.companies[0]?.companyName ?? "Unknown studio"} ·{" "}
            {firstGenre?.toLowerCase() ?? "game"}
          </Text>
        </View>
      </View>
    </View>
  );
}

function Artwork({ source, label, style }: { source?: string; label: string; style: object }) {
  const colors = useAppTheme();
  const [state, setState] = useState<"loading" | "loaded" | "error">(source ? "loading" : "error");

  return (
    <View style={[styles.artwork, style, { backgroundColor: colors.primaryContainer }]}>
      {source && state !== "error" ? (
        <Image
          accessibilityLabel={label}
          source={source}
          style={StyleSheet.absoluteFill}
          contentFit="cover"
          transition={180}
          onLoad={() => setState("loaded")}
          onError={() => setState("error")}
        />
      ) : null}
      {state === "loading" ? (
        <View style={styles.artworkState}>
          <ActivityIndicator color={colors.onPrimaryContainer} />
        </View>
      ) : null}
      {state === "error" ? (
        <View style={styles.artworkState}>
          <Text style={[styles.fallbackText, { color: colors.onPrimaryContainer }]}>
            No artwork
          </Text>
        </View>
      ) : null}
    </View>
  );
}

function DetailTabs({
  activeTab,
  onChange,
}: {
  activeTab: GameDetailTab;
  onChange: (tab: GameDetailTab) => void;
}) {
  const colors = useAppTheme();

  return (
    <View style={[styles.tabs, { borderBottomColor: colors.outline }]}>
      {TABS.map((tab) => {
        const selected = tab.value === activeTab;
        return (
          <Pressable
            key={tab.value}
            accessibilityRole="tab"
            accessibilityState={{ selected }}
            onPress={() => onChange(tab.value)}
            style={({ pressed }) => [styles.tab, { opacity: pressed ? 0.65 : 1 }]}
          >
            <Text style={[styles.tabText, { color: selected ? colors.primary : colors.textMuted }]}>
              {tab.label}
            </Text>
            <View
              style={[styles.tabIndicator, selected ? { backgroundColor: colors.primary } : null]}
            />
          </Pressable>
        );
      })}
    </View>
  );
}

function NativeSection({ title, children }: { title: string; children: ReactNode }) {
  const colors = useAppTheme();
  return (
    <View style={styles.section}>
      <Text style={[styles.sectionTitle, { color: colors.text }]}>{title}</Text>
      {children}
    </View>
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
    <ContentState
      status={status}
      loading={{ label: "Loading…" }}
      error={{ message: errorMessage, onRetry }}
      empty={{ message: emptyMessage }}
    />
  );
}

const styles = StyleSheet.create({
  screen: {
    flex: 1,
  },
  loadingScreen: {
    flex: 1,
    paddingTop: 72,
  },
  sectionList: {
    gap: 24,
  },
  section: {
    gap: 10,
  },
  sectionTitle: {
    fontSize: 19,
    fontWeight: "800",
  },
  hero: {
    aspectRatio: 16 / 8.2,
    borderRadius: 20,
    borderCurve: "continuous",
    overflow: "hidden",
  },
  artwork: {
    overflow: "hidden",
  },
  artworkState: {
    alignItems: "center",
    ...StyleSheet.absoluteFill,
    justifyContent: "center",
  },
  fallbackText: {
    fontSize: 12,
    fontWeight: "800",
  },
  chips: {
    bottom: 10,
    flexDirection: "row",
    flexWrap: "wrap",
    gap: 6,
    left: 10,
    position: "absolute",
    right: 10,
  },
  chip: {
    borderRadius: 999,
    paddingHorizontal: 9,
    paddingVertical: 5,
  },
  chipText: {
    color: "#FFFFFF",
    fontSize: 11,
    fontWeight: "800",
  },
  recordDetails: {
    alignItems: "flex-end",
    flexDirection: "row",
    gap: 14,
    marginTop: -42,
    paddingHorizontal: 12,
  },
  coverWrap: {
    borderRadius: 14,
    borderCurve: "continuous",
    elevation: 5,
    height: 138,
    overflow: "hidden",
    shadowColor: "#000000",
    shadowOpacity: 0.25,
    shadowRadius: 8,
    width: 96,
  },
  cover: {
    height: "100%",
    width: "100%",
  },
  recordCopy: {
    flex: 1,
    gap: 5,
    paddingBottom: 7,
  },
  recordTitle: {
    fontSize: 25,
    fontWeight: "900",
    lineHeight: 30,
  },
  recordMeta: {
    fontSize: 13,
    fontWeight: "700",
  },
  tabs: {
    borderBottomWidth: StyleSheet.hairlineWidth,
    flexDirection: "row",
  },
  tab: {
    alignItems: "center",
    flex: 1,
    gap: 8,
    minHeight: 44,
    justifyContent: "flex-end",
  },
  tabText: {
    fontSize: 14,
    fontWeight: "800",
  },
  tabIndicator: {
    borderRadius: 999,
    height: 3,
    width: 48,
  },
});
