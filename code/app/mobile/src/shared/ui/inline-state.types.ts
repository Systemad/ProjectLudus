export type InlineStateProps = {
  loading?: boolean;
  title?: string;
  message?: string;
  onRetry?: () => void;
  retryLabel?: string;
  minHeight?: number;
  fullScreen?: boolean;
};
