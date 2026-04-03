BEGIN

MERGE [catalog].[Product] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'b0000001-0000-4000-8000-000000000001',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'a0000002-0000-4000-8000-000000000002',
                N'Laptop Pro 14',
                N'laptop-pro-14',
                N'LAP-14-001',
                CAST(1299.99 AS DECIMAL(18, 2)),
                N'Lightweight 14" laptop for work and play.',
                N'Full specs: latest processor, 16 GB RAM, 512 GB SSD.'
            ),
            (
                N'b0000002-0000-4000-8000-000000000002',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'a0000001-0000-4000-8000-000000000001',
                N'Wireless Mouse',
                N'wireless-mouse',
                N'EL-MOU-001',
                CAST(29.99 AS DECIMAL(18, 2)),
                N'Ergonomic wireless mouse.',
                NULL
            ),
            (
                N'b0000003-0000-4000-8000-000000000003',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'a0000004-0000-4000-8000-000000000004',
                N'Classic Tee',
                N'classic-tee',
                N'CL-TSH-001',
                CAST(24.50 AS DECIMAL(18, 2)),
                N'Cotton crew-neck t-shirt.',
                N'Available in several colors; machine washable.'
            ),
            (
                N'b0000004-0000-4000-8000-000000000004',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'a0000005-0000-4000-8000-000000000005',
                N'Sample Novel',
                N'sample-novel',
                N'BK-NVL-001',
                CAST(14.99 AS DECIMAL(18, 2)),
                N'Fiction paperback.',
                NULL
            ),
            (
                N'b0000005-0000-4000-8000-000000000005',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'a0000003-0000-4000-8000-000000000003',
                N'Running Jacket',
                N'running-jacket',
                N'CL-JKT-001',
                CAST(89.00 AS DECIMAL(18, 2)),
                N'Water-resistant running jacket.',
                NULL
            )
    ) AS V (
        [Id],
        [CreatedOn],
        [DeletedOn],
        [CategoryId],
        [Name],
        [Slug],
        [Sku],
        [Price],
        [ShortDescription],
        [Description]
    )
) AS S
    ON T.[Id] = S.[Id]
WHEN MATCHED THEN
    UPDATE SET
        T.[DeletedOn] = S.[DeletedOn],
        T.[CategoryId] = S.[CategoryId],
        T.[Name] = S.[Name],
        T.[Slug] = S.[Slug],
        T.[Sku] = S.[Sku],
        T.[Price] = S.[Price],
        T.[ShortDescription] = S.[ShortDescription],
        T.[Description] = S.[Description]
WHEN NOT MATCHED THEN
    INSERT (
        [Id],
        [CreatedOn],
        [DeletedOn],
        [CategoryId],
        [Name],
        [Slug],
        [Sku],
        [Price],
        [ShortDescription],
        [Description]
    )
    VALUES (
        S.[Id],
        S.[CreatedOn],
        S.[DeletedOn],
        S.[CategoryId],
        S.[Name],
        S.[Slug],
        S.[Sku],
        S.[Price],
        S.[ShortDescription],
        S.[Description]
    );

END
