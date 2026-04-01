BEGIN

MERGE [cart].[Cart] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'c0000001-0000-4000-8000-000000000001',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                N'b0000001-0000-4000-8000-000000000001',
                CAST(NULL AS NVARCHAR(128))
            )
    ) AS V ([Id], [CreatedOn], [UserId], [AnonymousKey])
) AS S
    ON T.[Id] = S.[Id]
WHEN MATCHED THEN
    UPDATE SET
        T.[UserId] = S.[UserId],
        T.[AnonymousKey] = S.[AnonymousKey]
WHEN NOT MATCHED THEN
    INSERT ([Id], [CreatedOn], [UserId], [AnonymousKey])
    VALUES (S.[Id], S.[CreatedOn], S.[UserId], S.[AnonymousKey]);

END
