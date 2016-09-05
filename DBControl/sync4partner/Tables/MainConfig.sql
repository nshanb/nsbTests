-- LastSchemaConfig: Last Date that everyting was configured both on ControllDb and Destinations
-- LastSyncTableChange: Last Date SyncTable was changed; is changed through trigger
CREATE TABLE [sync4partner].[MainConfig] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [LastSchemaConfig]    DATETIME2 (7) DEFAULT ('1879-03-19') NOT NULL,
    [LastSyncTableChange] DATETIME2 (7) DEFAULT (sysdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

