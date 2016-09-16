CREATE TABLE [sync4partner].[SyncTable] (
    [Name]        [sysname]      NOT NULL,
    [ToCopy]      BIT            NULL,
    [ToStage]     BIT            DEFAULT ((0)) NOT NULL,
    [OnlyInsert]  BIT            DEFAULT ((0)) NOT NULL,
    [NeedsUpsert] BIT            DEFAULT ((0)) NOT NULL,
    [Description] NVARCHAR (150) NULL,
    [SortOrder]   INT            DEFAULT ((100)) NOT NULL,
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