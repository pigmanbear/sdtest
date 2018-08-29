declare @conversation uniqueidentifier

while exists (select 1 from sys.conversation_endpoints)

begin

set @conversation = (select top 1 conversation_handle from sys.conversation_endpoints  )

end conversation @conversation with cleanup

end

