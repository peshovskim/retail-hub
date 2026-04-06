CREATE TABLE [identity].[AspNetUserTokens](
    [UserId]        INT              NOT NULL,
    [LoginProvider] NVARCHAR(450)    NOT NULL,
    [Name]          NVARCHAR(450)    NOT NULL,
    [Value]         NVARCHAR(MAX)    NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_User] FOREIGN KEY ([UserId]) REFERENCES [identity].[User] ([Id]) ON DELETE CASCADE
);
GO
