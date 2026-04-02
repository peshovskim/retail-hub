BEGIN

MERGE [catalog].[Category] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'a0000001-0000-4000-8000-000000000001',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Electronics',
                N'electronics',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a0000002-0000-4000-8000-000000000002',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Computers',
                N'computers',
                N'a0000001-0000-4000-8000-000000000001'
            ),
            (
                N'a0000003-0000-4000-8000-000000000003',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Clothing',
                N'clothing',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a0000004-0000-4000-8000-000000000004',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Menswear',
                N'menswear',
                N'a0000003-0000-4000-8000-000000000003'
            ),
            (
                N'a0000005-0000-4000-8000-000000000005',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Books',
                N'books',
                CAST(NULL AS UNIQUEIDENTIFIER)
            )
    ) AS V (
        [Id],
        [CreatedOn],
        [DeletedOn],
        [Name],
        [Slug],
        [ParentId]
    )
) AS S
    ON T.[Id] = S.[Id]
WHEN MATCHED THEN
    UPDATE SET
        T.[DeletedOn] = S.[DeletedOn],
        T.[Name] = S.[Name],
        T.[Slug] = S.[Slug],
        T.[ParentId] = S.[ParentId]
WHEN NOT MATCHED THEN
    INSERT (
        [Id],
        [CreatedOn],
        [DeletedOn],
        [Name],
        [Slug],
        [ParentId]
    )
    VALUES (
        S.[Id],
        S.[CreatedOn],
        S.[DeletedOn],
        S.[Name],
        S.[Slug],
        S.[ParentId]
    );

END
