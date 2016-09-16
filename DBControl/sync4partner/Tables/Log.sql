CREATE TABLE [sync4partner].[Log] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [SyncName]           NVARCHAR (50)  NOT NULL,
    [TaskConfigId]       INT            NULL,
    [SyncStartDate]      DATETIME2 (7)  NOT NULL,
    [SyncEndDate]        DATETIME2 (7)  NULL,
    [Description]        NVARCHAR (MAX) NULL,
    [Duration_ms]        BIGINT         NULL,
    [Status]             CHAR (10)      NOT NULL,
    [SyncRowCountSource] INT            NULL,
    [SyncRowCountInsert] INT            NULL,
    [SyncRowCountUpdate] INT            NULL,
    [SyncRowCountDelete] INT            NULL,
    [Source]             NVARCHAR (125) NULL,
    [Destination]        NVARCHAR (125) NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC)
);





