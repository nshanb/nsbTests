CREATE TABLE [dbo].[Table3] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (50) NULL,
    [LastName]  NVARCHAR (50) NOT NULL,
    [Modified]  DATETIME2 (7) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



