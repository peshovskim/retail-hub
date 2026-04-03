BEGIN

MERGE [catalog].[Category] AS T
USING (
    SELECT *
    FROM (
        VALUES
            (
                N'a0000001-0000-4000-8000-000000000001',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Rods & Reels',
                N'rods-and-reels',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a0000002-0000-4000-8000-000000000002',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Carp Rods',
                N'carp-rods',
                N'a0000001-0000-4000-8000-000000000001'
            ),
            (
                N'a0000003-0000-4000-8000-000000000003',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Bivvies & Shelters',
                N'bivvies-and-shelters',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a0000004-0000-4000-8000-000000000004',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Day Shelters',
                N'day-shelters',
                N'a0000003-0000-4000-8000-000000000003'
            ),
            (
                N'a0000005-0000-4000-8000-000000000005',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Bait & Feed',
                N'bait-and-feed',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a0000006-0000-4000-8000-000000000006',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Big Pit Reels',
                N'big-pit-reels',
                N'a0000001-0000-4000-8000-000000000001'
            ),
            (
                N'a0000007-0000-4000-8000-000000000007',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Rod Pods & Banksticks',
                N'rod-pods-and-banksticks',
                N'a0000001-0000-4000-8000-000000000001'
            ),
            (
                N'a0000008-0000-4000-8000-000000000008',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Terminal Tackle',
                N'terminal-tackle',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a0000009-0000-4000-8000-000000000009',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Hooks & Rigs',
                N'hooks-and-rigs',
                N'a0000008-0000-4000-8000-000000000008'
            ),
            (
                N'a000000a-0000-4000-8000-000000000010',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Weights & Leads',
                N'weights-and-leads',
                N'a0000008-0000-4000-8000-000000000008'
            ),
            (
                N'a000000b-0000-4000-8000-000000000011',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Boilies',
                N'boilies',
                N'a0000005-0000-4000-8000-000000000005'
            ),
            (
                N'a000000c-0000-4000-8000-000000000012',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Pellets & Particles',
                N'pellets-and-particles',
                N'a0000005-0000-4000-8000-000000000005'
            ),
            (
                N'a000000d-0000-4000-8000-000000000013',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Liquids & Additives',
                N'liquids-and-additives',
                N'a0000005-0000-4000-8000-000000000005'
            ),
            (
                N'a000000e-0000-4000-8000-000000000014',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Bedchairs & Sleep',
                N'bedchairs-and-sleep',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a000000f-0000-4000-8000-000000000015',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Bedchairs',
                N'bedchairs',
                N'a000000e-0000-4000-8000-000000000014'
            ),
            (
                N'a0000010-0000-4000-8000-000000000016',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Sleeping Bags',
                N'sleeping-bags',
                N'a000000e-0000-4000-8000-000000000014'
            ),
            (
                N'a0000011-0000-4000-8000-000000000017',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Clothing',
                N'clothing',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a0000012-0000-4000-8000-000000000018',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Luggage & Barrows',
                N'luggage-and-barrows',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a0000013-0000-4000-8000-000000000019',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Electronics',
                N'electronics',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a0000014-0000-4000-8000-000000000020',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Bite Alarms & Receivers',
                N'bite-alarms-and-receivers',
                N'a0000013-0000-4000-8000-000000000019'
            ),
            (
                N'a0000015-0000-4000-8000-000000000021',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Bivvies',
                N'bivvies',
                N'a0000003-0000-4000-8000-000000000003'
            ),
            (
                N'a0000016-0000-4000-8000-000000000022',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Chairs & Accessories',
                N'chairs-and-accessories',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a0000017-0000-4000-8000-000000000023',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Fishing Chairs',
                N'fishing-chairs',
                N'a0000016-0000-4000-8000-000000000022'
            ),
            (
                N'a0000018-0000-4000-8000-000000000024',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Nets & Landing',
                N'nets-and-landing',
                CAST(NULL AS UNIQUEIDENTIFIER)
            ),
            (
                N'a0000019-0000-4000-8000-000000000025',
                CAST(GETUTCDATE() AS DATETIME2(0)),
                CAST(NULL AS DATETIME2(0)),
                N'Landing Nets',
                N'landing-nets',
                N'a0000018-0000-4000-8000-000000000024'
            )
    ) AS V (
        [Id],
        [CreatedOn],
        [DeletedOn],
        [Name],
        [Slug],
        [ParentId]
    )
) AS S
    ON T.[Id] = S.[Id]
WHEN MATCHED THEN
    UPDATE SET
        T.[DeletedOn] = S.[DeletedOn],
        T.[Name] = S.[Name],
        T.[Slug] = S.[Slug],
        T.[ParentId] = S.[ParentId]
WHEN NOT MATCHED THEN
    INSERT (
        [Id],
        [CreatedOn],
        [DeletedOn],
        [Name],
        [Slug],
        [ParentId]
    )
    VALUES (
        S.[Id],
        S.[CreatedOn],
        S.[DeletedOn],
        S.[Name],
        S.[Slug],
        S.[ParentId]
    );

END
