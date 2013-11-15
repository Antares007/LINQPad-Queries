<Query Kind="Program">
  <Reference>D:\Dev\Libs\EPPlus\EPPlus.dll</Reference>
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	var con = this.Connection;
	con.Open();

	var periodi = "201310";
	var cinaPeriodi = DateTime.Parse(periodi.Insert(4,"-") + "-01").AddMonths(-1).ToString("yyyyMM").Dump();
//	sql.Replace("{periodi}", periodi).Dump();
//	return;
	var fileNames = string.Join(",", (new []{periodi,cinaPeriodi}).Select (x => string.Format("'GankhorcielebuliVizitebi_ALL_{0}'",x)));
	var misacemi = (
	from x in con.Query(@"select * from [GankhorcielebuliVizitebi] where FileName in ("+fileNames+") and VizitisPeriodi=" + cinaPeriodi)
	group x by x.PaketisNomeri into g
	let f = g.First ()
	select new {
			PaketisNomeri=(string)g.Key,
			Chabarda=(string)f.Chabarda,
			KontraktiGaformda=(string)f.KontraktiGaformda,
			Chambarebeli=(string)f.Chambarebeli,
			Periodi=(string)f.Periodi, 
			Polisebi = string.Join(", ", g.Select (pol => (string)pol.PolisisNomeri)),
			Dadgenileba=(string)f.Dadgenileba,
			VizitisTarigi=(string)f.VizitisTarigi,
			VizitisPeriodi=(string)f.VizitisPeriodi,
		}).ToList();
	var gamosartmeviPaketebi=(
	from x in con.Query(@"select * from [ChabarebuliPaketebi] where FileName = 'ChabarebuliPaketebi_ALL_"+periodi+"_Yvela'")
	group x by new {x.PaketisNomeri, x.MzgveveliKompaniisKodi} into g
	let f = g.First ()
	select new {
			MzgveveliKompaniisKodi=(string)g.Key.MzgveveliKompaniisKodi,
			Periodi=(string)f.Periodi,
			PaketisNomeri=(string)g.Key.PaketisNomeri,
			Dadgenileba=(string)f.Dadgenileba,
			VizitisTarigi=(string)f.VizitisTarigi,
			Polisebi=string.Join(", ", g.Select (pol => (string)pol.PolisisNomeri))
			}).ToList();
	foreach (var dad in new []{"218", "165"})
	{
		Ext.ToExcel(misacemi.Where (m => m.Dadgenileba == dad), "GankhorcielebuliVizitebi_v2_" + dad + "_" + periodi);
//		Ext.ToExcel(gamosartmeviPaketebi      .Where (m => m.Dadgenileba == dad), "ChabarebuliPaketebi_" + dad + "_"  + periodi + "_Yvela");
//		foreach(var g in gamosartmeviPaketebi .Where (m => m.Dadgenileba == dad) .GroupBy (g => g.MzgveveliKompaniisKodi))
//			Ext.ToExcel(g, "ChabarebuliPaketebi_" + dad.ToString() + "_" + periodi + "_" + g.Key);
	}
}

public static class Ext {
	public static IEnumerable<T> ToExcel<T>(IEnumerable<T> items, string filename=null)
	{
		using (ExcelPackage p = new ExcelPackage())
		{
			p.Workbook.Worksheets.Add("Data");
			ExcelWorksheet ws = p.Workbook.Worksheets[1];
			ws.Name = "Data"; //Setting Sheet's name
			var columns = typeof(T).GetProperties().Where (x => x.CanRead && (x.PropertyType.IsValueType||x.PropertyType==typeof(string))).ToDictionary(k=>k.Name);
	
			int colIndex = 1;
			int rowIndex = 1;
	
			
			foreach (var dc in columns.Keys) //Creating Headings
			{
				var cell = ws.Cells[rowIndex, colIndex];
				var fill = cell.Style.Fill;
				fill.PatternType = ExcelFillStyle.Solid;
				fill.BackgroundColor.SetColor(Color.Gray);
				var border = cell.Style.Border;
				border.Bottom.Style = 
					border.Top.Style = 
					border.Left.Style = 
					border.Right.Style = ExcelBorderStyle.Thin;
				cell.Value = dc;
				colIndex++;
			}
			foreach (var dr in items) 
			{
				colIndex = 1;
				rowIndex++;
				foreach (var colName  in columns.Keys)
				{
					var cell = ws.Cells[rowIndex, colIndex];
					cell.Value = columns[colName].GetValue(dr, null);
					var border = cell.Style.Border;
					border.Left.Style =
						border.Right.Style = ExcelBorderStyle.Thin;
					colIndex++;
				}
			}
			colIndex = 0;
			Byte[] bin = p.GetAsByteArray();
			string file = "c:\\temp\\" + (filename?? Guid.NewGuid().ToString()) + ".xlsx";
			File.WriteAllBytes(file, bin);
		}
		return items;
	}
}


