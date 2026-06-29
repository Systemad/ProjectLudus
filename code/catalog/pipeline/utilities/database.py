import dlt
import psycopg


def get_connection():
    return psycopg.connect(
        host=dlt.secrets.get("destination.postgres.credentials.host"),
        port=dlt.secrets.get("destination.postgres.credentials.port"),
        dbname=dlt.secrets.get("destination.postgres.credentials.database"),
        user=dlt.secrets.get("destination.postgres.credentials.username"),
        password=dlt.secrets.get("destination.postgres.credentials.password"),
        connect_timeout=30,
    )
