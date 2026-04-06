BEGIN

MERGE [cart].[CartItem] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'd1000001-0000-4000-8000-000000000001',
                (SELECT [Id] FROM [cart].[Cart] WHERE [Uid] = N'c0000001-0000-4000-8000-000000000001'),
                (SELECT [Id] FROM [catalog].[Product] WHERE [Uid] = N'b0000001-0000-4000-8000-000000000001'),
                2,
                CAST(349.99 AS DECIMAL(18, 2)),
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0))
            ),
            (
                N'd1000002-0000-4000-8000-000000000002',
                (SELECT [Id] FROM [cart].[Cart] WHERE [Uid] = N'c0000001-0000-4000-8000-000000000001'),
                (SELECT [Id] FROM [catalog].[Product] WHERE [Uid] = N'b0000002-0000-4000-8000-000000000002'),
                1,
                CAST(429.0 AS DECIMAL(18, 2)),
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0))
            )
    ) AS V ([Uid], [CartId], [ProductId], [Quantity], [UnitPrice], [CreatedOn], [DeletedOn], [UpdatedOn])
) AS S
    ON T.[Uid] = S.[Uid]
WHEN MATCHED THEN
    UPDATE SET
        T.[CartId] = S.[CartId],
        T.[ProductId] = S.[ProductId],
        T.[Quantity] = S.[Quantity],
        T.[UnitPrice] = S.[UnitPrice],
        T.[CreatedOn] = S.[CreatedOn],
        T.[DeletedOn] = S.[DeletedOn],
        T.[UpdatedOn] = S.[UpdatedOn]
WHEN NOT MATCHED THEN
    INSERT ([Uid], [CartId], [ProductId], [Quantity], [UnitPrice], [CreatedOn], [DeletedOn], [UpdatedOn])
    VALUES (S.[Uid], S.[CartId], S.[ProductId], S.[Quantity], S.[UnitPrice], S.[CreatedOn], S.[DeletedOn], S.[UpdatedOn]);

END
