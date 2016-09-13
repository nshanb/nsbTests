CREATE TABLE [sync4partner].[ColumnList] (
    [tName]         [sysname]      NOT NULL,
    [cName]         [sysname]      NOT NULL,
    [is_nullable]   BIT            NOT NULL,
    [is_identity]   BIT            NOT NULL,
    [column_id]     INT            NOT NULL,
    [key_ordinal]   INT            NULL,
    [iName]         [sysname]      NULL,
    [is_primarykey] BIT            NOT NULL,
    [typeName]      NVARCHAR (128) NOT NULL,
    [is_computed]   BIT            NOT NULL,
    CONSTRAINT [PK_ColumnList] PRIMARY KEY CLUSTERED ([tName] ASC, [cName] ASC)
);





