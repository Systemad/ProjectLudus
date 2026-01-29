from enum import Enum


# TODO: USE DBT CODEGEN TO GENERATE SQL, THEN CLEANUPS
# TODO: USE ALL IGDB FIELDS
class Endpoints(str, Enum):
    """IGDB Endpoints"""

    BASE_URL = "https://api.igdb.com/v4/"

    # Age ratings
    AGE_RATINGS = "age_ratings"
    AGE_RATING_CATEGORIES = "age_rating_categories"
    AGE_RATING_CONTENT_DESCRIPTION_TYPES = "age_rating_content_description_types"
    AGE_RATING_CONTENT_DESCRIPTIONS_V2 = "age_rating_content_descriptions_v2"
    AGE_RATING_ORGANIZATIONS = "age_rating_organizations"

    # artworks
    ARTWORKS = "artworks"
    ALTERNATIVE_NAMES = "alternative_names"
    ARTWORK_TYPES = "artwork_types"

    # Characters
    CHARACTERS = "characters"
    CHARACTER_GENDERS = "character_genders"
    CHARACTER_MUG_SHOTS = "character_mug_shots"
    CHARACTER_SPECIES = "character_species"

    # Collections
    COLLECTIONS = "collections"
    COLLECTION_MEMBERSHIPS = "collection_memberships"
    COLLECTION_MEMBERSHIP_TYPES = "collection_membership_types"
    COLLECTION_RELATIONS = "collection_relations"
    COLLECTION_RELATION_TYPES = "collection_relation_types"
    COLLECTION_TYPES = "collection_types"

    # companies
    COMPANIES = "companies"
    COMPANY_LOGOS = "company_logos"
    COMPANY_STATUSES = "company_statuses"
    COMPANY_WEBSITES = "company_websites"

    COVERS = "covers"
    DATE_FORMATS = "date_formats"
    EVENTS = "events"
    EVENT_LOGOS = "event_logos"
    EVENT_NETWORKS = "event_networks"
    EXTERNAL_GAMES = "external_games"

    EXTERNAL_GAME_SOURCES = "external_game_sources"
    FRANCHISES = "franchises"
    GAMES = "games"
    GAME_ENGINES = "game_engines"
    GAME_ENGINE_LOGOS = "game_engine_logos"

    GAME_LOCALIZATIONS = "game_localizations"
    GAME_MODES = "game_modes"
    GAME_RELEASE_FORMATS = "game_release_formats"
    GAME_STATUSES = "game_statuses"
    GAME_TIME_TO_BEATS = "game_time_to_beats"
    GAME_TYPES = "game_types"

    GAME_VERSIONS = "game_versions"
    GAME_VERSION_FEATURES = "game_version_features"
    GAME_VERSION_FEATURE_VALUES = "game_version_feature_values"
    GAME_VIDEOS = "game_videos"
    GENRES = "genres"
    INVOLVED_COMPANIES = "involved_companies"

    KEYWORDS = "keywords"
    LANGUAGES = "languages"
    LANGUAGE_SUPPORTS = "language_supports"
    LANGUAGE_SUPPORT_TYPES = "language_support_types"
    MULTIPLAYER_MODES = "multiplayer_modes"
    NETWORK_TYPES = "network_types"

    PLATFORMS = "platforms"
    PLATFORM_TYPES = "platform_types"
    PLATFORM_FAMILY = "platform_families"
    PLATFORM_VERSIONS = "platform_versions"
    PLATFORM_LOGOS = "platform_logos"
    PLATFORM_VERSION_COMPANIES = "platform_version_companies"
    PLATFORM_VERSION_RELEASE_DATES = "platform_version_release_dates"
    PLATFORM_WEBSITES = "platform_websites"

    PLAYER_PERSPECTIVES = "player_perspectives"
    POPULARITY_PRIMITIVES = "popularity_primitives"
    POPULARITY_TYPES = "popularity_types"
    REGIONS = "regions"
    RELEASE_DATES = "release_dates"
    RELEASE_DATE_REGIONS = "release_date_regions"

    RELEASE_DATE_STATUSES = "release_date_statuses"
    SCREENSHOTS = "screenshots"
    THEMES = "themes"
    WEBSITES = "websites"
    WEBSITE_TYPES = "website_types"
