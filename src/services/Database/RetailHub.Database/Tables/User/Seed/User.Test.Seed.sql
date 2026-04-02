BEGIN

MERGE [identity].[User] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'b0000001-0000-4000-8000-000000000001',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                N'seed.user@retailhub.local'
            )
    ) AS V ([Id], [CreatedOn], [Email])
) AS S
    ON T.[Id] = S.[Id]
WHEN MATCHED THEN
    UPDATE SET
        T.[Email] = S.[Email]
WHEN NOT MATCHED THEN
    INSERT ([Id], [CreatedOn], [Email])
    VALUES (S.[Id], S.[CreatedOn], S.[Email]);

END
