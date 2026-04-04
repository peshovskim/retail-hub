CREATE TABLE [orders].[OrderLine](
    [Id]            UNIQUEIDENTIFIER    NOT NULL,
    [OrderId]       UNIQUEIDENTIFIER    NOT NULL,
    [ProductId]     UNIQUEIDENTIFIER    NOT NULL,
    [Quantity]      INT                 NOT NULL,
    [UnitPrice]     DECIMAL(18, 2)      NOT NULL,
    [LineTotal]     DECIMAL(18, 2)      NOT NULL,
    [CreatedOn]     DATETIME2(0)        NOT NULL,
    [DeletedOn]     DATETIME2(0)        NULL,
    CONSTRAINT [PK_OrderLine] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [CK_OrderLine_Quantity_Positive] CHECK ([Quantity] > 0)
);
GO
