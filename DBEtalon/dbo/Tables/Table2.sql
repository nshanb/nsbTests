CREATE TABLE [dbo].[Table2] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [text2]    NVARCHAR (50) NULL,
    [Table1Id] INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Table2_Table1] FOREIGN KEY ([Table1Id]) REFERENCES [dbo].[Table1] ([Id])
);



