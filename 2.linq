<Query Kind="Program">
  <Connection>
    <ID>c574c557-3d94-420d-a692-04c4548d4174</ID>
    <Persist>true</Persist>
    <Driver>EntityFramework</Driver>
    <Server>triton</Server>
    <CustomAssemblyPath>C:\Temp\Insurancew\Insurancew\bin\Debug\Insurancew.dll</CustomAssemblyPath>
    <CustomTypeName>Insurancew.INSURANCEWEntities</CustomTypeName>
    <CustomMetadataPath>res://Insurancew/InsuraneWEntities.csdl|res://Insurancew/InsuraneWEntities.ssdl|res://Insurancew/InsuraneWEntities.msl</CustomMetadataPath>
    <SqlSecurity>true</SqlSecurity>
    <Database>INSURANCEW</Database>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA3XCANzX8A0+HtkxmZtXFGAAAAAACAAAAAAAQZgAAAAEAACAAAADopIjgwxoYgOO9NMoHuEPShag3DgqMloBpw3lQAVuSdgAAAAAOgAAAAAIAACAAAABlisvsvVqL2bUAso+p3V8qrHU7/jnYypaoAlnxU2AHixAAAAB7zvfR8STElf63owBgeiaxQAAAAOadFAO95Hks9jwTUeTuVpTezBrDWBKvG1nEcVUs71QZKUX42g3B6Pw9ptkscFwoqoTik4MxGqWe1Raja5Kwf3s=</Password>
  </Connection>
</Query>

void Main()
{
Fdata("050902700233")
	.Dump();

return ;
	var q = from d in DAZGVEVA_201112.Where(x => x.Unnom == 24 || x.Unnom == 23).AsEnumerable()
	let t=d.dagv_tar
	select new
	{
		Cxrili=201111,
		d.Unnom,
		d.FID,
		d.dagv_tar,
		d.End_Date,
		
		Kontrektebi = (new dynamic[]{
			//new { Periodi=int.Parse(d.dagv_tar.Value.ToString("yyyyMM")), State=-999, ADD_DATE=(DateTime?)null,       CONTINUE_DATE=(DateTime?)null,			STOP_DATE=(DateTime?)null},
			new { Periodi=201006, State=d.STATE_06, ADD_DATE=d.ADD_DATE_06,       CONTINUE_DATE=d.CONTINUE_DATE_06,			STOP_DATE=d.STOP_DATE_06},
			new { Periodi=201007, State=d.STATE_07, ADD_DATE=d.ADD_DATE_07, 		CONTINUE_DATE=d.CONTINUE_DATE_07,			STOP_DATE=d.STOP_DATE_07},
			new { Periodi=201008, State=d.STATE_08, ADD_DATE=d.ADD_DATE_08, 		CONTINUE_DATE=d.CONTINUE_DATE_08,			STOP_DATE=d.STOP_DATE_08},
			new { Periodi=201009, State=d.STATE_09, ADD_DATE=d.ADD_DATE_09, 		CONTINUE_DATE=d.CONTINUE_DATE_09,			STOP_DATE=d.STOP_DATE_09},
			new { Periodi=201010, State=d.STATE_10, ADD_DATE=d.ADD_DATE_10, 		CONTINUE_DATE=d.CONTINUE_DATE_10,			STOP_DATE=d.STOP_DATE_10},
			new { Periodi=201011, State=d.STATE_11, ADD_DATE=d.ADD_DATE_11, 		CONTINUE_DATE=d.CONTINUE_DATE_11,			STOP_DATE=d.STOP_DATE_11},
			new { Periodi=201012, State=d.STATE_12, ADD_DATE=d.ADD_DATE_12, 		CONTINUE_DATE=d.CONTINUE_DATE_12,			STOP_DATE=d.STOP_DATE_12},
			new { Periodi=201101, State=d.STATE_201101, ADD_DATE=d.ADD_DATE_201101, 	CONTINUE_DATE=d.CONTINUE_DATE_201101,		STOP_DATE=d.STOP_DATE_201101},
			new { Periodi=201102, State=d.STATE_201102, ADD_DATE=d.ADD_DATE_201102, 	CONTINUE_DATE=d.CONTINUE_DATE_201102,		STOP_DATE=d.STOP_DATE_201102},
			new { Periodi=201103, State=d.STATE_201103, ADD_DATE=d.ADD_DATE_201103, 	CONTINUE_DATE=d.CONTINUE_DATE_201103,		STOP_DATE=d.STOP_DATE_201103},
			new { Periodi=201104, State=d.STATE_201104, ADD_DATE=d.ADD_DATE_201104, 	CONTINUE_DATE=d.CONTINUE_DATE_201104,		STOP_DATE=d.STOP_DATE_201104},
			new { Periodi=201105, State=d.STATE_201105, ADD_DATE=d.ADD_DATE_201105, 	CONTINUE_DATE=d.CONTINUE_DATE_201105,		STOP_DATE=d.STOP_DATE_201105},
			new { Periodi=201106, State=d.STATE_201106, ADD_DATE=d.ADD_DATE_201106, 	CONTINUE_DATE=d.CONTINUE_DATE_201106,		STOP_DATE=d.STOP_DATE_201106},
			new { Periodi=201107, State=d.STATE_201107, ADD_DATE=d.ADD_DATE_201107, 	CONTINUE_DATE=d.CONTINUE_DATE_201107,		STOP_DATE=d.STOP_DATE_201107},
			new { Periodi=201108, State=d.STATE_201108, ADD_DATE=d.ADD_DATE_201108, 	CONTINUE_DATE=d.CONTINUE_DATE_201108,		STOP_DATE=d.STOP_DATE_201108},
			new { Periodi=201109, State=d.STATE_201109, ADD_DATE=d.ADD_DATE_201109, 	CONTINUE_DATE=d.CONTINUE_DATE_201109,		STOP_DATE=d.STOP_DATE_201109},
			new { Periodi=201110, State=d.STATE_201110, ADD_DATE=d.ADD_DATE_201110,  CONTINUE_DATE=d.CONTINUE_DATE,				STOP_DATE=d.STOP_DATE},
			new { Periodi=201111, State=d.STATE,        ADD_DATE=d.ADD_DATE, CONTINUE_DATE=d.CONTINUE_DATE,	STOP_DATE=d.STOP_DATE},
			new { Periodi=201112, State=d.STATE_201112,  ADD_DATE=d.ADD_DATE_201112_TMP, CONTINUE_DATE=d.CONTINUE_DATE_201112_TMP,	STOP_DATE=d.STOP_DATE_201112_TMP},
		}).ToList()
//		.GroupBy(g => new {g.State,g.CONTINUE_DATE})
//		.Select(g => new {g.Key.State,g.Key.CONTINUE_DATE,Dan= g.Min(z=>z.Periodi),Mde=g.Max(z=>z.Periodi)})
		//.Where(x=>x.State!=null || x.CONTINUE_DATE!=null)
	};
	q.ToList();
	q.Dump();
}

