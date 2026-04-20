{{ config(materialized="table") }}

select distinct e.id as event_id, t.value as event_network_id
from {{ ref("stg_events__event_networks") }} t
inner join {{ ref("stg_events") }} ev on t._dlt_parent_id = ev._dlt_id
inner join {{ ref("mart_events") }} e on ev.id = e.id
inner join {{ ref("mart_event_networks") }} men on t.value = men.id
where t.value is not null
