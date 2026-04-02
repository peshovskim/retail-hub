CREATE TABLE [identity].[User](
    [Id]            UNIQUEIDENTIFIER    NOT NULL,
    [CreatedOn]     DATETIME2(0)        NOT NULL,
    [Email]         NVARCHAR(256)       NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [UQ_User_Email] UNIQUE ([Email])
);
GO
