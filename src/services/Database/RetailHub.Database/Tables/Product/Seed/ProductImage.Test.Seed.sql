/*
  One image per active catalog.Product (SortOrder = 0), with URLs derived from product slug:
    full:  {BaseUrl}/{Slug}.jpg
    thumb: {BaseUrl}/{Slug}-thumb.jpg

  Upload each file to your Azure Blob container using the exact slug filename (example: "fox-eos-42-net.jpg").
  Change @BaseUrl to your storage account/container once, then publish.
*/

BEGIN

DECLARE @BaseUrl NVARCHAR(512) = N'https://retailhubphotos.blob.core.windows.net/product-images';

MERGE [catalog].[ProductImage] AS T
USING (
    SELECT
        p.[Id] AS [ProductId],
        NEWID() AS [Uid],
        CAST(GETUTCDATE() AS DATETIME2(0)) AS [CreatedOn],
        CAST(NULL AS DATETIME2(0)) AS [DeletedOn],
        CONCAT(@BaseUrl, N'/', p.[Slug], N'.jpg') AS [ImageUrl],
        CONCAT(@BaseUrl, N'/', p.[Slug], N'-thumb.jpg') AS [ThumbnailImageUrl]
    FROM [catalog].[Product] AS p
    WHERE p.[DeletedOn] IS NULL
) AS S
    ON T.[ProductId] = S.[ProductId] AND T.[SortOrder] = 0
WHEN MATCHED THEN
    UPDATE SET
        T.[DeletedOn] = S.[DeletedOn],
        T.[ImageUrl] = S.[ImageUrl],
        T.[ThumbnailImageUrl] = S.[ThumbnailImageUrl]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Uid], [CreatedOn], [DeletedOn], [ProductId], [SortOrder], [ImageUrl], [ThumbnailImageUrl])
    VALUES (
        S.[Uid],
        S.[CreatedOn],
        S.[DeletedOn],
        S.[ProductId],
        0,
        S.[ImageUrl],
        S.[ThumbnailImageUrl]
    );

END
