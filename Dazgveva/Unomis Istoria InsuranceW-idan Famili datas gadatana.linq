<Query Kind="Expression">
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

		FAMILY_DATA_201101.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201101})
.Concat(FAMILY_DATA_201102.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201102}))
.Concat(FAMILY_DATA_201103.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201103}))
.Concat(FAMILY_DATA_201104.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201104}))
.Concat(FAMILY_DATA_201105.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201105}))
.Concat(FAMILY_DATA_201106.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201106}))
.Concat(FAMILY_DATA_201107.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201107}))
.Concat(FAMILY_DATA_201108.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201108}))
.Concat(FAMILY_DATA_201109.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201109}))
.Concat(FAMILY_DATA_201110.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201110}))
.Concat(FAMILY_DATA_201111.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201111}))
.Concat(FAMILY_DATA_201112.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201112}))
.Concat(FAMILY_DATA_201201.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201201}))
.Concat(FAMILY_DATA_201202.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201202}))


.Distinct()
.Take(10)