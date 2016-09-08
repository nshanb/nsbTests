CREATE FUNCTION [sync4partner].fn_concatCompareStr(@tableName sysname)
RETURNS Varchar(Max)
AS
BEGIN
 declare @c Varchar(Max)
 set @c = ''
 
 select @c = @c + '((ISNULL([' + cName + '])!=ISNULL(['+cName+' (1)]))||((!ISNULL([' + cName + '])&&!ISNULL(['+cName+' (1)]))&&(['+cName+']!=['+cName+' (1)]))) ||'
  from [sync4partner].[ColumnList]
  where is_primarykey = 0 and is_nullable = 1 and typeName <> 'timestamp' and tname = @tableName

  select @c = @c +  '(['+cName+']!=['+cName+' (1)]) ||'
   from [sync4partner].[ColumnList]
   where is_primarykey = 0 and is_nullable = 0 and typeName <> 'timestamp' and tname = @tableName

   if( len(@c) = 0 ) return 'FALSE'
 RETURN substring(@c,0,len(@c)-2)

END


-- ((ISNULL([text2])!=ISNULL([text2 (1)])) || ( (! ISNULL([text2]) && ! ISNULL([text2 (1)]) ) && ([text2] != [text2 (1)]) ))  || ((ISNULL([Table1Id])!=ISNULL([Table1Id (1)])) ||( ( ! ISNULL([Table1Id]) && ! ISNULL([Table1Id (1)])) && ([Table1Id] != [Table1Id (1)]) ))