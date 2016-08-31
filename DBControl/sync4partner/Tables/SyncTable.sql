CREATE TABLE [sync4partner].[SyncTable] (
    [Name]    [sysname] NOT NULL,
    [ToCopy]  BIT       NULL,
    [ToStage] BIT       DEFAULT ((0)) NOT NULL,
	PackageName varchar(125) DEFAULT ('EmptyPlaceHolder.dtsx') NOT NULL,
    CONSTRAINT [PK_SyncTable] PRIMARY KEY CLUSTERED ([Name] ASC)
);



