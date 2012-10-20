<Query Kind="Statements">
  <Connection>
    <ID>453dabbc-e8b3-4c04-8eec-536e8e4e7b58</ID>
    <Persist>true</Persist>
    <Server>triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <NoPluralization>true</NoPluralization>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA3XCANzX8A0+HtkxmZtXFGAAAAAACAAAAAAAQZgAAAAEAACAAAACp+sEJqJxOPz2BQ1lxRRtXH/XJirk9/kw8mJj0bPf5jQAAAAAOgAAAAAIAACAAAACZd/POK3nxMSCEaCgbVDIUSs/pSsra35l5MYJ8LGMjExAAAABDe6Hrhl+CP58Aq2DzWMGNQAAAAA6bC1RC4u17G4KQF35FDnGlInhYWmmeVWDNOqKPGjJdSaTe9PMIuov4DO+z/r3whP06rpngcdEwF0GcF80cZT0=</Password>
    <IncludeSystemObjects>true</IncludeSystemObjects>
    <Database>INSURANCEW</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

var ids=new List<int>{998839}; //{20,352,1,47645,23,471,2,236499};
				DAZGVEVA_06.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_06"})
		.Concat(DAZGVEVA_08.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_08"}))
		.Concat(DAZGVEVA_09.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_09"}))
		.Concat(DAZGVEVA_10.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_10"}))
		.Concat(DAZGVEVA_11.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_11"}))
		.Concat(DAZGVEVA_12.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_12"}))
	.Concat(DAZGVEVA_201101.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201101"}))
	.Concat(DAZGVEVA_201102.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201102"}))
	.Concat(DAZGVEVA_201103.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201103"}))
	.Concat(DAZGVEVA_201104.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201104"}))
	.Concat(DAZGVEVA_201105.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201105"}))
	.Concat(DAZGVEVA_201106.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201106"}))
	.Concat(DAZGVEVA_201107.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201107"}))
	.Concat(DAZGVEVA_201108.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201108"}))
	.Concat(DAZGVEVA_201109.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201109"}))
	.Concat(DAZGVEVA_201110.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201110"}))
	.Concat(DAZGVEVA_201111.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201111"}))
	.Concat(DAZGVEVA_201112.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201112"}))
	.Concat(DAZGVEVA_201201.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201201"}))
	.Concat(DAZGVEVA_201202.Where (x => ids.Contains(x.ID)).Select (x => new {x.ID, x.Unnom, x.PID, x.Dagv_tar, Table="DAZGVEVA_201202"}))
	.Select (x => new {x.ID,x.Dagv_tar,x.PID,x.Unnom})
	.Distinct().ToList().Dump().AsParallel()
	.GroupBy (x => x.ID)
	.Select (g => new {Id = g.Key, Raod=g.Count()})
	.GroupBy (g => g.Raod)
	.Select (g => new {g.Key, Id = g.First ().Id})
	.Take(150)
	.Dump();