CREATE TABLE [identity].[AspNetUserClaims](
    [Id]         INT              IDENTITY (1, 1) NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [ClaimType]  NVARCHAR(MAX)    NULL,
    [ClaimValue] NVARCHAR(MAX)    NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_User] FOREIGN KEY ([UserId]) REFERENCES [identity].[User] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId]
    ON [identity].[AspNetUserClaims] ([UserId]);
GO
