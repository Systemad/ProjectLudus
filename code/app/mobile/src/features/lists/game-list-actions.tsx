import {
  Card,
  Checkbox,
  Column,
  ListItem,
  ModalBottomSheet,
  OutlinedButton,
  Row,
  Text,
} from "@expo/ui/jetpack-compose";
import {
  clickable,
  fillMaxWidth,
  height,
  padding,
  paddingAll,
  weight,
} from "@expo/ui/jetpack-compose/modifiers";
import { useState } from "react";

import { useGameDetailActions } from "./use-game-detail-actions";

export function GameListActions({ gameId }: { gameId: string }) {
  const actions = useGameDetailActions(gameId);
  const [sheetVisible, setSheetVisible] = useState(false);
  const [trackingPlayers, setTrackingPlayers] = useState(false);

  const openListSheet = () => {
    if (actions.signedIn) setSheetVisible(true);
    else actions.goToProfile();
  };

  return (
    <Column modifiers={[fillMaxWidth(), padding(0, 0, 0, 9)]}>
      <Row modifiers={[fillMaxWidth()]} horizontalArrangement={{ spacedBy: 9 }}>
        <ActionSurface
          icon="♡"
          label={actions.membership.data?.isWishlisted ? "Wishlisted" : "Wishlist"}
          onClick={() => {
            if (!actions.pending) void actions.toggleWishlist();
          }}
        />
        <ActionSurface icon="☷" label="Save to list" onClick={openListSheet} />
        <ActionSurface
          icon={trackingPlayers ? "◉" : "◎"}
          label={trackingPlayers ? "Tracking" : "Track players"}
          onClick={() => setTrackingPlayers((value) => !value)}
        />
        <ActionSurface icon="↗" label="Share" onClick={actions.share} />
      </Row>
      {sheetVisible ? (
        <ModalBottomSheet onDismissRequest={() => setSheetVisible(false)}>
          <Column
            modifiers={[fillMaxWidth(), paddingAll(20)]}
            verticalArrangement={{ spacedBy: 8 }}
          >
            <Text style={{ typography: "titleLarge" }}>Save to a list</Text>
            {actions.lists.data?.map((list) => {
              const selected = actions.membership.data?.listIds.includes(list.id) ?? false;
              return (
                <ListItem
                  key={list.id}
                  modifiers={[fillMaxWidth(), clickable(() => void actions.toggleList(list.id))]}
                >
                  <ListItem.HeadlineContent>
                    <Text>{list.name}</Text>
                  </ListItem.HeadlineContent>
                  <ListItem.SupportingContent>
                    <Text>{`${list.itemCount} games`}</Text>
                  </ListItem.SupportingContent>
                  <ListItem.TrailingContent>
                    <Checkbox
                      enabled={!actions.pending}
                      onCheckedChange={() => void actions.toggleList(list.id)}
                      value={selected}
                    />
                  </ListItem.TrailingContent>
                </ListItem>
              );
            })}
            <OutlinedButton onClick={actions.createList}>
              <Text>Create new list</Text>
            </OutlinedButton>
          </Column>
        </ModalBottomSheet>
      ) : null}
    </Column>
  );
}

function ActionSurface({
  icon,
  label,
  onClick,
}: {
  icon: string;
  label: string;
  onClick: () => void;
}) {
  return (
    <Card modifiers={[weight(1), height(100), clickable(onClick)]}>
      <Column
        modifiers={[fillMaxWidth(), paddingAll(10)]}
        horizontalAlignment="center"
        verticalArrangement={{ spacedBy: 6 }}
      >
        <Text style={{ typography: "titleLarge" }}>{icon}</Text>
        <Text
          style={{ typography: "labelSmall", textAlign: "center" }}
          maxLines={2}
          overflow="ellipsis"
        >
          {label}
        </Text>
      </Column>
    </Card>
  );
}
