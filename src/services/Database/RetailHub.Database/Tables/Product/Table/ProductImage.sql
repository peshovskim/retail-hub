CREATE TABLE [catalog].[ProductImage](
    [Id]         INT                 NOT NULL IDENTITY(1, 1),
    [Uid]        UNIQUEIDENTIFIER    NOT NULL,
    [CreatedOn]  DATETIME2(0)        NOT NULL,
    [DeletedOn]  DATETIME2(0)        NULL,
    [ProductId]  INT                 NOT NULL,
    [SortOrder]  INT                 NOT NULL CONSTRAINT [DF_ProductImage_SortOrder] DEFAULT (0),
    [ImageUrl]   NVARCHAR(2048)      NOT NULL,
    CONSTRAINT [PK_ProductImage] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [UQ_ProductImage_Uid] UNIQUE ([Uid]),
    CONSTRAINT [FK_ProductImage_Product] FOREIGN KEY ([ProductId]) REFERENCES [catalog].[Product] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_ProductImage_ProductId]
    ON [catalog].[ProductImage] ([ProductId]);
GO
