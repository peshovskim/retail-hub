CREATE TABLE [identity].[AspNetRoles](
    [Id]               INT              NOT NULL IDENTITY(1, 1),
    [Name]             NVARCHAR(256)    NULL,
    [NormalizedName]   NVARCHAR(256)    NOT NULL,
    [ConcurrencyStamp] NVARCHAR(MAX)    NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [UQ_AspNetRoles_NormalizedName] UNIQUE ([NormalizedName])
);
GO
