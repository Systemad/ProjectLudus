import logging
from collections import defaultdict

from psycopg import sql
from utilities.database import get_connection
from utilities.endpoints import get_default

logger = logging.getLogger(__name__)

VALID_ENDPOINTS = set(get_default())


def delete_from_table(conn, endpoint, ids):
    table = sql.Identifier("public", endpoint)
    ids = list(ids)
    conn.execute(
        sql.SQL("DELETE FROM {} m WHERE m.id = ANY(%s)").format(table),
        (ids,),
    )


def mark_events_processed(conn):
    conn.execute(
        """
        UPDATE igdb_source.webhook_events
        SET processed = true
        WHERE event_type = 'delete' AND not processed
        """
    )


def run():
    conn = get_connection()
    try:
        events = conn.execute(
            """
            SELECT endpoint, entity_id
            FROM igdb_source.webhook_events
            WHERE event_type = 'delete' AND not processed
            """
        ).fetchall()

        if not events:
            logger.info("No unprocessed delete events found.")
            return

        by_endpoint = defaultdict(set)
        for ep, eid in events:
            if ep in VALID_ENDPOINTS:
                by_endpoint[ep].add(eid)

        if not by_endpoint:
            logger.warning("No valid endpoint delete events found.")
            return

        with conn.transaction():
            for ep, ids in by_endpoint.items():
                delete_from_table(conn, ep, ids)
            mark_events_processed(conn)

        logger.info("Processed %d endpoint(s).", len(by_endpoint))
    finally:
        conn.close()
