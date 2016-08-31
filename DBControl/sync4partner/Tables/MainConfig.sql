CREATE TABLE [sync4partner].[MainConfig] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [LastSchemaConfig] DATETIME2 (7) DEFAULT ('1879-03-19') NOT NULL,
    [LastSyncTableChange] DATETIME2 (7) DEFAULT (SYSDATETIME()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


