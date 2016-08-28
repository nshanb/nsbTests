CREATE TABLE [sync4partner].[SchemaCheckResults] (
    [ChecktDate]  DATETIME2 (7)  DEFAULT (sysdatetime()) NOT NULL,
    [Name]        [sysname]      NOT NULL,
    [SourceError] NVARCHAR (350) NULL,
    [DestError]   NVARCHAR (350) NULL
);

