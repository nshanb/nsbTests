CREATE FUNCTION [sync4partner].fn_concatUpdateStr(@tableName sysname)
RETURNS Varchar(Max)
AS
BEGIN
 declare @c Varchar(Max)
 set @c = 'update ' + @tableName + ' set '
 
 select @c = @c + '[' + cName + ']=s.['+cName+'],'
  from [sync4partner].[ColumnList]
  where is_identity = 0 and is_nullable = 1 and tname = @tableName

  set @c = substring(@c,0,len(@c)-0)
 RETURN @c + ' from staging.' +@tableName+ ' as s where s.id='+@tableName+'.id; delete staging.'+@tableName

END


-- update Table2 set [Text2]=s.[Text2],[Table1Id]=s.[Table1Id]  from staging.Table2 as s where s.id=table2.id; delete staging.Table2