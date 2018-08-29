select * from sys.dm_qn_subscriptions

select * from sys.dm_broker_queue_monitors

select * from sys.dm_broker_connections

select * from sys.dm_broker_forwarded_messages

select *
from sys.service_queues

SELECT id, database_id, sid, object_id, created, timeout, status  
FROM sys.dm_qn_subscriptions;  
GO  

SELECT qn.id AS query_subscription_id  
    ,it.name AS internal_table_name  
    ,it.object_id AS internal_table_id  
FROM sys.internal_tables AS it  
JOIN sys.dm_qn_subscriptions AS qn ON it.object_id = qn.object_id  
WHERE it.internal_type_desc = 'QUERY_NOTIFICATION';  
GO