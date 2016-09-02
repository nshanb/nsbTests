CREATE TABLE [sync4partner].[SyncTable] (
    [Name]    [sysname] NOT NULL,
    [ToCopy]  BIT       NULL,
    [ToStage] BIT       DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SyncTable] PRIMARY KEY CLUSTERED ([Name] ASC)
);






GO
CREATE TRIGGER [sync4partner].LastTableChange_SyncTable
   ON  [sync4partner].[SyncTable]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	update [sync4partner].[MainConfig] set [LastSyncTableChange]=SYSDATETIME()
END