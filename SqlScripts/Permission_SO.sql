--create user for schema ownership
CREATE USER SqlDependencySchemaOwner WITHOUT LOGIN;
GO
--create schema for SqlDependency objects
CREATE SCHEMA SqlDependency AUTHORIZATION SqlDependencySchemaOwner;
GO

--add existing login as a minimally privileged database user with default schema SqlDependency
CREATE USER depUser WITH DEFAULT_SCHEMA = SqlDependency;

--grant user control permissions on SqlDependency schema
GRANT CONTROL ON SCHEMA::SqlDependency TO depUser;

--grant user impersonate permissions on SqlDependency schema owner
GRANT IMPERSONATE ON USER::SqlDependencySchemaOwner TO depUser;
GO

--grant database permissions needed to create and use SqlDependency objects
GRANT CREATE PROCEDURE TO depUser;
GRANT CREATE QUEUE TO depUser;
GRANT CREATE SERVICE TO depUser;
GRANT REFERENCES ON
    CONTRACT::[http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification] TO depUser;
GRANT VIEW DEFINITION TO depUser;
GRANT SELECT to depUser;
GRANT SUBSCRIBE QUERY NOTIFICATIONS TO depUser;
GRANT RECEIVE ON QueryNotificationErrorsQueue TO depUser;
GO

--grant permissions on user objects used by application
GRANT SELECT ON dbo.Services TO depUser;
GO