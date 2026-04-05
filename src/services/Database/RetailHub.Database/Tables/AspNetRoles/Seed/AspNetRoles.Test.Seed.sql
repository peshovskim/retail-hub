BEGIN

MERGE [identity].[AspNetRoles] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'c0000001-0000-4000-8000-000000000001',
                N'Customer',
                N'CUSTOMER',
                N'a3333333-3333-3333-3333-333333333333'
            ),
            (
                N'c0000002-0000-4000-8000-000000000001',
                N'Admin',
                N'ADMIN',
                N'a4444444-4444-4444-4444-444444444444'
            )
    ) AS V ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
) AS S
    ON T.[Id] = S.[Id]
WHEN MATCHED THEN
    UPDATE SET
        T.[Name] = S.[Name],
        T.[NormalizedName] = S.[NormalizedName],
        T.[ConcurrencyStamp] = S.[ConcurrencyStamp]
WHEN NOT MATCHED THEN
    INSERT ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
    VALUES (S.[Id], S.[Name], S.[NormalizedName], S.[ConcurrencyStamp]);

END
