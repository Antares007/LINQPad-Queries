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
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Namespace>System.Globalization</Namespace>
</Query>

(
from p in Polisebi
group p by new {ShekmnisTarigi=p.ShekmnisTarigi,p.PolisisStatusi} into g 
select new {g.Key.ShekmnisTarigi,g.Key.PolisisStatusi,Raodenoba=g.Count ()}

).OrderBy (x => x.ShekmnisTarigi)
.ToList()
.GroupBy (x => x.ShekmnisTarigi.ToString("MMMM",new CultureInfo("ka")))
.Select (g => new {
		g.Key,Polisebi=g.Select (x => new {
					Dge=x.ShekmnisTarigi.ToString("dd",new CultureInfo("ka")),
					x.PolisisStatusi,
					x.Raodenoba
					})
	})