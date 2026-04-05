CREATE TABLE [orders].[Order](
    [Id]            UNIQUEIDENTIFIER    NOT NULL,
    [CreatedOn]     DATETIME2(0)        NOT NULL,
    [DeletedOn]     DATETIME2(0)        NULL,
    [UserId]        UNIQUEIDENTIFIER    NULL,
    [Status]        NVARCHAR(64)        NOT NULL,
    [CartId]        UNIQUEIDENTIFIER    NULL,
    [TotalAmount]   DECIMAL(18, 2)      NOT NULL CONSTRAINT [DF_Order_TotalAmount] DEFAULT ((0)),
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_Order_User] FOREIGN KEY ([UserId]) REFERENCES [identity].[User] ([Id])
);
GO
