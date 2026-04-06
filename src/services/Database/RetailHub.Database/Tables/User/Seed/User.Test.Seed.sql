/*
  Test users use password: Passw0rd!
  Hash from ASP.NET Core Identity PasswordHasher (Microsoft.Extensions.Identity.Core 10.x).
*/
BEGIN

DECLARE @SeedPasswordHash NVARCHAR(MAX) = N'AQAAAAIAAYagAAAAEFc5nXTzD7+txKVelJHcWU0voNCZPiDFufXE5cYpq0zfW2bR4aNB48s6oNUpbWeC3g==';

MERGE [identity].[User] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                CONVERT(UNIQUEIDENTIFIER, N'b0000001-0000-4000-8000-000000000001'),
                CAST(GETUTCDATE() AS DATETIME2(0)),
                N'seed.user@retailhub.local',
                N'SEED.USER@RETAILHUB.LOCAL',
                N'seed.user@retailhub.local',
                N'SEED.USER@RETAILHUB.LOCAL',
                CONVERT(BIT, 1),
                @SeedPasswordHash,
                N'11111111-1111-1111-1111-111111111111',
                N'a1111111-1111-1111-1111-111111111111',
                CAST(NULL AS NVARCHAR(MAX)),
                CONVERT(BIT, 0),
                CONVERT(BIT, 0),
                CAST(NULL AS DATETIMEOFFSET(7)),
                CONVERT(BIT, 1),
                0
            ),
            (
                CONVERT(UNIQUEIDENTIFIER, N'b0000002-0000-4000-8000-000000000001'),
                CAST(GETUTCDATE() AS DATETIME2(0)),
                N'admin.seed@retailhub.local',
                N'ADMIN.SEED@RETAILHUB.LOCAL',
                N'admin.seed@retailhub.local',
                N'ADMIN.SEED@RETAILHUB.LOCAL',
                CONVERT(BIT, 1),
                @SeedPasswordHash,
                N'22222222-2222-2222-2222-222222222222',
                N'a2222222-2222-2222-2222-222222222222',
                CAST(NULL AS NVARCHAR(MAX)),
                CONVERT(BIT, 0),
                CONVERT(BIT, 0),
                CAST(NULL AS DATETIMEOFFSET(7)),
                CONVERT(BIT, 1),
                0
            )
    ) AS V (
        [Uid],
        [CreatedOn],
        [UserName],
        [NormalizedUserName],
        [Email],
        [NormalizedEmail],
        [EmailConfirmed],
        [PasswordHash],
        [SecurityStamp],
        [ConcurrencyStamp],
        [PhoneNumber],
        [PhoneNumberConfirmed],
        [TwoFactorEnabled],
        [LockoutEnd],
        [LockoutEnabled],
        [AccessFailedCount]
    )
) AS S
    ON T.[Uid] = S.[Uid]
WHEN MATCHED THEN
    UPDATE SET
        T.[UserName] = S.[UserName],
        T.[NormalizedUserName] = S.[NormalizedUserName],
        T.[Email] = S.[Email],
        T.[NormalizedEmail] = S.[NormalizedEmail],
        T.[EmailConfirmed] = S.[EmailConfirmed],
        T.[PasswordHash] = S.[PasswordHash],
        T.[SecurityStamp] = S.[SecurityStamp],
        T.[ConcurrencyStamp] = S.[ConcurrencyStamp],
        T.[PhoneNumber] = S.[PhoneNumber],
        T.[PhoneNumberConfirmed] = S.[PhoneNumberConfirmed],
        T.[TwoFactorEnabled] = S.[TwoFactorEnabled],
        T.[LockoutEnd] = S.[LockoutEnd],
        T.[LockoutEnabled] = S.[LockoutEnabled],
        T.[AccessFailedCount] = S.[AccessFailedCount]
WHEN NOT MATCHED THEN
    INSERT (
        [Uid],
        [CreatedOn],
        [UserName],
        [NormalizedUserName],
        [Email],
        [NormalizedEmail],
        [EmailConfirmed],
        [PasswordHash],
        [SecurityStamp],
        [ConcurrencyStamp],
        [PhoneNumber],
        [PhoneNumberConfirmed],
        [TwoFactorEnabled],
        [LockoutEnd],
        [LockoutEnabled],
        [AccessFailedCount]
    )
    VALUES (
        S.[Uid],
        S.[CreatedOn],
        S.[UserName],
        S.[NormalizedUserName],
        S.[Email],
        S.[NormalizedEmail],
        S.[EmailConfirmed],
        S.[PasswordHash],
        S.[SecurityStamp],
        S.[ConcurrencyStamp],
        S.[PhoneNumber],
        S.[PhoneNumberConfirmed],
        S.[TwoFactorEnabled],
        S.[LockoutEnd],
        S.[LockoutEnabled],
        S.[AccessFailedCount]
    );

END
