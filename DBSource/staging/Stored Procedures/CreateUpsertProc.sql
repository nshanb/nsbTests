CREATE PROCEDURE [staging].[CreateUpsertProc]
	@tableName sysname
AS
  SET NOCOUNT ON
	declare @procName sysname
	declare @procBody nvarchar(MAX)
  declare @com varchar(max)
  set @procName = 'DoUpsert'+@tableName

  set @com='CREATE '
  if exists (select 1 from sys.objects where type='P' and name=@procName and SCHEMA_ID('staging')=schema_id)
    set @com='ALTER '
  set @com=@com+'PROCEDURE [staging].['+@procName+']'

  select tName,cName,is_nullable,is_identity,cols.column_id,key_ordinal,iName,
  CASE WHEN iName is null THEN 0 ELSE 1 END as is_primarykey,typeName into #t
  from 
  (select distinct t.name tName,c.name cName,c.is_nullable,is_identity,c.column_id,t.object_id,types.name as typeName from sys.tables as t join sys.columns as c on t.object_id = c.object_id  join sys.schemas on t.schema_id=schemas.schema_id and schemas.name='dbo' join sys.types on c.user_type_id=types.user_type_id) as cols
  left join
  (select indexes.object_id,index_columns.column_id,key_ordinal,indexes.name as iName from sys.index_columns 
  join sys.indexes on index_columns.object_id=indexes.object_id and indexes.is_primary_key=1 and indexes.index_id=index_columns.index_id) as pk
  on pk.object_id=cols.object_id and pk.column_id=cols.column_id
  where @tableName=tName

  declare @c Varchar(Max)
  set @c = ''
  select @c = @c + '[' + cName + ']=s.['+cName+'],'
   from #t
   where typeName <> 'timestamp'
  set @c = substring(@c,0,len(@c)-0)

  declare @i1 Varchar(Max)
  declare @i2 Varchar(Max)
  set @i1 = ''
  select @i1 = @i1 + '[' + cName + '],'
   from #t
   where typeName <> 'timestamp'
  set @i1 = substring(@i1,0,len(@i1)-0)
  set @i2 = ''
  select @i2 = @i2 + 's.['+cName+'],'
   from #t
   where typeName <> 'timestamp'
  set @i2 = substring(@i2,0,len(@i2)-0)

  set @procBody = 
'
AS
MERGE ['+@tableName+'] AS target
USING staging.['+@tableName+'] AS s
ON (target.Id = s.Id)
WHEN MATCHED
THEN
  UPDATE SET '+@c+'
WHEN NOT MATCHED BY target
THEN
  INSERT ('+@i1+')
  VALUES ('+@i2+');
RETURN 0
'
  EXEC(@com+@procBody)

RETURN 0