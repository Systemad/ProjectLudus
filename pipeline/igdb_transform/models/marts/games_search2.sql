{{ config(materialized="table", cluster_by=["release_year"]) }}

with
    game_genres as (
        select 
            gg.game_id, 
            jsonb_agg(jsonb_build_object('id', gen.id, 'name', gen.name)) as genres,
            -- Also create a plain text string for pg_search to index easily
            string_agg(gen.name, ' ') as genres_text 
        from {{ ref("bridge_game_genre") }} gg
        join {{ ref("mart_genres") }} gen on gg.genre_id = gen.id
        group by gg.game_id
    )
-- ... (repeat for themes/modes)

select
    g.id,
    g.name,
    g.summary,
    coalesce(gg.genres, '[]'::jsonb) as genres,
    -- Concatenate name, summary, and genres_text into a search vector
    to_tsvector('english', g.name || ' ' || coalesce(g.summary, '') || ' ' || coalesce(gg.genres_text, '')) as search_vector
from {{ ref("mart_games") }} g
left join game_genres gg on g.id = gg.game_id