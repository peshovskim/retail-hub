BEGIN

MERGE [identity].[AspNetUserRoles] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'b0000001-0000-4000-8000-000000000001',
                N'c0000001-0000-4000-8000-000000000001'
            ),
            (
                N'b0000002-0000-4000-8000-000000000001',
                N'c0000002-0000-4000-8000-000000000001'
            )
    ) AS V ([UserId], [RoleId])
) AS S
    ON T.[UserId] = S.[UserId] AND T.[RoleId] = S.[RoleId]
WHEN NOT MATCHED THEN
    INSERT ([UserId], [RoleId])
    VALUES (S.[UserId], S.[RoleId]);

END
