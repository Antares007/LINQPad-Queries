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
    <Database>INSURANCEW_LOG</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

	    FAMILY_DATA_07.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201007})
.Concat(FAMILY_DATA_08.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201008}))
.Concat(FAMILY_DATA_09.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201009}))
.Concat(FAMILY_DATA_10.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201010}))
.Concat(FAMILY_DATA_11.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201011}))
.Concat(FAMILY_DATA_12.Select (x => new {x.ID, x.PID,x.FID,x.FAMILY_SCORE,x.VISIT_DATE,Periodi=201012}))
.Take(1)