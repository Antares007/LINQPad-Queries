<Query Kind="SQL">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

-- Region Parameters
DECLARE @p0 Int = 20
DECLARE @p1 Date = '2011-12-04'
DECLARE @p2 Int = 2
DECLARE @p3 NChar(1) = '0'
DECLARE @p4 NVarChar(1000) = 'Chasabarebeli'
DECLARE @p5 NVarChar(1000) = 'GaukmdaChaubareblobisGamo'
DECLARE @p6 NVarChar(1000) = 'Raioni'
DECLARE @p7 NVarChar(1000) = 'Fosta'
DECLARE @p8 NVarChar(1000) = '...'
DECLARE @p9 Int = 20
DECLARE @p10 Date = '2011-12-04'
DECLARE @p11 Int = 2
DECLARE @p12 NChar(1) = '0'
DECLARE @p13 NVarChar(1000) = 'Chabarda'
DECLARE @p14 NVarChar(1000) = 'Gaformda'
DECLARE @p15 NVarChar(1000) = 'KontraktiGaformda'
DECLARE @p16 NVarChar(1000) = '...'
DECLARE @p17 VarChar(1000) = 'Chabarda'
DECLARE @p18 Int = 20
DECLARE @p19 Date = '2011-12-04'
DECLARE @p20 Int = 2
DECLARE @p21 NChar(1) = '0'
DECLARE @p22 NVarChar(1000) = 'VerChabarda'
DECLARE @p23 NVarChar(1000) = 'Gaformda'
DECLARE @p24 Int = 20
DECLARE @p25 Date = '2011-12-04'
DECLARE @p26 NVarChar(1000) = ''
DECLARE @p27 Int = 2
DECLARE @p28 NChar(1) = '0'
DECLARE @p29 NVarChar(1000) = 'Gaukmda'
-- EndRegion
delete PolisisMdebareoba 
insert into PolisisMdebareoba (PolisisNomeri,ShekmnisPeriodi,DarigebisStatusi,DarigebisMdebareoba)
SELECT [t33].[PolisisNomeri], [t33].[value] AS [ShekmnisPeriodi], [t33].[value2] AS [DarigebisStatusi], [t33].[value3] AS [DarigebisMdebareoba]
FROM (
    SELECT [t22].[PolisisNomeri], [t22].[value], [t22].[value2], [t22].[value3]
    FROM (
        SELECT [t13].[PolisisNomeri], [t13].[value], [t13].[value2], [t13].[value3]
        FROM (
            SELECT [t0].[PolisisNomeri], (CONVERT(NVarChar,DATEPART(Year, [t0].[ShekmnisTarigi]))) + (
                (CASE 
                    WHEN (CONVERT(Int,DATALENGTH(CONVERT(NVarChar,DATEPART(Month, [t0].[ShekmnisTarigi]))) / 2)) >= @p2 THEN CONVERT(NVarChar,DATEPART(Month, [t0].[ShekmnisTarigi]))
                    ELSE REPLICATE(@p3, @p2 - (CONVERT(Int,DATALENGTH(CONVERT(NVarChar,DATEPART(Month, [t0].[ShekmnisTarigi]))) / 2))) + (CONVERT(NVarChar,DATEPART(Month, [t0].[ShekmnisTarigi])))
                 END)) AS [value], @p4 AS [value2], 
                (CASE 
                    WHEN EXISTS(
                        SELECT NULL AS [EMPTY]
                        FROM [dbo].[MovlenebiGaformdaGaukmdaKontrakti] (nolock) AS [t5]
                        WHERE [t5].[PolisisNomeri] = [t0].[PolisisNomeri]
                        ) THEN @p5
                    WHEN (EXISTS(
                        SELECT NULL AS [EMPTY]
                        FROM [dbo].[GadaecaRaions] (nolock) AS [t6]
                        WHERE [t6].[PolisisNomeri] = [t0].[PolisisNomeri]
                        )) OR (EXISTS(
                        SELECT NULL AS [EMPTY]
                        FROM [dbo].[Daibechda]  (nolock)  AS [t7]
                        WHERE [t7].[PolisisNomeri] = [t0].[PolisisNomeri]
                        )) THEN CONVERT(NVarChar(25),@p6)
                    WHEN EXISTS(
                        SELECT NULL AS [EMPTY]
                        FROM [dbo].[GadaecaFostas]  (nolock) AS [t8]
                        WHERE [t8].[PolisisNomeri] = [t0].[PolisisNomeri]
                        ) THEN CONVERT(NVarChar(25),@p7)
                    ELSE CONVERT(NVarChar(25),@p8)
                 END) AS [value3]
            FROM [dbo].[Polisebi]  (nolock) AS [t0]
            LEFT OUTER JOIN (
                SELECT 1 AS [test], [t1].[PolisisNomeri]
                FROM [dbo].[MovlenaChabarebebi]  (nolock) AS [t1]
                ) AS [t2] ON [t2].[PolisisNomeri] = [t0].[PolisisNomeri]
            WHERE (NOT (EXISTS(
                SELECT NULL AS [EMPTY]
                FROM [dbo].[MovlenebiGaformdaGaukmdaKontrakti]  (nolock) AS [t3]
                WHERE [t3].[PolisisNomeri] = [t0].[PolisisNomeri]
                ))) AND (NOT (EXISTS(
                SELECT NULL AS [EMPTY]
                FROM [dbo].[PolisisChabarebisIstoria]  (nolock) AS [t4]
                WHERE [t4].[PolisisNomeri] = [t0].[PolisisNomeri]
                ))) AND ([t2].[test] IS NULL) AND ([t0].[ProgramisId] < @p0) AND ([t0].[ShekmnisTarigi] <> @p1)
            UNION ALL
            SELECT [t9].[PolisisNomeri], (CONVERT(NVarChar,DATEPART(Year, [t9].[ShekmnisTarigi]))) + (
                (CASE 
                    WHEN (CONVERT(Int,DATALENGTH(CONVERT(NVarChar,DATEPART(Month, [t9].[ShekmnisTarigi]))) / 2)) >= @p11 THEN CONVERT(NVarChar,DATEPART(Month, [t9].[ShekmnisTarigi]))
                    ELSE REPLICATE(@p12, @p11 - (CONVERT(Int,DATALENGTH(CONVERT(NVarChar,DATEPART(Month, [t9].[ShekmnisTarigi]))) / 2))) + (CONVERT(NVarChar,DATEPART(Month, [t9].[ShekmnisTarigi])))
                 END)) AS [value], @p13 AS [value2], 
                (CASE 
                    WHEN EXISTS(
                        SELECT NULL AS [EMPTY]
                        FROM [dbo].[MovlenebiGaformdaGaukmdaKontrakti]  (nolock) AS [t12]
                        WHERE ([t12].[Statusi] = @p14) AND ([t12].[PolisisNomeri] = [t9].[PolisisNomeri])
                        ) THEN @p15
                    ELSE CONVERT(NVarChar(17),@p16)
                 END) AS [value3]
            FROM [dbo].[Polisebi]  (nolock) AS [t9]
            LEFT OUTER JOIN (
                SELECT 1 AS [test], [t10].[PolisisNomeri]
                FROM [dbo].[MovlenaChabarebebi]  (nolock) AS [t10]
                ) AS [t11] ON [t11].[PolisisNomeri] = [t9].[PolisisNomeri]
            WHERE ([t11].[test] IS NOT NULL) AND ([t9].[ProgramisId] < @p9) AND ([t9].[ShekmnisTarigi] <> @p10)
            ) AS [t13]
        UNION ALL
        SELECT [t14].[PolisisNomeri], (CONVERT(NVarChar,DATEPART(Year, [t14].[ShekmnisTarigi]))) + (
            (CASE 
                WHEN (CONVERT(Int,DATALENGTH(CONVERT(NVarChar,DATEPART(Month, [t14].[ShekmnisTarigi]))) / 2)) >= @p20 THEN CONVERT(NVarChar,DATEPART(Month, [t14].[ShekmnisTarigi]))
                ELSE REPLICATE(@p21, @p20 - (CONVERT(Int,DATALENGTH(CONVERT(NVarChar,DATEPART(Month, [t14].[ShekmnisTarigi]))) / 2))) + (CONVERT(NVarChar,DATEPART(Month, [t14].[ShekmnisTarigi])))
             END)) AS [value], @p22 AS [value2], (
            SELECT [t21].[Statusi]
            FROM (
                SELECT TOP (1) [t19].[Statusi]
                FROM [dbo].[PolisisChabarebisIstoria]  (nolock) AS [t19]
                WHERE ([t19].[VizitisTarigi] = ((
                    SELECT MIN([t20].[VizitisTarigi])
                    FROM [dbo].[PolisisChabarebisIstoria]  (nolock) AS [t20]
                    WHERE [t20].[PolisisNomeri] = [t14].[PolisisNomeri]
                    ))) AND ([t19].[PolisisNomeri] = [t14].[PolisisNomeri])
                ) AS [t21]
            ) AS [value3]
        FROM [dbo].[Polisebi]  (nolock) AS [t14]
        LEFT OUTER JOIN (
            SELECT 1 AS [test], [t15].[PolisisNomeri]
            FROM [dbo].[MovlenaChabarebebi]  (nolock) AS [t15]
            ) AS [t16] ON [t16].[PolisisNomeri] = [t14].[PolisisNomeri]
        WHERE (NOT (EXISTS(
            SELECT NULL AS [EMPTY]
            FROM [dbo].[PolisisChabarebisIstoria]  (nolock) AS [t17]
            WHERE ([t17].[Statusi] = @p17) AND ([t17].[PolisisNomeri] = [t14].[PolisisNomeri])
            ))) AND (EXISTS(
            SELECT NULL AS [EMPTY]
            FROM [dbo].[PolisisChabarebisIstoria]  (nolock) AS [t18]
            WHERE [t18].[PolisisNomeri] = [t14].[PolisisNomeri]
            )) AND ([t16].[test] IS NULL) AND ([t14].[ProgramisId] < @p18) AND ([t14].[ShekmnisTarigi] <> @p19)
        ) AS [t22]
    UNION ALL
    SELECT [t23].[PolisisNomeri], ((CONVERT(NVarChar,DATEPART(Year, [t23].[ShekmnisTarigi]))) + @p26) + (
        (CASE 
            WHEN (CONVERT(Int,DATALENGTH(CONVERT(NVarChar,DATEPART(Month, [t23].[ShekmnisTarigi]))) / 2)) >= @p27 THEN CONVERT(NVarChar,DATEPART(Month, [t23].[ShekmnisTarigi]))
            ELSE REPLICATE(@p28, @p27 - (CONVERT(Int,DATALENGTH(CONVERT(NVarChar,DATEPART(Month, [t23].[ShekmnisTarigi]))) / 2))) + (CONVERT(NVarChar,DATEPART(Month, [t23].[ShekmnisTarigi])))
         END)) AS [value], @p29 AS [value2], (
        SELECT [t32].[Statusi]
        FROM (
            SELECT TOP (1) [t30].[Statusi]
            FROM [dbo].[MovlenebiGaformdaGaukmdaKontrakti]  (nolock) AS [t30]
            WHERE ([t30].[Dro] = ((
                SELECT MAX([t31].[Dro])
                FROM [dbo].[MovlenebiGaformdaGaukmdaKontrakti]  (nolock) AS [t31]
                WHERE [t31].[PolisisNomeri] = [t23].[PolisisNomeri]
                ))) AND ([t30].[PolisisNomeri] = [t23].[PolisisNomeri])
            ) AS [t32]
        ) AS [value3]
    FROM [dbo].[Polisebi]  (nolock) AS [t23]
    LEFT OUTER JOIN (
        SELECT 1 AS [test], [t24].[PolisisNomeri]
        FROM [dbo].[MovlenaChabarebebi]  (nolock) AS [t24]
        ) AS [t25] ON [t25].[PolisisNomeri] = [t23].[PolisisNomeri]
    WHERE (((
        SELECT [t28].[Statusi]
        FROM (
            SELECT TOP (1) [t26].[Statusi]
            FROM [dbo].[MovlenebiGaformdaGaukmdaKontrakti]  (nolock) AS [t26]
            WHERE ([t26].[Dro] = ((
                SELECT MAX([t27].[Dro])
                FROM [dbo].[MovlenebiGaformdaGaukmdaKontrakti]  (nolock) AS [t27]
                WHERE [t27].[PolisisNomeri] = [t23].[PolisisNomeri]
                ))) AND ([t26].[PolisisNomeri] = [t23].[PolisisNomeri])
            ) AS [t28]
        )) <> @p23) AND (NOT (EXISTS(
        SELECT NULL AS [EMPTY]
        FROM [dbo].[PolisisChabarebisIstoria]  (nolock) AS [t29]
        WHERE [t29].[PolisisNomeri] = [t23].[PolisisNomeri]
        ))) AND ([t25].[test] IS NULL) AND ([t23].[ProgramisId] < @p24) AND ([t23].[ShekmnisTarigi] <> @p25)
    ) AS [t33]
