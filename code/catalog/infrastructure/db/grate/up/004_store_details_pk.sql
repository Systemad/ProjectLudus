alter table steam_raw.store_details drop constraint store_details_pkey;
alter table steam_raw.store_details add primary key (game_id);
