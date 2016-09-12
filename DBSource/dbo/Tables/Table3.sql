﻿CREATE TABLE [dbo].[Table3] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (50) NULL,
    [LastName]  NVARCHAR (50) NOT NULL,
    [Modified]  datetime2 DEFAULT(SYSDATETIME()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

