CREATE PROCEDURE [sync4partner].FillSyncOperational
AS
BEGIN
	SET NOCOUNT ON;
	-- fill SyncTableOperational
	insert sync4partner.SyncTableOperational (Name) select Name from sync4partner.SyncTable where not exists (select 1 from sync4partner.SyncTableOperational as d where SyncTable.Name=d.Name) and ToCopy=1

	-- fill SyncTableOperational
	insert sync4partner.SyncTableData (Name,PartnerId) select Name,PartnerID from sync4partner.SyncTable, sync4partner.TaskConfig 
		where not exists (select 1 from sync4partner.SyncTableData as d where SyncTable.Name=d.Name and TaskConfig.PartnerID=d.PartnerId) and ToCopy=1

	update [sync4partner].[SyncTableOperational] set CountStr = 'select count(*) as N1 from ['+[Name]+'] ' + PartnerID

	-- fill operational info
	update [sync4partner].[SyncTableOperational] set UpdateConditionForSplit = [sync4partner].fn_concatCompareStr(name)
	update [sync4partner].[SyncTableOperational] set UpdateExpressonForOLECommand = [sync4partner].fn_concatUpdateStr(name)
	update [sync4partner].[SyncTableOperational] set UpdateExpressonForOLECommand = [sync4partner].fn_concatUpsertStr(name)
    where exists( select 1 from [sync4partner].[SyncTable] where [SyncTable].Name=[SyncTableOperational].Name and NeedsUpsert=1)
END