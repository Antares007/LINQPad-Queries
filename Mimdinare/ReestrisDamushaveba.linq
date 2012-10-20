<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>JUST</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{
	var infschema=this.ExecuteQuery<Column>(@"SELECT 'JUST' as DB,C.TABLE_NAME collate Latin1_General_BIN as TABLE_NAME,C.COLUMN_NAME collate Latin1_General_BIN as COLUMN_NAME,C.COLLATION_NAME collate Latin1_General_BIN as COLLATION_NAME FROM JUST.INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME LIKE 'JUST_20%'
UNION
SELECT 'UketesiReestri',C.TABLE_NAME,C.COLUMN_NAME,C.COLLATION_NAME FROM UketesiReestri.INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME LIKE 'JUST_20%'
").ToList();
	

	(from i in infschema
	group i by new {i.DB,i.TABLE_NAME} into g
	
	select new {
				DB=g.Key.DB, 
	            TNAME=g.Key.TABLE_NAME, 
				tarigi=g.Key.TABLE_NAME.Substring(5,8),
				hasAppd=g.Any(x=>x.COLUMN_NAME=="APPD_STATUS_DESCRIPTION"),
				isUnicode = g.Any(x=>x.COLUMN_NAME=="APPD_STATUS_DESCRIPTION") && this.ExecuteQuery<int>("select count(*) rownumber  from (SELECT top 1 APPD_STATUS_DESCRIPTION FROM "+g.Key.DB+".dbo."+g.Key.TABLE_NAME+" WHERE APPD_STATUS_DESCRIPTION like N'%ა%') a").Single()>0,
			})
	.Where(x=>x.hasAppd)
	.OrderBy(x=>x.tarigi).Dump()
	.Select(x=>@"
INSERT INTO PirAppdCvlilebeli(PID,APPD_STATUS_DESCRIPTION,Tarigi)
SELECT PID,APPD_STATUS_DESCRIPTION,'"+x.tarigi+@"' FROM (
						SELECT PID collate Latin1_General_BIN PID,APPD_STATUS_DESCRIPTION collate Latin1_General_BIN as APPD_STATUS_DESCRIPTION FROM "+x.DB+@".dbo."+x.TNAME+@" WHERE ISNUMERIC(PID)=1 AND APPD_STATUS_DESCRIPTION is not null
						EXCEPT 
						SELECT PID collate Latin1_General_BIN,APPD_STATUS_DESCRIPTION collate Latin1_General_BIN 
						FROM PirAppdCvlilebeli yvela
						join (SELECT Max(Id) Id FROM PirAppdCvlilebeli GROUP BY PID) bolo on yvela.Id=bolo.Id
						) R
GO
")
			
	.Dump();
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	return;
	
	
	
	
	
	var allCols=infschema.Select (x => x.COLUMN_NAME).Distinct();
	
	allCols.SelectMany(c=>infschema.Where (i => i.COLUMN_NAME==c)
		.Select (i => new {c,i.TABLE_NAME}))
		.GroupBy (c => c.c)
		.Select (g => new {g.Key,Raod=g.Count (),MinTname=g.Min (x => x.TABLE_NAME)})
		.OrderBy (g => g.Key);
	var colsDic = infschema.GroupBy (i => i.TABLE_NAME).ToDictionary (k => k.Key, v=> v.Select (x => x.COLUMN_NAME).ToList());	

	var cols = (new []{
				"PID",
				"APPD_DATE",
				"APPD_DUE_DATE",
				"APPD_STATUS_DESCRIPTION",
				"CONDITION_ID",
				"CONDITION_DESCRIPTION",
				"STATUS_ID",
				"STATUS_DESCRIPTION",
				"DEATH_DATE",
				"DEATH_REGISTRATION_DATE",
				"SUB_TYPE_ID",
				"SUB_TYPE",
				});
	cols.SelectMany (c => colsDic.SelectMany (d => d.Value.Where(v=>v==c).DefaultIfEmpty().Select (v => new{d.Key,v,c})))
			.GroupBy (c => c.Key)
			.Select (g => new{g.Key,Cols=string.Join(", ",g.Select (x => x.v==null?"NULL as "+x.c:x.c))})
			.OrderBy (g => g.Key)
	.Select((t,i)=>@"
GO
DECLARE @ts DATETIME
SELECT @ts='" + t.Key.Substring(5,8) + @"' 
PRINT CONVERT(VARCHAR, @ts, 112) + ' "+(i+1)+@"'
INSERT INTO JSAttrs ([PID],  [APPD_DATE],  [APPD_DUE_DATE],  [APPD_STATUS_DESCRIPTION],  [CONDITION_ID],  [CONDITION_DESCRIPTION],  [STATUS_ID],  [STATUS_DESCRIPTION],  [DEATH_DATE],  [DEATH_REGISTRATION_DATE],  [SUB_TYPE_ID],  [SUB_TYPE], Dro)
SELECT             l.[PID],l.[APPD_DATE],l.[APPD_DUE_DATE],l.[APPD_STATUS_DESCRIPTION],l.[CONDITION_ID],l.[CONDITION_DESCRIPTION],l.[STATUS_ID],l.[STATUS_DESCRIPTION],l.[DEATH_DATE],l.[DEATH_REGISTRATION_DATE],l.[SUB_TYPE_ID],l.[SUB_TYPE], @ts as dro  
FROM  (
		SELECT "+t.Cols+@" 
		FROM JUST.[dbo]." + t.Key + @" WHERE ISNUMERIC(PID) = 1
	  ) l left join
	  (
		SELECT PID, APPD_DATE, APPD_DUE_DATE, APPD_STATUS_DESCRIPTION, CONDITION_ID, CONDITION_DESCRIPTION, STATUS_ID, STATUS_DESCRIPTION, DEATH_DATE, DEATH_REGISTRATION_DATE, SUB_TYPE_ID, SUB_TYPE 
		FROM JSAttrs jsa join (SELECT MAX(Id) LastId FROM JSAttrs GROUP BY PID) lst ON jsa.Id = lst.LastId
	  ) r
  on  l.PID collate Latin1_General_BIN= r.PID collate Latin1_General_BIN
  and (l.APPD_DATE = r.APPD_DATE or (l.APPD_DATE is null and r.APPD_DATE is null)) 
  and (l.APPD_DUE_DATE = r.APPD_DUE_DATE or (l.APPD_DUE_DATE is null and r.APPD_DUE_DATE is null)) 
  and (l.CONDITION_ID = r.CONDITION_ID or (l.CONDITION_ID is null and r.CONDITION_ID is null)) 
  and (l.STATUS_ID = r.STATUS_ID or (l.STATUS_ID is null and r.STATUS_ID is null)) 
  and (l.DEATH_DATE = r.DEATH_DATE or (l.DEATH_DATE is null and r.DEATH_DATE is null)) 
  and (l.DEATH_REGISTRATION_DATE = r.DEATH_REGISTRATION_DATE or (l.DEATH_REGISTRATION_DATE is null and r.DEATH_REGISTRATION_DATE is null)) 
  and (l.SUB_TYPE_ID = r.SUB_TYPE_ID or (l.SUB_TYPE_ID is null and r.SUB_TYPE_ID is null)) 
  and (l.APPD_STATUS_DESCRIPTION collate Latin1_General_BIN = r.APPD_STATUS_DESCRIPTION  collate Latin1_General_BIN or (l.APPD_STATUS_DESCRIPTION is null and r.APPD_STATUS_DESCRIPTION is null)) 
WHERE r.PID is null
")
.Select (x => x.Replace("NULL as APPD_STATUS_DESCRIPTION","cast(NULL as varchar) as APPD_STATUS_DESCRIPTION"))
.Dump();

	return;
	var unicodeTables= this.ExecuteQuery<TableName>(string.Join(" UNION ALL ", infschema
													.Select (i => i.TABLE_NAME)
													.Distinct()
													.Select (tn => "SELECT '"+tn+"' as TABLE_NAME FROM (SELECT TOP (100) [t0].[FIRST] FROM "+tn+" AS [t0] ) AS [t1] WHERE [t1].[FIRST] LIKE N'%ა%'\n")
													))
									.GroupBy (tn => tn.TABLE_NAME)
									.Where (g => g.Count ()>10)
									.Select (g => g.Key)
									.Distinct().ToList();
	Func<bool,Func<string,string>> ccr = (isUnicode)=>{
		if(isUnicode)
			return (col)=>"SAESA2005.dbo.fn_conutf82ascii("+col+")";
		return (col)=> col;
	}; 
	
	infschema
		.GroupBy (c => c.TABLE_NAME)
		.Select (g => new { 
				Name=g.Key, 
				Cols=g.Select (x => new {x.COLUMN_NAME,x.COLLATION_NAME}),
				ColDecorator=ccr(unicodeTables.Contains(g.Key))
				})
		.OrderBy (t => t.Name.ToUpper())
		.Select ((t,i) => 
		@"
GO
DECLARE @ts DATETIME
SELECT @ts='" + t.Name.Substring(5,8) + @"' 
PRINT CONVERT(VARCHAR, @ts, 112) + ' "+(i+1)+@"'
INSERT INTO JS (PID, [FIRST], [LAST], BIRTH_DATE, Dro)
SELECT l.p,l.f,l.l,l.b,@ts as dro  
FROM	(SELECT j.PID collate Latin1_General_BIN p, "+t.ColDecorator("RTRIM(LTRIM(j.FIRST))")+@" collate Latin1_General_BIN f, "+t.ColDecorator("RTRIM(LTRIM(j.LAST))")+@" collate Latin1_General_BIN l, j.BIRTH_DATE b FROM JUST.[dbo]." + t.Name + @" j WHERE ISNUMERIC(j.PID) = 1) l left join
		(SELECT JS.PID,[FIRST],[LAST],[BIRTH_DATE] FROM JS join (SELECT MAX(Id) LastId FROM JS GROUP BY PID) l ON js.Id=l.LastId) r
		on	l.p  = PID collate Latin1_General_BIN
		and (l.f = [FIRST] collate Latin1_General_BIN     or (l.f is null and [FIRST] is null)) 
		and (l.l = [LAST] collate Latin1_General_BIN      or (l.l is null and [LAST] is null)) 
		and (l.b = [BIRTH_DATE] or (l.b is null and [BIRTH_DATE] is null))
WHERE r.PID is null
")
		.Dump();
	
}

public class Column
{
	public string DB {get;set;}
	public string TABLE_NAME {get;set;}
	public string COLUMN_NAME {get;set;}
	public string COLLATION_NAME {get;set;}
}
public class TableName
{
	public string TABLE_NAME {get;set;}
}