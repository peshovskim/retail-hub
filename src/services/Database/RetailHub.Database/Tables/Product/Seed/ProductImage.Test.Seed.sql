/*
  One placeholder image per active catalog.Product (SortOrder = 0).
  After uploading real files to Azure Blob (or CDN), replace ImageUrl with the public HTTPS URL
  or run UPDATE statements keyed by ProductId / Uid.
*/

BEGIN

MERGE [catalog].[ProductImage] AS T
USING (
    SELECT
        p.[Id] AS [ProductId],
        NEWID() AS [Uid],
        CAST(GETUTCDATE() AS DATETIME2(0)) AS [CreatedOn],
        CAST(NULL AS DATETIME2(0)) AS [DeletedOn]
    FROM [catalog].[Product] AS p
    WHERE p.[DeletedOn] IS NULL
) AS S
    ON T.[ProductId] = S.[ProductId] AND T.[SortOrder] = 0
WHEN MATCHED THEN
    UPDATE SET
        T.[DeletedOn] = S.[DeletedOn]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Uid], [CreatedOn], [DeletedOn], [ProductId], [SortOrder], [ImageUrl])
    VALUES (
        S.[Uid],
        S.[CreatedOn],
        S.[DeletedOn],
        S.[ProductId],
        0,
        N'https://placeholder.example.blob.core.windows.net/product-images/REPLACE-WITH-YOUR-BLOB-URL.png'
    );

END
