with
    source as (select * from {{ ref("stg_game_version_feature_values") }}),

    renamed as (

        select id, game, game_feature, included_feature, checksum, note from source

    )

select *
from renamed
