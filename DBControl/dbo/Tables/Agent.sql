CREATE TABLE [dbo].[Agent] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]  NVARCHAR (50) NULL,
    [LastName]   NVARCHAR (50) NULL,
    [CurrencyID] NCHAR (10)    NULL,
    CONSTRAINT [PK_Agent] PRIMARY KEY CLUSTERED ([Id] ASC)
);

