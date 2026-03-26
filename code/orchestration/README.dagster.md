# Dagster compose — dev vs prod

Why there are two compose files

-   `docker-compose.dagster.dev.yml`: development compose that joins Aspire's persistent dev network (external name). Use this when running Dagster alongside Aspire in development so containers can resolve each other by name.
-   `docker-compose.dagster.prod.yml`: production/publish compose where the `aspire` network is created by compose (driver: bridge). This matches `aspire publish` expectations.

Discover the Aspire dev network name

1. List networks and find the Aspire entry:

```bash
docker network ls
```

2. Inspect the candidate network to confirm contents (containers, subnet):

```bash
docker network inspect <network-name>
# example from this environment:
docker network inspect aspire-persistent-network-d0f4c22e-AppHost
```

Run Dagster in development (join Aspire dev network)

Make sure your environment variables (IGDB_CLIENT_ID, IGDB_ACCESS_TOKEN, etc.) are set or provided via an `.env` file.

```bash
docker compose -f docker-compose.dagster.dev.yml up -d
docker compose -f docker-compose.dagster.dev.yml logs -f
```

If you need to attach a running container to the Aspire network manually:

```bash
docker network connect aspire-persistent-network-d0f4c22e-AppHost <container-name-or-id>
```

Run Dagster in production/publish

Use the prod compose which creates its own `aspire` network:

```bash
docker compose -f docker-compose.dagster.prod.yml up -d
```

Notes and security

-   The dev compose uses an external network by name; replace `aspire-persistent-network-d0f4c22e-AppHost` with the network name observed on your machine.
-   Bind-mounting `/var/run/docker.sock` is convenient for local dev but is a security risk in production — avoid it unless necessary.
-   You can add `networks: { aspire: { aliases: ["my-service-alias"] } }` to services for stable DNS names across stacks.

## Exporting Aspire Postgres credentials (dev)

If Aspire generates your Postgres service and credentials, avoid manual copy/paste where possible. Two practical options:

-   Quick/manual (one-time): locate the Aspire-generated compose file (for example `aspire-generated.yml` or the file produced by `aspire publish`) and copy the Postgres `environment:` entries (keys like `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB`, `POSTGRES_HOST`, `POSTGRES_PORT`) into a local `.env.aspire` file in `orchestration/`.

    Example `.env.aspire`:

    POSTGRES_USER=example_user
    POSTGRES_PASSWORD=supersecret
    POSTGRES_DB=projectludus
    POSTGRES_HOST=catalog-primary
    POSTGRES_PORT=5432

    Make sure `.env.aspire` is in `.gitignore` and stored securely.

-   Using the rendered compose (safer to inspect): render the merged compose YAML and copy the env block for the Postgres service into `.env.aspire`:

    ```bash
    docker compose -f aspire-generated.yml config > rendered.yml
    # open rendered.yml and copy the environment: block for the Postgres service
    # paste into .env.aspire as KEY=VALUE pairs
    ```

Notes:

-   The exact service name for Postgres may vary (e.g., `postgres`, `db`, `catalog-primary`). Search `rendered.yml` for the Postgres service and its `environment:` section.
-   Keep `.env.aspire` out of source control and restrict access.

## CI / Production recommendation

For production deploys prefer CI injection or a secrets manager instead of shipping plaintext env files. Typical workflow:

-   Store Postgres credentials in your CI secrets or a secret manager (Vault, AWS Secrets Manager, Azure KeyVault).
-   At deploy time, have the CI pipeline render `docker-compose.dagster.prod.yml` with the secrets injected, or use Docker secrets so services read credentials securely at runtime.
