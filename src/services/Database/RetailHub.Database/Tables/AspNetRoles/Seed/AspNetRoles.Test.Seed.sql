BEGIN

SET IDENTITY_INSERT [identity].[AspNetRoles] ON;

MERGE [identity].[AspNetRoles] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (1, N'Customer', N'CUSTOMER', N'a3333333-3333-3333-3333-333333333333'),
            (2, N'Admin', N'ADMIN', N'a4444444-4444-4444-4444-444444444444')
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

SET IDENTITY_INSERT [identity].[AspNetRoles] OFF;

END
