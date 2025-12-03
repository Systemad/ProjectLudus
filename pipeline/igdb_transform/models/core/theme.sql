{{ config(materialized='table') }}

with source as (

    select * from {{ source('igdb_raw', 'themes') }}

),

renamed as (

    select
        id,
        name,
        slug,
        url,
        checksum
        
    from source

)

select * from renamed
