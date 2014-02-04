<Query Kind="Program">
  <Connection>
    <ID>393fc2a1-3d2f-4fc3-8b9d-c916c0bb1c52</ID>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA2LGCigg0bUqfvV+5fr69FwAAAAACAAAAAAAQZgAAAAEAACAAAACcweYNfVdHbduk84GFtEdAPuSkqyq1f325WOB4ML95NAAAAAAOgAAAAAIAACAAAAD36Sg/oROiTUg/42mIUi6NiAfDI74nSKxIv07xmKcvtBAAAAB2muXkj5NigwmlBpZY/1pIQAAAAFksUka7cbQZQT6Q1yIE0ma77Yk31wW4wRSQ4l3uFjF52bpNqR0IC7RQ8f9J/ibc6PgXSfcWBPmK6ilXFruEoz8=</Password>
    <Database>SocialuriDazgveva</Database>
    <ShowServer>true</ShowServer>
    <Persist>true</Persist>
  </Connection>
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>EPPlus</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.ConditionalFormatting</Namespace>
  <Namespace>OfficeOpenXml.ConditionalFormatting.Contracts</Namespace>
  <Namespace>OfficeOpenXml.DataValidation</Namespace>
  <Namespace>OfficeOpenXml.DataValidation.Contracts</Namespace>
  <Namespace>OfficeOpenXml.DataValidation.Formulas.Contracts</Namespace>
  <Namespace>OfficeOpenXml.Drawing</Namespace>
  <Namespace>OfficeOpenXml.Drawing.Chart</Namespace>
  <Namespace>OfficeOpenXml.Drawing.Vml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>OfficeOpenXml.Style.Dxf</Namespace>
  <Namespace>OfficeOpenXml.Style.XmlAccess</Namespace>
  <Namespace>OfficeOpenXml.Table</Namespace>
  <Namespace>OfficeOpenXml.Table.PivotTable</Namespace>
  <Namespace>OfficeOpenXml.Utils</Namespace>
  <Namespace>OfficeOpenXml.VBA</Namespace>
</Query>

string gaascorePeriodi(string periodi){
	periodi =  periodi.Replace("ივლისი","201307")
						   .Replace("აგვისტო","201308")
						   .Replace("სექტემბერი","201309");
	if(periodi.Length > 20)
		return periodi.Substring(0,7).Replace(".","");
	return periodi;
}
string gaascoreDasakheleba(string dasakheleba){
	if(dasakheleba.Contains("14"))
		return "№ 14";
	return dasakheleba
	              .Replace("პალიატიური მზრუნველობის საქართველოს ეროვნული აკადემია_პრაქტიკული, საგანმანათლებლო და სამეცნიერო რესურს ცენტრი","აკადემია")
				  .Replace("შპს \"სრული მედ სერვისი\"", "მედ სერვისი")
				  .Replace("შპს ნეომედი", "ნეომედი");
}
void Main()
{
	using(var conn = new System.Data.OleDb.OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""C:\\Users\\Arakela\\Documents\\Database2.accdb"""))
	{
		conn.Open();

		var regulirebisSia = (
			from c in conn.Query("select * from regulirebisSia")
			select new {
				Dacesebuleba = gaascoreDasakheleba(c.Dacesebuleba),
				DanartisNomeri = (string)c.DanartisNomeri,
				PID = (string)c.PID,
			}
		);
		var sruliBaza = (
			from c in conn.Query("select * from sruliBaza")
			select new {
					PID = (string)c.PID,
					Dacesebuleba = gaascoreDasakheleba(c.Dacesebuleba),
					Periodi = int.Parse(gaascorePeriodi(c.Periodi)),
					Kodi = (string)c.Kodi,
					Pacienti = (string)c.Pacienti,
					DabTarigi = (string)c.DabTarigi,
					Tankha = (double)c.Tankha,
					Vizitebi = (int)c.Vizitebi
				}
			
		);
		var regCnobari = regulirebisSia.GroupBy (x => new {x.PID,x.Dacesebuleba})
			.OrderByDescending (g => g.Count ())
			.Select (g => new {g.Key.PID,g.Key.Dacesebuleba,Danartebi=string.Join("; ",g.Select (x => x.DanartisNomeri))})
			.ToList()
			;
			//let reg = regCnobari.FirstOrDefault (c => c.PID == g.Key.PID && c.Dacesebuleba == g.Key.Dacesebuleba)
		(
			from p in sruliBaza
			group p by p.PID into gPid
			orderby gPid.Count () descending
			let fpid = gPid.First ()
			let Dacesebulebebi=(
						from d in gPid.ToList()
						group d by d.Dacesebuleba into gDac
						let reg = regCnobari.FirstOrDefault (c => c.PID == gPid.Key && c.Dacesebuleba == gDac.Key)
						select new {
							Dacesebuleba = gDac.Key,
							Danartebi = reg == null ? "N/A" : reg.Danartebi,
							Periodebi = gDac.Select (da => new {da.Periodi,da.Tankha,da.Vizitebi})
						}
					)
			select new {
				PID = gPid.Key,
				fpid.Pacienti,
				fpid.DabTarigi,
				SulPeriodebiRaodenoba = gPid.Count (),
				SulPeriodebi = string.Join("; ", Dacesebulebebi.Select (x => string.Format("{0} - {1} ({2})",x.Dacesebuleba,x.Periodebi.Count (),x.Danartebi))),
				Dacesebulebebi,
				}
		)
		.SelectMany (p => p.Dacesebulebebi.SelectMany (d => d.Periodebi.Select (pr => new {
			PID= "'"+p.PID,
			p.Pacienti,
			p.DabTarigi,
			p.SulPeriodebi,
			p.SulPeriodebiRaodenoba,
			d.Dacesebuleba,
			d.Danartebi,
			pr.Periodi,
			pr.Tankha,
			pr.Vizitebi,
		
		})))
		
		.Dump();
		
	}
}