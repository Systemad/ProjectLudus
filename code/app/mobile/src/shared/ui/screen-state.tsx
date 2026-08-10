import { InlineState } from "@/shared/ui/inline-state";

export function LoadingState({ label = "Loading catalog…" }: { label?: string }) {
  return <InlineState fullScreen loading message={label} />;
}

export function EmptyState({ title, message }: { title: string; message: string }) {
  return <InlineState fullScreen title={title} message={message} />;
}

export function ErrorState({
  onRetry,
  title = "Couldn’t load this page",
  message = "The Game-index API could not be reached.",
  retryLabel = "Try again",
}: {
  onRetry: () => void;
  title?: string;
  message?: string;
  retryLabel?: string;
}) {
  return (
    <InlineState
      fullScreen
      title={title}
      message={message}
      onRetry={onRetry}
      retryLabel={retryLabel}
    />
  );
}
