BEGIN

MERGE [cart].[Cart] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'c0000001-0000-4000-8000-000000000001',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'b0000001-0000-4000-8000-000000000001',
                CAST(NULL AS NVARCHAR(128))
            )
    ) AS V ([Id], [CreatedOn], [DeletedOn], [UserId], [AnonymousKey])
) AS S
    ON T.[Id] = S.[Id]
WHEN MATCHED THEN
    UPDATE SET
        T.[UserId] = S.[UserId],
        T.[AnonymousKey] = S.[AnonymousKey],
        T.[DeletedOn] = S.[DeletedOn]
WHEN NOT MATCHED THEN
    INSERT ([Id], [CreatedOn], [DeletedOn], [UserId], [AnonymousKey])
    VALUES (S.[Id], S.[CreatedOn], S.[DeletedOn], S.[UserId], S.[AnonymousKey]);

END
