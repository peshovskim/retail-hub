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
                1,
                N'b0000001-0000-4000-8000-000000000001',
                N'Pending',
                (SELECT [Id] FROM [cart].[Cart] WHERE [Uid] = N'c0000001-0000-4000-8000-000000000001'),
                N'c0000001-0000-4000-8000-000000000001',
                CAST(0.00 AS DECIMAL(18, 2))
            )
    ) AS V ([Uid], [CreatedOn], [DeletedOn], [UserId], [UserUid], [Status], [CartId], [CartUid], [TotalAmount])
) AS S
    ON T.[Uid] = S.[Uid]
WHEN MATCHED THEN
    UPDATE SET
        T.[UserId] = S.[UserId],
        T.[UserUid] = S.[UserUid],
        T.[Status] = S.[Status],
        T.[CartId] = S.[CartId],
        T.[CartUid] = S.[CartUid],
        T.[TotalAmount] = S.[TotalAmount],
        T.[DeletedOn] = S.[DeletedOn]
WHEN NOT MATCHED THEN
    INSERT ([Uid], [CreatedOn], [DeletedOn], [UserId], [UserUid], [Status], [CartId], [CartUid], [TotalAmount])
    VALUES (S.[Uid], S.[CreatedOn], S.[DeletedOn], S.[UserId], S.[UserUid], S.[Status], S.[CartId], S.[CartUid], S.[TotalAmount]);

END
