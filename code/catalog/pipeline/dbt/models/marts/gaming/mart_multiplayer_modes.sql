{{ config(
    materialized="table",
    post_hook=[
        "create index if not exists idx_multiplayer_modes_game on {{ this }} (game)",
        "create index if not exists idx_multiplayer_modes_platform on {{ this }} (platform)",
        "create index if not exists idx_multiplayer_modes_coop_game on {{ this }} (game) where campaigncoop is true or offlinecoop is true or onlinecoop is true or lancoop is true",
    ],
) }}

with

formatted as (

    select
        id,
        campaigncoop,
        dropin,
        game,
        lancoop,
        offlinecoop,
        onlinecoop,
        platform,
        splitscreen,
        checksum,
        offlinemax,
        onlinemax,
        onlinecoopmax,
        offlinecoopmax

    from {{ ref("stg_multiplayer_modes") }}

)

select *
from formatted
where
    game is not null
    and platform is not null

