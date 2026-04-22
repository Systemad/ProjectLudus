# GitHub Preparation

## Issues Found

### Hardcoded Secrets (must fix before publishing)

| File | Issue |
|------|-------|
| `frontend/apps/game-index/src/Typesense/instantsearch.ts` | Typesense API key in plain text |
| `backend/CatalogAPI/Context/AppDbContext.cs` | PostgreSQL password in plain text |

### Recommended Actions

1. **Frontend** - Add `.env.local` files for secrets:
   ```
   VITE_TYPESENSE_SEARCH_KEY=your_search_api_key
   VITE_TYPESENSE_HOST=localhost
   VITE_TYPESENSE_PORT=8108
   ```

2. **Backend** - Use `appsettings.json` or environment variables instead of hardcoded connection strings

3. **Orchestration** - The `.env` file is already gitignored (keeps IGDB tokens safe)

## Before Publishing

- [ ] Remove/rotate the hardcoded API keys above
- [ ] Add `.env.example` files as templates
- [ ] Ensure no other secrets in code comments
- [ ] Review `appsettings.json` files for accidental credentials