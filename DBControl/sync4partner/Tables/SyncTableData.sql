CREATE TABLE [sync4partner].[SyncTableData] (
    [Name]      [sysname] NOT NULL,
    [PartnerID] INT       NOT NULL,
    CONSTRAINT [PK_SyncTableData] PRIMARY KEY CLUSTERED ([Name] ASC),
    CONSTRAINT [FK_SyncTableData_SyncTable] FOREIGN KEY ([Name]) REFERENCES [sync4partner].[SyncTable] ([Name]),
    CONSTRAINT [FK_SyncTableData_TaskConfig] FOREIGN KEY ([PartnerID]) REFERENCES [sync4partner].[TaskConfig] ([PartnerID])
);

