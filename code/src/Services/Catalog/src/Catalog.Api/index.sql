--- DROP INDEX search_idx; ---

CREATE INDEX CONCURRENTLY search_idx ON public.games
    USING bm25 (id, name, release_date, game_type, platforms, game_engines, genres, themes, rating, rating_count,
    total_rating, total_rating_count)
    WITH (
    key_field='id',
    text_fields = '{
      "name": {
        "fast": true,
        "tokenizer": {
          "type": "default"
        }
      }
    }',
    numeric_fields = '{
      "rating": { "fast": true },
      "rating_count": { "fast": true },
      "total_rating": { "fast": true },
      "total_rating_count": { "fast": true }
    }',
    datetime_fields = '{
      "release_date": { "fast": true }
    }'
    );