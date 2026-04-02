CREATE TABLE [cart].[Cart](
    [Id]            UNIQUEIDENTIFIER    NOT NULL,
    [CreatedOn]     DATETIME2(0)        NOT NULL,
    [UserId]        UNIQUEIDENTIFIER    NULL,
    [AnonymousKey]  NVARCHAR(128)       NULL,
    CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_Cart_User] FOREIGN KEY ([UserId]) REFERENCES [identity].[User] ([Id])
);
GO
