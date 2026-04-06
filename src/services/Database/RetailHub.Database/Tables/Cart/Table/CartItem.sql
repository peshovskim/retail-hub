CREATE TABLE [cart].[CartItem](
    [Id]            INT                 NOT NULL IDENTITY(1, 1),
    [Uid]           UNIQUEIDENTIFIER    NOT NULL,
    [CartId]        INT                 NOT NULL,
    [ProductId]     INT                 NOT NULL,
    [Quantity]      INT                 NOT NULL,
    [UnitPrice]     DECIMAL(18, 2)      NOT NULL,
    [CreatedOn]     DATETIME2(0)        NOT NULL,
    [DeletedOn]     DATETIME2(0)        NULL,
    [UpdatedOn]     DATETIME2(0)        NULL,
    CONSTRAINT [PK_CartItem] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [UQ_CartItem_Uid] UNIQUE ([Uid]),
    CONSTRAINT [FK_CartItem_Cart] FOREIGN KEY ([CartId]) REFERENCES [cart].[Cart] ([Id]),
    CONSTRAINT [FK_CartItem_Product] FOREIGN KEY ([ProductId]) REFERENCES [catalog].[Product] ([Id]),
    CONSTRAINT [CK_CartItem_Quantity_Positive] CHECK ([Quantity] > 0)
);
GO

CREATE NONCLUSTERED INDEX [IX_CartItem_CartId]
    ON [cart].[CartItem] ([CartId]);
GO
