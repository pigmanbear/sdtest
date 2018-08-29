CREATE QUEUE dbo.PersonsChangeMessages;  

CREATE SERVICE PersonsChangeNotifications  
  ON QUEUE dbo.PersonsChangeMessages  
([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]);