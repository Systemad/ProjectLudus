# Release alert notifications

## Outcome

Notify users when a game on one of their lists releases. The release schedule comes from the Catalog data, never from a user-selected hour or an imprecise placeholder date.

The work is deliberately split into two phases. Phase 1 establishes and tests the scheduling domain without requiring any Expo project, Firebase credentials, mobile permission, or device token. Phase 2 adds the real Expo transport and mobile registration.

## Data rules

The notification worker reads `igdb.release_dates` directly. A record is schedulable only when:

```text
date_format = YYYYMMDD
status      = Full Release
date        is in the next three UTC calendar days
```

The worker excludes `YYYY`, `YYYYMM`, `YYYYQ1`–`YYYYQ4`, `TBD`, null statuses, Alpha, Beta, Early Access, Offline, Cancelled, Advanced Access, and patch release statuses. This matters because the existing Catalog data contains year- and quarter-only records represented as December 31.

Catalog provides an exact calendar day but not an authoritative release hour. The initial scheduling convention is the start of that exact UTC day. This is used only for `YYYYMMDD` records.

## Database boundaries

```text
catalogdb       Catalog pipeline owns writes; notification worker has SELECT-only access.
playdb          GameIndex.Api owns writes; notification worker has SELECT-only access.
notificationsdb Notification worker owns its schema and writes.
```

The worker does not call or run inside `GameIndex.Api`.

## Phase 1 — scheduling foundation

### Projects

Add:

```text
code/app/backend/GameIndex.Notifications/
code/app/backend/GameIndex.Notifications.Tests/
```

`GameIndex.Notifications` is a .NET worker-style web host with ServiceDefaults, health endpoints, TickerQ, Catalog's scaffolded data project, and its own code-first `NotificationDbContext`.

### Notification schema

Create these entities in `notificationsdb`:

```text
ReleaseAlertEvent
  Id
  GameId
  ReleaseDayUtc
  DispatchAtUtc
  Status: Scheduled | Dispatching | Dispatched | Failed
  CreatedAtUtc
  UpdatedAtUtc

ReleaseAlertPlatform
  Id
  ReleaseAlertEventId
  SourceReleaseDateId
  PlatformId
  ReleaseRegionId

NotificationDelivery
  Id
  ReleaseAlertEventId
  UserId
  PushEndpointId
  Provider
  Status
  CreatedAtUtc
  SentAtUtc

NotificationDeliveryAttempt
  Id
  NotificationDeliveryId
  AttemptedAtUtc
  Succeeded
  ProviderReceipt
  FailureReason
```

Database invariants:

```text
UNIQUE (game_id, release_day_utc)
UNIQUE (release_alert_event_id, source_release_date_id)
UNIQUE (release_alert_event_id, user_id, push_endpoint_id, provider)
```

The first key produces one user-facing alert per game/day, while the platform rows preserve which release records caused it.

### TickerQ

Use `TickerQ` and `TickerQ.EntityFrameworkCore` with `NotificationDbContext` as TickerQ's application context. TickerQ tables use their own `ticker` schema in `notificationsdb` and are included in the worker's migrations.

Register one Phase-1 cron job:

```text
DiscoverUpcomingReleaseAlertsJob
Cron: 5 0 * * * (00:05 UTC daily)
```

The job calls `ReleaseAlertDiscovery.DiscoverAsync`:

1. Query only the eligible Catalog release records for the three-day UTC window.
2. Group rows by game and UTC release day.
3. Upsert `ReleaseAlertEvent` records and attach their source platform rows.
4. Leave events in `Scheduled` state.

The discovery path is idempotent. Running it repeatedly does not duplicate events or platform rows.

### Dispatch seam and tests

Phase 1 contains the testable delivery shape but does not enable production delivery:

```csharp
public interface IReleaseAlertRecipients
{
    IAsyncEnumerable<ReleaseAlertRecipient> GetAsync(
        long gameId,
        CancellationToken cancellationToken);
}

public interface INotificationSender
{
    Task<NotificationSendResult> SendAsync(
        ReleaseAlertRecipient recipient,
        ReleaseAlertEvent releaseAlert,
        CancellationToken cancellationToken);
}
```

The unit-test implementation records messages in memory. No Expo HTTP request, mobile token, or provider credential exists in Phase 1.

Tests cover exact-date eligibility, all excluded precision/status combinations, event grouping, idempotent upserts, and delivery deduplication with a fake recipient reader and sender.

### Aspire

Add `notificationsdb` as a connection string resource and `GameIndex.Notifications` as an AppHost project. Add the worker's EF migration resource using `AddEFMigrations` and make the worker wait for it. Do not start the worker as part of this implementation task.

The local AppHost configuration gains a `ConnectionStrings:notificationsdb` value. Deployment receives a separate `notificationsdb` connection and read-only `catalogdb` / `playdb` credentials.

## Phase 2 — real Expo delivery

### Mobile preparation

1. Create/configure the EAS project ID.
2. Configure Android Firebase Cloud Messaging v1 credentials.
3. Add `expo-notifications` and `expo-constants`.
4. Add the `expo-notifications` config plugin.
5. Build and install an Android development build.

### Play data and API

Add `NotificationPreference` and `PushEndpoint` to the Play code-first model. A user owns their preferences and registered endpoints. The worker only reads them.

```text
GET/PUT    /api/me/notification-preferences
GET         /api/me/push-endpoints
PUT         /api/me/push-endpoints/{token}
DELETE      /api/me/push-endpoints/{id}
```

Use:

```csharp
public enum NotificationProvider
{
    ExpoPush = 1,
}
```

### Worker delivery

Implement `PlayReleaseAlertRecipients` as a read-only, keyset-paged query over Play list membership, enabled preferences, and active endpoints.

At `DispatchAtUtc`, a TickerQ time ticker runs `DispatchReleaseAlertJob`. It reads recipients in batches, inserts deduplicated delivery rows, sends via `ExpoPushSender`, stores receipts, and deactivates invalid Expo tokens.

Recipients are selected at dispatch time, not during daily discovery. A user who adds the game before release can be notified; a user who removes it before release is skipped. Large releases are streamed in batches rather than pre-creating subscription rows for every user.

### End-to-end test

1. Register a device token from the development build.
2. Verify it is stored in Play.
3. Create a near-future exact/full-release test event.
4. Confirm one notification-delivery row and one Expo receipt.
5. Tap the notification and confirm the deep link opens the game page.

## Validation

Phase 1 validates with worker/test builds and unit tests only. It does not start Aspire, Expo, Android, or a device.

Phase 2 validates first with Expo's direct push-notification tool, then through a scheduled release event.
