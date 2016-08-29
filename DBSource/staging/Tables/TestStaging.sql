CREATE TABLE [staging].[TestStaging] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [TestText] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

