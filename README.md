# Project Ludus

Game-index is a data-first game discovery platform. IGDB and Steam data flows through the catalog pipeline into PostgreSQL, Backend.API, and the web and Android clients.

## Repository

~~~text
code/
├── app/
│   ├── AppHost.cs
│   ├── backend/                  # Backend.API, Catalog, Play, Notifications
│   ├── frontend/apps/game-index/ # web client
│   └── mobile/                   # Android Expo client
└── catalog/                      # dlt, dbt, grate and scaffold inputs
~~~

## Data flow

~~~text
IGDB/Steam → dlt/dbt → PostgreSQL → Backend.API → Kubb hooks → clients
~~~

Read [AGENTS.md](AGENTS.md) for repository rules. The scoped READMEs and AGENTS files document setup for each application area.

