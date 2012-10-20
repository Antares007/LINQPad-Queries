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

var daz=DAZGVEVA_201202.Take(13);//.Where (x => x.ID%10==0);
	    daz.Select (c => new {c.ID, Periodi=201006, STATE=c.STATE_06,ADD_DATE=c.ADD_DATE_06,CONTINUE_DATE=c.CONTINUE_DATE_06,STOP_DATE=c.STOP_DATE_06,Comment=c.Comment_06,Company=c.Company_06,Company_ID=c.Company_ID_06,Tankha=c._201006})
.Concat(daz.Select (c => new {c.ID, Periodi=201007, STATE=c.STATE_07,ADD_DATE=c.ADD_DATE_07,CONTINUE_DATE=c.CONTINUE_DATE_07,STOP_DATE=c.STOP_DATE_07,Comment=c.Comment_07,Company=c.Company_07,Company_ID=c.Company_ID_07,Tankha=c._201007}))
.Concat(daz.Select (c => new {c.ID, Periodi=201008, STATE=c.STATE_08,ADD_DATE=c.ADD_DATE_08,CONTINUE_DATE=c.CONTINUE_DATE_08,STOP_DATE=c.STOP_DATE_08,Comment=c.Comment_08,Company=c.Company_08,Company_ID=c.Company_ID_08,Tankha=c._201008}))
.Concat(daz.Select (c => new {c.ID, Periodi=201009, STATE=c.STATE_09,ADD_DATE=c.ADD_DATE_09,CONTINUE_DATE=c.CONTINUE_DATE_09,STOP_DATE=c.STOP_DATE_09,Comment=c.Comment_09,Company=c.Company_09,Company_ID=c.Company_ID_09,Tankha=c._201009}))
.Concat(daz.Select (c => new {c.ID, Periodi=201010, STATE=c.STATE_10,ADD_DATE=c.ADD_DATE_10,CONTINUE_DATE=c.CONTINUE_DATE_10,STOP_DATE=c.STOP_DATE_10,Comment=c.Comment_10,Company=c.Company_10,Company_ID=c.Company_ID_10,Tankha=c._201010}))
.Concat(daz.Select (c => new {c.ID, Periodi=201011, STATE=c.STATE_11,ADD_DATE=c.ADD_DATE_11,CONTINUE_DATE=c.CONTINUE_DATE_11,STOP_DATE=c.STOP_DATE_11,Comment=c.Comment_11,Company=c.Company_11,Company_ID=c.Company_ID_11,Tankha=c._201011}))
.Concat(daz.Select (c => new {c.ID, Periodi=201012, STATE=c.STATE_12,ADD_DATE=c.ADD_DATE_12,CONTINUE_DATE=c.CONTINUE_DATE_12,STOP_DATE=c.STOP_DATE_12,Comment=c.Comment_12,Company=c.Company_12,Company_ID=c.Company_ID_12,Tankha=c._201012}))
.Concat(daz.Select (c => new {c.ID, Periodi=201101, STATE=c.STATE_201101,ADD_DATE=c.ADD_DATE_201101,CONTINUE_DATE=c.CONTINUE_DATE_201101,STOP_DATE=c.STOP_DATE_201101,Comment=c.Comment_201101,Company=c.Company_201101,Company_ID=c.Company_ID_201101,Tankha=c._201101}))
.Concat(daz.Select (c => new {c.ID, Periodi=201102, STATE=c.STATE_201102,ADD_DATE=c.ADD_DATE_201102,CONTINUE_DATE=c.CONTINUE_DATE_201102,STOP_DATE=c.STOP_DATE_201102,Comment=c.Comment_201102,Company=c.Company_201102,Company_ID=c.Company_ID_201102,Tankha=c._201102}))
.Concat(daz.Select (c => new {c.ID, Periodi=201103, STATE=c.STATE_201103,ADD_DATE=c.ADD_DATE_201103,CONTINUE_DATE=c.CONTINUE_DATE_201103,STOP_DATE=c.STOP_DATE_201103,Comment=c.Comment_201103,Company=c.Company_201103,Company_ID=c.Company_ID_201103,Tankha=c._201103}))
.Concat(daz.Select (c => new {c.ID, Periodi=201104, STATE=c.STATE_201104,ADD_DATE=c.ADD_DATE_201104,CONTINUE_DATE=c.CONTINUE_DATE_201104,STOP_DATE=c.STOP_DATE_201104,Comment=c.Comment_201104,Company=c.Company_201104,Company_ID=c.Company_ID_201104,Tankha=c._201104}))
.Concat(daz.Select (c => new {c.ID, Periodi=201105, STATE=c.STATE_201105,ADD_DATE=c.ADD_DATE_201105,CONTINUE_DATE=c.CONTINUE_DATE_201105,STOP_DATE=c.STOP_DATE_201105,Comment=c.Comment_201105,Company=c.Company_201105,Company_ID=c.Company_ID_201105,Tankha=c._201105}))
.Concat(daz.Select (c => new {c.ID, Periodi=201106, STATE=c.STATE_201106,ADD_DATE=c.ADD_DATE_201106,CONTINUE_DATE=c.CONTINUE_DATE_201106,STOP_DATE=c.STOP_DATE_201106,Comment=c.Comment_201106,Company=c.Company_201106,Company_ID=c.Company_ID_201106,Tankha=c._201106}))
.Concat(daz.Select (c => new {c.ID, Periodi=201107, STATE=c.STATE_201107,ADD_DATE=c.ADD_DATE_201107,CONTINUE_DATE=c.CONTINUE_DATE_201107,STOP_DATE=c.STOP_DATE_201107,Comment=c.Comment_201107,Company=c.Company_201107,Company_ID=c.Company_ID_201107,Tankha=c._201107}))
.Concat(daz.Select (c => new {c.ID, Periodi=201108, STATE=c.STATE_201108,ADD_DATE=c.ADD_DATE_201108,CONTINUE_DATE=c.CONTINUE_DATE_201108,STOP_DATE=c.STOP_DATE_201108,Comment=c.Comment_201108,Company=c.Company_201108,Company_ID=c.Company_ID_201108,Tankha=c._201108}))
.Concat(daz.Select (c => new {c.ID, Periodi=201109, STATE=c.STATE_201109,ADD_DATE=c.ADD_DATE_201109,CONTINUE_DATE=c.CONTINUE_DATE_201109,STOP_DATE=c.STOP_DATE_201109,Comment=c.Comment_201109,Company=c.Company_201109,Company_ID=c.Company_ID_201109,Tankha=c._201109}))
.Concat(daz.Select (c => new {c.ID, Periodi=201110, STATE=c.STATE_201110,ADD_DATE=c.ADD_DATE_201110,CONTINUE_DATE=c.CONTINUE_DATE_201110,STOP_DATE=c.STOP_DATE_201110,Comment=c.Comment_201110,Company=c.Company_201110,Company_ID=c.Company_ID_201110,Tankha=c._201110}))
.Concat(daz.Select (c => new {c.ID, Periodi=201111, STATE=c.STATE_201111,ADD_DATE=c.ADD_DATE_201111,CONTINUE_DATE=c.CONTINUE_DATE_201111,STOP_DATE=c.STOP_DATE_201111,Comment=c.Comment_201111,Company=c.Company_201111,Company_ID=c.Company_ID_201111,Tankha=c._201111}))
.Concat(daz.Select (c => new {c.ID, Periodi=201112, STATE=c.STATE_201112,ADD_DATE=c.ADD_DATE_201112,CONTINUE_DATE=c.CONTINUE_DATE_201112,STOP_DATE=c.STOP_DATE_201112,Comment=c.Comment_201112,Company=c.Company_201112,Company_ID=c.Company_ID_201112,Tankha=c._201112}))
.Concat(daz.Select (c => new {c.ID, Periodi=201201, STATE=c.STATE,ADD_DATE=c.ADD_DATE,CONTINUE_DATE=c.CONTINUE_DATE,STOP_DATE=c.STOP_DATE,Comment=c.Comment,Company=c.Company,Company_ID=c.Company_ID,Tankha=c._201201}))
.Concat(daz.Select (c => new {c.ID, Periodi=201202, STATE=c.STATE_201202,ADD_DATE=c.ADD_DATE_201202_TMP,CONTINUE_DATE=c.CONTINUE_DATE_201101,STOP_DATE=c.STOP_DATE_201202_TMP,Comment=c.Comment_201202_TMP,Company=c.Company_201202,Company_ID=c.Company_ID_201202,Tankha=c._201202}))
.Where (x => !(x.ADD_DATE == null && x.Comment == null && x.Company == null && x.Company_ID == null && x.CONTINUE_DATE == null && x.STATE == null && x.STOP_DATE == null && x.Tankha == null))
.GroupBy (x => new {x.ADD_DATE,x.Comment,x.Company,x.Company_ID,x.CONTINUE_DATE,x.STATE,x.STOP_DATE,x.Tankha,x.ID})
.Select (g => new {g.Key.ID,Tve=g.Min (x => x.Periodi),g.Key.ADD_DATE,g.Key.Comment,g.Key.Company,g.Key.Company_ID,g.Key.CONTINUE_DATE,g.Key.STATE,g.Key.STOP_DATE,g.Key.Tankha})
.Dump();