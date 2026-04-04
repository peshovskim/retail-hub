BEGIN

MERGE [orders].[Order] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'd0000001-0000-4000-8000-000000000001',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'b0000001-0000-4000-8000-000000000001',
                N'Pending',
                NULL,
                CAST(0.00 AS DECIMAL(18, 2))
            )
    ) AS V ([Id], [CreatedOn], [DeletedOn], [UserId], [Status], [CartId], [TotalAmount])
) AS S
    ON T.[Id] = S.[Id]
WHEN MATCHED THEN
    UPDATE SET
        T.[UserId] = S.[UserId],
        T.[Status] = S.[Status],
        T.[CartId] = S.[CartId],
        T.[TotalAmount] = S.[TotalAmount],
        T.[DeletedOn] = S.[DeletedOn]
WHEN NOT MATCHED THEN
    INSERT ([Id], [CreatedOn], [DeletedOn], [UserId], [Status], [CartId], [TotalAmount])
    VALUES (S.[Id], S.[CreatedOn], S.[DeletedOn], S.[UserId], S.[Status], S.[CartId], S.[TotalAmount]);

END
