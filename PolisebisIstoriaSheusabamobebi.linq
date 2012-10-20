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
</Query>

(from p in PolisisChabarebisIstoria
group p by p.PolisisNomeri into g
select new { PolisisNomeri=g.Key, Istoria=g.OrderBy (x => x.Chambarebeli=="Fosta" ? x.VizitisTarigi : x.VizitisTarigi.AddYears(100)).ToList() })
.Where (x => x.Istoria.Count () > 1)
.AsEnumerable()
.Where (x => x.Istoria.Any (i => i.Statusi == "Chabarda"))
.Where(x=>x.Istoria.Last ().Statusi != "Chabarda")