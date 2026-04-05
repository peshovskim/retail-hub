CREATE TABLE [identity].[User](
    [Id]                    UNIQUEIDENTIFIER    NOT NULL,
    [CreatedOn]             DATETIME2(0)        NOT NULL,
    [UserName]              NVARCHAR(256)       NULL,
    [NormalizedUserName]    NVARCHAR(256)       NULL,
    [Email]                 NVARCHAR(256)       NOT NULL,
    [NormalizedEmail]       NVARCHAR(256)       NULL,
    [EmailConfirmed]        BIT                 NOT NULL CONSTRAINT [DF_User_EmailConfirmed] DEFAULT ((0)),
    [PasswordHash]          NVARCHAR(MAX)       NULL,
    [SecurityStamp]         NVARCHAR(MAX)       NULL,
    [ConcurrencyStamp]      NVARCHAR(MAX)       NULL,
    [PhoneNumber]           NVARCHAR(MAX)       NULL,
    [PhoneNumberConfirmed]  BIT                 NOT NULL CONSTRAINT [DF_User_PhoneNumberConfirmed] DEFAULT ((0)),
    [TwoFactorEnabled]      BIT                 NOT NULL CONSTRAINT [DF_User_TwoFactorEnabled] DEFAULT ((0)),
    [LockoutEnd]            DATETIMEOFFSET(7)   NULL,
    [LockoutEnabled]        BIT                 NOT NULL CONSTRAINT [DF_User_LockoutEnabled] DEFAULT ((1)),
    [AccessFailedCount]     INT                 NOT NULL CONSTRAINT [DF_User_AccessFailedCount] DEFAULT ((0)),
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [UQ_User_NormalizedUserName] UNIQUE ([NormalizedUserName]),
    CONSTRAINT [UQ_User_NormalizedEmail] UNIQUE ([NormalizedEmail])
);
GO
