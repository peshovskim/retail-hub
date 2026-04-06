CREATE TABLE [cart].[Cart](
    [Id]            INT                 NOT NULL IDENTITY(1, 1),
    [Uid]           UNIQUEIDENTIFIER    NOT NULL,
    [CreatedOn]     DATETIME2(0)        NOT NULL,
    [DeletedOn]     DATETIME2(0)        NULL,
    [UserId]        INT                 NULL,
    [AnonymousKey]  NVARCHAR(128)       NULL,
    CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [UQ_Cart_Uid] UNIQUE ([Uid]),
    CONSTRAINT [FK_Cart_User] FOREIGN KEY ([UserId]) REFERENCES [identity].[User] ([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_Cart_AnonymousKey]
    ON [cart].[Cart] ([AnonymousKey]);
GO
