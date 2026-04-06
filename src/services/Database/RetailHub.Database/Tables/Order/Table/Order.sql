CREATE TABLE [orders].[Order](
    [Id]            INT                 NOT NULL IDENTITY(1, 1),
    [Uid]           UNIQUEIDENTIFIER    NOT NULL,
    [CreatedOn]     DATETIME2(0)        NOT NULL,
    [DeletedOn]     DATETIME2(0)        NULL,
    [UserId]        INT                 NULL,
    [UserUid]       UNIQUEIDENTIFIER    NULL,
    [Status]        NVARCHAR(64)        NOT NULL,
    [CartId]        INT                 NULL,
    [CartUid]       UNIQUEIDENTIFIER    NULL,
    [TotalAmount]   DECIMAL(18, 2)      NOT NULL CONSTRAINT [DF_Order_TotalAmount] DEFAULT ((0)),
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [UQ_Order_Uid] UNIQUE ([Uid]),
    CONSTRAINT [FK_Order_User] FOREIGN KEY ([UserId]) REFERENCES [identity].[User] ([Id]),
    CONSTRAINT [FK_Order_Cart] FOREIGN KEY ([CartId]) REFERENCES [cart].[Cart] ([Id])
);
GO
