BEGIN

MERGE [cart].[CartItem] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'd1000001-0000-4000-8000-000000000001',
                N'c0000001-0000-4000-8000-000000000001',
                N'b0000001-0000-4000-8000-000000000001',
                2,
                CAST(349.99 AS DECIMAL(18, 2)),
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0))
            ),
            (
                N'd1000002-0000-4000-8000-000000000002',
                N'c0000001-0000-4000-8000-000000000001',
                N'b0000002-0000-4000-8000-000000000002',
                1,
                CAST(429.0 AS DECIMAL(18, 2)),
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0))
            )
    ) AS V ([Id], [CartId], [ProductId], [Quantity], [UnitPrice], [CreatedOn], [UpdatedOn])
) AS S
    ON T.[Id] = S.[Id]
WHEN MATCHED THEN
    UPDATE SET
        T.[CartId] = S.[CartId],
        T.[ProductId] = S.[ProductId],
        T.[Quantity] = S.[Quantity],
        T.[UnitPrice] = S.[UnitPrice],
        T.[CreatedOn] = S.[CreatedOn],
        T.[UpdatedOn] = S.[UpdatedOn]
WHEN NOT MATCHED THEN
    INSERT ([Id], [CartId], [ProductId], [Quantity], [UnitPrice], [CreatedOn], [UpdatedOn])
    VALUES (S.[Id], S.[CartId], S.[ProductId], S.[Quantity], S.[UnitPrice], S.[CreatedOn], S.[UpdatedOn]);

END
