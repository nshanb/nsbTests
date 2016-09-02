CREATE TABLE [sync4partner].[ColumnList] (
    [tName]       [sysname] NOT NULL,
    [cName]       [sysname] NOT NULL,
    [is_nullable] BIT       NOT NULL,
    [is_identity] BIT       NOT NULL,
    CONSTRAINT [PK_ColumnList] PRIMARY KEY CLUSTERED ([tName] ASC, [cName] ASC)
);

