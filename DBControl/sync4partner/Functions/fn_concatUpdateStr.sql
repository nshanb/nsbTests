CREATE FUNCTION [sync4partner].fn_concatUpdateStr(@tableName sysname)
RETURNS Varchar(Max)
AS
BEGIN
 declare @n int
 select @n = count(*) from [sync4partner].[ColumnList]
 where is_primarykey = 0 and tName = @tableName
 if( @n = 0 ) return ''

 declare @c Varchar(Max)
 set @c = 'update [' + @tableName + '] set '
 
 select @c = @c + '[' + cName + ']=s.['+cName+'],'
  from [sync4partner].[ColumnList]
  where is_primarykey = 0 and typeName <> 'timestamp'and tname = @tableName and is_computed=0
  set @c = substring(@c,0,len(@c)-0)


  declare @cond Varchar(Max)
  set @cond = ''
  select @cond = @cond + '[' + @tableName+'].[' + cName + ']=s.['+cName+'] and '
   from [sync4partner].[ColumnList]
   where is_primarykey=1 and tName = @tableName
   order by key_ordinal
  set @cond = substring(@cond,0,len(@cond)-3)

 RETURN @c + ' from staging.[' +@tableName+ '] as s where ' + @cond+'; truncate table staging.['+@tableName+']'

END


-- update Table2 set [Text2]=s.[Text2],[Table1Id]=s.[Table1Id]  from staging.Table2 as s where s.id=table2.id; delete staging.Table2