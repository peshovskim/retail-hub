BEGIN

MERGE [identity].[AspNetUserRoles] AS T
USING (
    SELECT u.[Id] AS UserId, r.[Id] AS RoleId
    FROM [identity].[User] u
    CROSS JOIN [identity].[AspNetRoles] r
    WHERE (u.[Uid] = N'b0000001-0000-4000-8000-000000000001' AND r.[NormalizedName] = N'CUSTOMER')
       OR (u.[Uid] = N'b0000002-0000-4000-8000-000000000001' AND r.[NormalizedName] = N'ADMIN')
) AS S
    ON T.[UserId] = S.[UserId] AND T.[RoleId] = S.[RoleId]
WHEN NOT MATCHED THEN
    INSERT ([UserId], [RoleId])
    VALUES (S.[UserId], S.[RoleId]);

END
