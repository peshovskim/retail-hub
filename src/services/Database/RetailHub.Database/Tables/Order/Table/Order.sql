CREATE TABLE [orders].[Order](
    [Id]            UNIQUEIDENTIFIER    NOT NULL,
    [CreatedOn]     DATETIME2(0)        NOT NULL,
    [UserId]        UNIQUEIDENTIFIER    NULL,
    [Status]        NVARCHAR(64)        NOT NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_Order_User] FOREIGN KEY ([UserId]) REFERENCES [identity].[User] ([Id])
);
GO
