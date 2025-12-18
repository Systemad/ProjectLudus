with source as (

    select * from {{ source('igdb_raw_v2', 'platform_versions') }}

),

renamed as (

    select
        id,
        created_at,
        updated_at,
        name,
        platform_logo,
        platform_version_release_dates,
        slug,
        summary,
        url,
        checksum,
        _dlt_load_id,
        _dlt_id,
        companies,
        cpu,
        media,
        memory,
        output,
        resolutions,
        sound,
        connectivity,
        storage,
        graphics,
        os,
        main_manufacturer

    from source

)

select * from renamed

