CREATE TABLE [cart].[CartItem](
    [Id]            UNIQUEIDENTIFIER    NOT NULL,
    [CartId]        UNIQUEIDENTIFIER    NOT NULL,
    [ProductId]     UNIQUEIDENTIFIER    NOT NULL,
    [Quantity]      INT                 NOT NULL,
    [UnitPrice]     DECIMAL(18, 2)      NOT NULL,
    [CreatedOn]     DATETIME2(0)        NOT NULL,
    [UpdatedOn]     DATETIME2(0)        NULL,
    CONSTRAINT [PK_CartItem] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_CartItem_Cart] FOREIGN KEY ([CartId]) REFERENCES [cart].[Cart] ([Id]),
    CONSTRAINT [FK_CartItem_Product] FOREIGN KEY ([ProductId]) REFERENCES [catalog].[Product] ([Id]),
    CONSTRAINT [CK_CartItem_Quantity_Positive] CHECK ([Quantity] > 0),
    CONSTRAINT [UQ_CartItem_Cart_Product] UNIQUE ([CartId], [ProductId])
);
GO