go
DELETE PolisisMdebareoba FROM PolisisMdebareoba 
WHERE  ShekmnisPeriodi in ( 201108,
							201109,
							201110,
							201111,
							201112,
							201201,
							201202,
							201203)
go
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'...' WHERE DarigebisStatusi='...'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'ჩაბარდა' WHERE DarigebisStatusi='Chabarda'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'კონტრაქტი გაფორმდა' WHERE DarigebisStatusi='KontraktiGaformda'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'ვერ ჩაბარდა' WHERE DarigebisStatusi='VerChabarda'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'არარსებული მისამართი' WHERE DarigebisStatusi='ArasebuliMisamarti'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'არ დამხვდა' WHERE DarigebisStatusi='ArDamkhvda'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'არ ცხოვრობს აღ. მისამართზე' WHERE DarigebisStatusi='ArCkhovrobsAgnishnulMisamartze'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'უარი განაცხადა' WHERE DarigebisStatusi='UariGanackhada'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'გარდაიცვალა' WHERE DarigebisStatusi='Gardacvlilia'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'პირადი მონაც. არასწორია' WHERE DarigebisStatusi='MonacemebiArascoria'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'დაპატიმრებულია' WHERE DarigebisStatusi='Dapatimrebulia'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'ჯარშია' WHERE DarigebisStatusi='Jarshia'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'აღარ ცხოვრობს აღნიშნულ ოჯახში' WHERE DarigebisStatusi='AgarCxovrobsOjakhshi'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'ჩასაბარებელი' WHERE DarigebisStatusi='Chasabarebeli'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'რაიონი' WHERE DarigebisStatusi='Raioni'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'ფოსტა' WHERE DarigebisStatusi='Fosta'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'არ მოუნიშნიათ' WHERE DarigebisStatusi='ArMounishniat'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'გაუმდა' WHERE DarigebisStatusi='Gaukmda'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'გაუქმდა პოლ. ჩაუბარებლობის გამო' WHERE DarigebisStatusi='GaukmdaPolisisChaubareblobisGamo'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'დამთავრდა სად. პერიოდი' WHERE DarigebisStatusi='DamtavrdaSadazgvevoPeriodi'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisStatusi=N'გაუქმდა გარდაცვალების გამო' WHERE DarigebisStatusi='GaukmdaGardacvalebisGamo'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'...' WHERE DarigebisMdebareoba='...'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'ჩაბარდა' WHERE DarigebisMdebareoba='Chabarda'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'კონტრაქტი გაფორმდა' WHERE DarigebisMdebareoba='KontraktiGaformda'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'ვერ ჩაბარდა' WHERE DarigebisMdebareoba='VerChabarda'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'არარსებული მისამართი' WHERE DarigebisMdebareoba='ArasebuliMisamarti'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'არ დამხვდა' WHERE DarigebisMdebareoba='ArDamkhvda'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'არ ცხოვრობს აღ. მისამართზე' WHERE DarigebisMdebareoba='ArCkhovrobsAgnishnulMisamartze'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'უარი განაცხადა' WHERE DarigebisMdebareoba='UariGanackhada'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'გარდაიცვალა' WHERE DarigebisMdebareoba='Gardacvlilia'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'პირადი მონაც. არასწორია' WHERE DarigebisMdebareoba='MonacemebiArascoria'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'დაპატიმრებულია' WHERE DarigebisMdebareoba='Dapatimrebulia'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'ჯარშია' WHERE DarigebisMdebareoba='Jarshia'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'აღარ ცხოვრობს აღნიშნულ ოჯახში' WHERE DarigebisMdebareoba='AgarCxovrobsOjakhshi'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'ჩასაბარებელი' WHERE DarigebisMdebareoba='Chasabarebeli'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'რაიონი' WHERE DarigebisMdebareoba='Raioni'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'ფოსტა' WHERE DarigebisMdebareoba='Fosta'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'არ მოუნიშნიათ' WHERE DarigebisMdebareoba='ArMounishniat'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'გაუმდა' WHERE DarigebisMdebareoba='Gaukmda'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'გაუქმდა პოლ. ჩაუბარებლობის გამო' WHERE DarigebisMdebareoba='GaukmdaPolisisChaubareblobisGamo'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'დამთავრდა სად. პერიოდი' WHERE DarigebisMdebareoba='DamtavrdaSadazgvevoPeriodi'
UPDATE [dbo].[PolisisMdebareoba] SET DarigebisMdebareoba=N'გაუქმდა გარდაცვალების გამო' WHERE DarigebisMdebareoba='GaukmdaGardacvalebisGamo'