static string sql = @"
WITH gadakhdili as (
    SELECT i.PolisisNomeri 
    FROM PolisisChabarebisIstoria i 
    where exists (  select null 
                    from ChabarebuliPaketebi  cp 
                    where cp.PaketisNomeri = i.PaketisNomeri)
)
select * into gadakhdili_ from gadakhdili;
with gamosartmevi as (
    SELECT * 
    FROM  vPolisisChabarebisIstoria i
    WHERE ShekmnisTarigi > '20121001'
      AND NOT EXISTS (
            SELECT null
            FROM gadakhdili_ gv
            WHERE gv.PolisisNomeri = i.PolisisNomeri 
            )
), gamosartmeviPaketebi AS (
	SELECT   c.MzgveveliKompaniisKodi
            ,c.[PaketisNomeri]
            ,c.[Dadgenileba]
            ,case when chabarda.PaketisNomeri is not null then 'Chabarda' else 'VerChabarda' end Chabarda
            ,case when s.[GaformdaKontrakti] = 1 then 'KontraktiGaformda' else 'KontraktiVerGaformda' end KontraktiGaformda
            ,c.[Chambarebeli]
            ,c.[VizitisPeriodi]
            ,c.[PolisisNomeri]
            ,c.[Statusi] ChabarebisStatusi
            ,c.[PolisisStatusi]
            ,c.Shigtavsi
	FROM gamosartmevi c
	join	  (	SELECT PaketisNomeri, max(ShekmnisTarigi) ShekmnisTarigi, max([GaformdaKontrakti]) [GaformdaKontrakti]
				FROM gamosartmevi 
				group By PaketisNomeri) s on s.PaketisNomeri = c.PaketisNomeri
	left join (	select distinct PaketisNomeri 
				from gamosartmevi where Statusi='Chabarda') chabarda on chabarda.PaketisNomeri = c.PaketisNomeri
)
SELECT  * into gamosartmeviPaketebi_ from  gamosartmeviPaketebi 
where  KontraktiGaformda = 'KontraktiGaformda' and ChabarebisStatusi = 'Chabarda';
drop table gadakhdili_;
with amodubluliGadasakhdeliPaketebi as (
    select gp.* 
    from (
        SELECT  PolisisNomeri,max(PaketisNomeri) PaketisNomeri
        from  gamosartmeviPaketebi_ 
        group by PolisisNomeri) boloPaketi
    outer apply 
        (SELECT  *
        from  gamosartmeviPaketebi_ t
        where t.PolisisNomeri=boloPaketi.PolisisNomeri and t.PaketisNomeri = boloPaketi.PaketisNomeri) gp
)
INSERT INTO [dbo].[ChabarebuliPaketebi]
           ([FileName],                       [MzgveveliKompaniisKodi],[Periodi],[PaketisNomeri],[Dadgenileba],[VizitisTarigi],[PolisisNomeri],[ChabarebisStatusi],[PolisisStatusi],[Shigtavsi])
select 'ChabarebuliPaketebi_ALL_{periodi}_Yvela',[MzgveveliKompaniisKodi],'{periodi}', [PaketisNomeri],[Dadgenileba],'',             [PolisisNomeri],[ChabarebisStatusi],[PolisisStatusi],[Shigtavsi] 
from amodubluliGadasakhdeliPaketebi;
drop table gamosartmeviPaketebi_;




