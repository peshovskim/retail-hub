CREATE TABLE [catalog].[Product](
    [Id]               UNIQUEIDENTIFIER    NOT NULL,
    [CreatedOn]        DATETIME2(0)        NOT NULL,
    [DeletedOn]        DATETIME2(0)        NULL,
    [CategoryId]       UNIQUEIDENTIFIER    NOT NULL,
    [Name]             NVARCHAR(256)       NOT NULL,
    [Slug]             NVARCHAR(256)       NOT NULL,
    [Sku]              NVARCHAR(64)        NOT NULL,
    [Price]            DECIMAL(18, 2)      NOT NULL,
    [ShortDescription] NVARCHAR(512)       NULL,
    [Description]      NVARCHAR(2000)      NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_Product_Category] FOREIGN KEY ([CategoryId]) REFERENCES [catalog].[Category] ([Id])
);
GO
