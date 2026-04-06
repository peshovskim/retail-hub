CREATE TABLE [identity].[AspNetUserRoles](
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_User] FOREIGN KEY ([UserId]) REFERENCES [identity].[User] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_Role] FOREIGN KEY ([RoleId]) REFERENCES [identity].[AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId]
    ON [identity].[AspNetUserRoles] ([RoleId]);
GO
