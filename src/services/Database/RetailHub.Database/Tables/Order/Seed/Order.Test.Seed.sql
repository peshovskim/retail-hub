BEGIN

MERGE [orders].[Order] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'd0000001-0000-4000-8000-000000000001',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                N'b0000001-0000-4000-8000-000000000001',
                N'Pending'
            )
    ) AS V ([Id], [CreatedOn], [UserId], [Status])
) AS S
    ON T.[Id] = S.[Id]
WHEN MATCHED THEN
    UPDATE SET
        T.[UserId] = S.[UserId],
        T.[Status] = S.[Status]
WHEN NOT MATCHED THEN
    INSERT ([Id], [CreatedOn], [UserId], [Status])
    VALUES (S.[Id], S.[CreatedOn], S.[UserId], S.[Status]);

END