WITH gadakhdili as (
    SELECT i.PolisisNomeri, CASE WHEN i.[Chambarebeli] in ('Fosta','Banki') THEN i.[Chambarebeli] ELSE 'AdgilidanGacema' END AS [Chambarebeli]
    FROM PolisisChabarebisIstoria i 
    WHERE exists (  select null 
                    from GankhorcielebuliVizitebi gv
                    where   (     (KontraktiGaformda = 'KontraktiGaformda' AND ChabarebisStatusi = 'Chabarda')
                             or   (KontraktiGaformda = 'KontraktiGaformda' AND Chambarebeli = 'Fosta' AND VizitisPeriodi < 201301)
                             or   (Chambarebeli = 'Banki' AND ChabarebisStatusi = 'Chabarda' AND VizitisPeriodi < 201301)
                            )
                        and i.PaketisNomeri = gv.PaketisNomeri
                 )
)
select * into gadakhdili_ from gadakhdili ;
with gadasakhdeli as (
    SELECT * 
    FROM  vPolisisChabarebisIstoria i
    WHERE ShekmnisTarigi > '20121001'
      AND NOT EXISTS (
            SELECT null
            FROM gadakhdili_ gv
            WHERE   gv.Chambarebeli = i.Chambarebeli AND gv.PolisisNomeri = i.PolisisNomeri
            )
), gadasakhdeliPaketebi AS (
	SELECT   c.[PaketisNomeri]
            ,case when chabarda.PaketisNomeri is not null then 'Chabarda' else 'VerChabarda' end Chabarda
            ,case when s.[GaformdaKontrakti] = 1 then 'KontraktiGaformda' else 'KontraktiVerGaformda' end KontraktiGaformda
            ,c.[Chambarebeli]
            ,c.[Dadgenileba]
            ,c.[VizitisPeriodi]
            ,c.[PolisisNomeri]
            ,c.[Statusi] ChabarebisStatusi
            ,c.[PolisisStatusi]
            ,c.Shigtavsi
	FROM gadasakhdeli c
	join	  (	SELECT PaketisNomeri, max(ShekmnisTarigi) ShekmnisTarigi, max([GaformdaKontrakti]) [GaformdaKontrakti]
				FROM gadasakhdeli 
				group By PaketisNomeri) s on s.PaketisNomeri = c.PaketisNomeri
	left join (	select distinct PaketisNomeri 
				from gadasakhdeli where Statusi='Chabarda') chabarda on chabarda.PaketisNomeri = c.PaketisNomeri
)
SELECT  * into gadasakhdeliPaketebi_ from  gadasakhdeliPaketebi 
where  (    (KontraktiGaformda = 'KontraktiGaformda' AND ChabarebisStatusi = 'Chabarda')
       or   (KontraktiGaformda = 'KontraktiGaformda' AND Chambarebeli = 'Fosta' AND VizitisPeriodi < 201301)
       or   (Chambarebeli = 'Banki' AND ChabarebisStatusi = 'Chabarda' AND VizitisPeriodi < 201301)
       );
drop table gadakhdili_;
with amodubluliGadasakhdeliPaketebi as (
    select gp.* 
    from (
        SELECT  PolisisNomeri,Chambarebeli,max(PaketisNomeri) PaketisNomeri
        from  gadasakhdeliPaketebi_ 
        group by PolisisNomeri,Chambarebeli ) boloPaketi
    outer apply 
        (SELECT  *
        from  gadasakhdeliPaketebi_ t
        where t.PolisisNomeri=boloPaketi.PolisisNomeri and t.Chambarebeli = boloPaketi.Chambarebeli and t.PaketisNomeri = boloPaketi.PaketisNomeri) gp
)
INSERT INTO [dbo].[GankhorcielebuliVizitebi]
           (FileName,                        PaketisNomeri,Chabarda,KontraktiGaformda,Chambarebeli,Periodi, Dadgenileba,VizitisTarigi,VizitisPeriodi,PolisisNomeri,ChabarebisStatusi,PolisisStatusi,Shigtavsi)
select 'GankhorcielebuliVizitebi_ALL_{periodi}',PaketisNomeri,Chabarda,KontraktiGaformda,Chambarebeli,'{periodi}',Dadgenileba,'',           VizitisPeriodi,PolisisNomeri,ChabarebisStatusi,PolisisStatusi,Shigtavsi 
from amodubluliGadasakhdeliPaketebi;
drop table gadasakhdeliPaketebi_ ;
";