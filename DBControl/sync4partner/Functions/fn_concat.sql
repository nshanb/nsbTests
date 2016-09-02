CREATE FUNCTION [sync4partner].fn_concat(@tableName sysname)
RETURNS Varchar(Max)
AS
BEGIN
 declare @c Varchar(Max)
 set @c = ''
 
 select @c = @c + '(ISNULL([' + cName + '])?ISNULL(['+cName+' (1)]):['+cName+']!=['+cName+' (1)]) ||'
  from [sync4partner].[ColumnList]
  where is_identity = 0 and is_nullable = 1 and tname = @tableName

  select @c = @c +  '(['+cName+']!=['+cName+' (1)]) ||'
   from [sync4partner].[ColumnList]
   where is_identity = 0 and is_nullable = 0  and tname = @tableName

 RETURN substring(@c,0,len(@c)-2)

END