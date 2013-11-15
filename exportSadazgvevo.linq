<Query Kind="Program" />



string Daagzavne(string sql, string dasakheleba, string ganmarteba)
{
//	var ckhrilisMisamarti = dasakheleba + "_" + DateTime.Now.ToString("yyyyMMdd_HH_mm_ss");
//	mocheriCxrilshi(sql, ckhrilisSakheli);
//	return ckhrilisMisamarti;
return "";
}

void Main()
{


//DateTime.Now.ToString("yyyyMMdd_HH_mm_ss").Dump();
//	var ckhrilisMisamarti = Daagzavne(sql, "ბაზები", "მოჭრილი ბაზა");

	
	
//	   DirSearch(@"\\172.17.7.40\სადაზღვევოებს","*.accdb")
//			.Select (x => new FileInfo(x))
//			.Select (x => new {x.FullName, CreationTime=x.CreationTime.ToString("yyyyMMdd_HH_mm_ss")})
//			.ToList().Dump();
}

IEnumerable<string> DirSearch(string sDir, string searchPattern="*.xls") 
{
   var dirs = Directory.GetDirectories(sDir).AsEnumerable();
   if(Directory.Exists(sDir))
	   dirs = new []{sDir}.Concat(dirs);
   foreach (string d in dirs) 
   {
		foreach (string f in Directory.GetFiles(d, searchPattern)) 
		{
			yield return f;
		}
		DirSearch(d);
   }
}

string sql = @"
SELECT        ik.MzgveveliKompaniisKodi
            , g.ID, g.Base_type, g.Base_Description
            
            , null as REGION_ID
            , CASE WHEN mis.ID IS NULL THEN g.RAI          ELSE mis.RAI          END RAI
            , CASE WHEN mis.ID IS NULL THEN g.RAI_NAME     ELSE mis.RAI_NAME     END RAI_NAME
            , CASE WHEN mis.ID IS NULL THEN g.CITY         ELSE mis.CITY         END CITY
            , CASE WHEN mis.ID IS NULL THEN g.VILLAGE      ELSE mis.VILLAGE      END VILLAGE
            , CASE WHEN mis.ID IS NULL THEN g.ADDRESS_FULL ELSE mis.ADDRESS_FULL END ADDRESS_FULL

            , g.SAG_DACESEBULEBA 
            
            , g.Unnom
            , CASE WHEN u.Unnom IS NULL THEN g.PID        ELSE u.PID collate Latin1_General_BIN2        END PID
            , CASE WHEN u.Unnom IS NULL THEN g.FIRST_NAME ELSE u.FIRST_NAME collate Latin1_General_BIN2 END FIRST_NAME
            , CASE WHEN u.Unnom IS NULL THEN g.LAST_NAME  ELSE u.LAST_NAME collate Latin1_General_BIN2  END LAST_NAME
            , CASE WHEN u.Unnom IS NULL THEN g.BIRTH_DATE ELSE u.BIRTH_DATE                             END BIRTH_DATE
            , CASE WHEN u.Unnom IS NULL THEN g.Sqesi      ELSE u.Sex                                    END Sex
            
            , g.Company_ID, g.Company 
            , g.[dagv-tar],g.End_Date 
            , g.ADD_DATE,g.STOP_DATE 
            , g.PolisisNomeri 
            , g.STATE,g.[201308]
--SELECT COUNT(*)
--SELECT DISTINCT g.STATE
FROM        INSURANCEW.dbo.[165_DadgenilebisFarglebshiDasazgvevBeneficiartaSia_20130802] g  
JOIN        SocialuriDazgveva.dbo.MzgvevelisIdDaKodi ik on ik.Id = g.Company_ID
LEFT JOIN   UketesiReestri.dbo.vUnnomBoloKargiChanaceri u on g.Unnom = u.Unnom
LEFT JOIN   INSURANCEW.dbo.aMisamartebi mis on g.ID = mis.ID
";
// Define other methods and classes here
