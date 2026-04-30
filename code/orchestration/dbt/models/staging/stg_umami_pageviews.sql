with
    source as (
        select * from {{ source("igdb_source", "umami_pageviews_by_path") }}
    ),
    renamed as (
        select
            x as url_path,
            y as pageviews,
            regexp_replace(x, '/games/(\d+).*', '\1') as game_id,
            _dlt_load_id,
            _dlt_id
        from source
        where x like '/games/%'
    )
select * from renamed
