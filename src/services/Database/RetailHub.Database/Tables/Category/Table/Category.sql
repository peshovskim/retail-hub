CREATE TABLE [catalog].[Category](
    [Id]            UNIQUEIDENTIFIER    NOT NULL,
    [CreatedOn]     DATETIME2(0)        NOT NULL,
    [DeletedOn]     DATETIME2(0)        NULL,
    [Name]          NVARCHAR(256)       NOT NULL,
    [Slug]          NVARCHAR(256)       NOT NULL,
    [ParentId]      UNIQUEIDENTIFIER    NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_Category_Parent] FOREIGN KEY ([ParentId]) REFERENCES [catalog].[Category] ([Id])
);
GO
