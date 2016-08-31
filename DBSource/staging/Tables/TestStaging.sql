CREATE TABLE [staging].[TestStaging] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [MessageText] NVARCHAR (250) NULL,
    [MessageDate]     DATETIME2 (7)  NOT NULL DEFAULT(SYSDATETIME()),
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

