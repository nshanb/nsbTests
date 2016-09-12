CREATE PROCEDURE [sync4partner].FillSyncOperational
AS
BEGIN
	SET NOCOUNT ON;
	update [sync4partner].[SyncTableOperational] set UpdateConditionForSplit = [sync4partner].fn_concatCompareStr(name)
	update [sync4partner].[SyncTableOperational] set UpdateExpressonForOLECommand = [sync4partner].fn_concatUpdateStr(name)
	update [sync4partner].[SyncTableOperational] set UpdateExpressonForOLECommand = [sync4partner].fn_concatUpsertStr(name)
    where exists( select 1 from [sync4partner].[SyncTable] where [SyncTable].Name=[SyncTableOperational].Name and NeedsUpsert=1)
END