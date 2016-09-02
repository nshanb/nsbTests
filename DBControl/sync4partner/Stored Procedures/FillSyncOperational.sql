CREATE PROCEDURE [sync4partner].FillSyncOperational
AS
BEGIN
	SET NOCOUNT ON;
	update [sync4partner].[SyncTableOperational] set UpdateConditionForSplit = [sync4partner].fn_concat(name)
END