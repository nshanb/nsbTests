CREATE FUNCTION [sync4partner].fn_concatUpsertStr(@tableName sysname)
RETURNS Varchar(Max)
AS
BEGIN

declare @comBody nvarchar(MAX)
  declare @ident varchar(100)
  set @ident=''
  if exists(select 1 from [sync4partner].[ColumnList] where is_identity=1 and tName = @tableName)
    set @ident='SET IDENTITY_INSERT dbo.[' +@tableName+ '] ON'

  declare @cond Varchar(Max)
  set @cond = ''
  select @cond = @cond + 'target.[' + cName + ']=s.['+cName+'] and'
   from [sync4partner].[ColumnList]
   where is_primarykey=1 and tName = @tableName
   order by key_ordinal
  set @cond = substring(@cond,0,len(@cond)-3)

  declare @c Varchar(Max)
  set @c = ''
  select @c = @c + '[' + cName + ']=s.['+cName+'],'
   from [sync4partner].[ColumnList]
   where typeName <> 'timestamp' and is_primarykey=0 and tName = @tableName
  set @c = substring(@c,0,len(@c)-0)

  declare @i1 Varchar(Max)
  declare @i2 Varchar(Max)
  set @i1 = ''
  select @i1 = @i1 + '[' + cName + '],'
   from [sync4partner].[ColumnList]
   where typeName <> 'timestamp' and tName = @tableName
  set @i1 = substring(@i1,0,len(@i1)-0)
  
  set @i2 = ''
  select @i2 = @i2 + 's.['+cName+'],'
   from [sync4partner].[ColumnList]
   where typeName <> 'timestamp' and tName = @tableName
  set @i2 = substring(@i2,0,len(@i2)-0)

  set @comBody = @ident+';
MERGE ['+@tableName+'] AS target
USING staging.['+@tableName+'] AS s
ON ('+@cond+')
WHEN MATCHED
THEN
  UPDATE SET '+@c+'
WHEN NOT MATCHED BY target
THEN
  INSERT ('+@i1+')
  VALUES ('+@i2+');'

RETURN @comBody
END