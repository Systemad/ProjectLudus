import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { requestFn } from '@openapi-qraft/react';
import { RouterProvider, createRouter } from '@tanstack/react-router'
import { createAPIClient } from './api'
import { routeTree } from './routeTree.gen'

import {
  QueryClient,
  QueryClientProvider,
} from '@tanstack/react-query'

const queryClient = new QueryClient()
export const api = createAPIClient({
  requestFn,
  queryClient,
  baseUrl: "",
});
const router = createRouter({ routeTree })

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  </StrictMode>,
)
