CREATE TABLE [sync4partner].[SyncTableOperational] (
    [Name]                         [sysname]     NOT NULL,
    [PackageName]                  VARCHAR (125) DEFAULT ('EmptyPlaceHolder.dtsx') NOT NULL,
    [UpdateConditionForSplit]      VARCHAR (MAX) DEFAULT ('False') NOT NULL,
    [UpdateExpressonForOLECommand] VARCHAR (MAX) DEFAULT ('') NOT NULL,
    [PartnerID]                    VARCHAR (MAX) DEFAULT ('') NOT NULL,
    [CountStr]                     VARCHAR (MAX) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SyncTableOperational] PRIMARY KEY CLUSTERED ([Name] ASC),
    CONSTRAINT [FK_SyncTableOperational_SyncTable] FOREIGN KEY ([Name]) REFERENCES [sync4partner].[SyncTable] ([Name])
);





