import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/games/$game')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/games/$game"!</div>
}
