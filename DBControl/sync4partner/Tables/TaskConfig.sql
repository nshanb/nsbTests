CREATE TABLE [sync4partner].[TaskConfig] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [PartnerID]       INT           NOT NULL,
    [LastSchemaCheck] DATETIME2 (7) DEFAULT ('1879-03-19') NOT NULL,
    [FirstTime]       BIT           DEFAULT ((1)) NULL,
    [SchemaStatus]    CHAR (1)      NULL,
    [LastSuccess]     DATETIME2 (7) DEFAULT ('1879-03-19') NOT NULL,
    [LastSuccessTemp] DATETIME2 (7) DEFAULT ('1879-03-19') NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UIX_TaskConfig_PartnerID] UNIQUE NONCLUSTERED ([PartnerID] ASC)
);



