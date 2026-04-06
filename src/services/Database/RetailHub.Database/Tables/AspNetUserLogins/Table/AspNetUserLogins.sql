CREATE TABLE [identity].[AspNetUserLogins](
    [LoginProvider]       NVARCHAR(450)    NOT NULL,
    [ProviderKey]         NVARCHAR(450)    NOT NULL,
    [ProviderDisplayName] NVARCHAR(MAX)    NULL,
    [UserId]              INT              NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_User] FOREIGN KEY ([UserId]) REFERENCES [identity].[User] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId]
    ON [identity].[AspNetUserLogins] ([UserId]);
GO
