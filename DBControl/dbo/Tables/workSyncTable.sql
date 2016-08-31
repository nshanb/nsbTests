CREATE TABLE [dbo].[workSyncTable] (
    [tableName]  [sysname]     NOT NULL,
    [ToCopy]     VARCHAR (1)   NOT NULL,
    [OnlyInsert] VARCHAR (1)   NOT NULL,
    [Other]      VARCHAR (125) NULL,
    [f1]         [sysname]     NULL,
    [f5]         [sysname]     NULL,
    [f2]         [sysname]     NULL,
    [f3]         [sysname]     NULL,
    [f4]         [sysname]     NULL
);

