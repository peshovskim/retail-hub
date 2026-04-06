CREATE TABLE [orders].[OrderLine](
    [Id]            INT                 NOT NULL IDENTITY(1, 1),
    [Uid]           UNIQUEIDENTIFIER    NOT NULL,
    [OrderId]       INT                 NOT NULL,
    [ProductId]     INT                 NOT NULL,
    [ProductUid]    UNIQUEIDENTIFIER    NOT NULL,
    [Quantity]      INT                 NOT NULL,
    [UnitPrice]     DECIMAL(18, 2)      NOT NULL,
    [LineTotal]     DECIMAL(18, 2)      NOT NULL,
    [CreatedOn]     DATETIME2(0)        NOT NULL,
    [DeletedOn]     DATETIME2(0)        NULL,
    CONSTRAINT [PK_OrderLine] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [UQ_OrderLine_Uid] UNIQUE ([Uid]),
    CONSTRAINT [FK_OrderLine_Order] FOREIGN KEY ([OrderId]) REFERENCES [orders].[Order] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderLine_Product] FOREIGN KEY ([ProductId]) REFERENCES [catalog].[Product] ([Id]),
    CONSTRAINT [CK_OrderLine_Quantity_Positive] CHECK ([Quantity] > 0)
);
GO

CREATE NONCLUSTERED INDEX [IX_OrderLine_OrderId]
    ON [orders].[OrderLine] ([OrderId]);
GO
