CREATE TABLE [dbo].[BigTable]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	FirstName nvarchar(200) NULL,
	LastName nvarchar(200) NULL,
	[Address] nvarchar(200) NULL,
	Address1 nvarchar(200) NULL,
	ParPriz int not null CONSTRAINT [DF_BigTable_ParPriz] Default(0),
	Modified DateTime2 not null CONSTRAINT [DF_Modified] Default(sysdatetime()),
    CONSTRAINT [PK_BigTable] PRIMARY KEY CLUSTERED ([Id] ASC)
)