public IDictionary<int,IEnumerable<dynamic>> Fdata(string fid)
{
	var d01= (from f in FAMILY_DATA_201101 
					where f.FID==fid
					select new {Periodi=201101, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});
	var d02= (from f in FAMILY_DATA_201102
					where f.FID==fid
					select new {Periodi=201102, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});
	var d03= (from f in FAMILY_DATA_201103
					where f.FID==fid
					select new {Periodi=201103, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});				
	var d04= (from f in FAMILY_DATA_201104
					where f.FID==fid
					select new {Periodi=201104, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});		
	var d05= (from f in FAMILY_DATA_201105
					where f.FID==fid
					select new {Periodi=201105, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});		
	var d06= (from f in FAMILY_DATA_201106
					where f.FID==fid
					select new {Periodi=201106, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});		
	var d07= (from f in FAMILY_DATA_201107
					where f.FID==fid
					select new {Periodi=201107, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});		
	var d08= (from f in FAMILY_DATA_201108
					where f.FID==fid
					select new {Periodi=201108, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});		
	var d09= (from f in FAMILY_DATA_201109
					where f.FID==fid
					select new {Periodi=201109, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});		
	var d10= (from f in FAMILY_DATA_201110
					where f.FID==fid
					select new {Periodi=201110, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});		
	var d11= (from f in FAMILY_DATA_201111
					where f.FID==fid
					select new {Periodi=201111, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});		
	var d12= (from f in FAMILY_DATA_201112
					where f.FID==fid
					select new {Periodi=201112, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});	
	var d1201= (from f in FAMILY_DATA_201201
					where f.FID==fid
					select new {Periodi=201201, f.Unnom,f.PID,f.FID,f.FIRST_NAME,f.LAST_NAME,f.BIRTH_DATE,Dadareba=f.J_ID > 0 ?"DadardaReestrs":"ArDadardaReestrs"});	
	return d01.Concat(d02).Concat(d03).Concat(d04).Concat(d05).Concat(d06).Concat(d07).Concat(d08).Concat(d09).Concat(d10).Concat(d11).Concat(d12).Concat(d1201)
			.GroupBy (d => d.Periodi)
			.Select (d => new 
			{
				Periodi = d.Key, 
				Cevrebi = d.Select (x => new {x.FID,x.PID,x.Unnom,x.FIRST_NAME,x.LAST_NAME,x.BIRTH_DATE,x.Dadareba})
			})
			.OrderBy (d => d.Periodi).ToDictionary (d => d.Periodi,k=>(IEnumerable<dynamic>)k.Cevrebi);		
}
