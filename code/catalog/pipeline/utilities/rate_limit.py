import logging

import niquests

logger = logging.getLogger(__name__)

STEAM_SAFE_LIMITER = niquests.TokenBucketLimiter(rate=3.0, capacity=3)
APPDETAILS_LIMITER = niquests.TokenBucketLimiter(rate=0.5, capacity=1)


def log_steam_response(response, *args, **kwargs):
    if response.status_code == 429:
        logger.warning("Steam rate limited (429) on %s", response.url)
    elif response.status_code == 403:
        logger.warning("Steam block detected (403) on %s", response.url)


def log_steam_retry(response, *args, **kwargs):
    if response.status_code == 429:
        logger.warning("Steam API rate limited on %s, retrying...", response.url)


def create_steam_session(limiter=None, include_retry_hook=True):
    retry = niquests.RetryConfiguration(
        total=2, status_forcelist=[429], respect_retry_after_header=True
    )
    hooks = [log_steam_response]
    if include_retry_hook:
        hooks.insert(0, log_steam_retry)
    session = niquests.Session(hooks=limiter, retries=retry)
    session.hooks["response"] = hooks
    return session
