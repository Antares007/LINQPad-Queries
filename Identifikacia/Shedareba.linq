<Query Kind="Program">
  <Connection>
    <ID>8c49975d-7243-4e66-bf5a-92b155c73773</ID>
    <Persist>true</Persist>
    <Server>MegaMozg</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA3XCANzX8A0+HtkxmZtXFGAAAAAACAAAAAAAQZgAAAAEAACAAAACwZJTj6RI6G9b/trIDVOasBfrbl2/EeQ2symplXOxXjgAAAAAOgAAAAAIAACAAAACXcRvebWOVOdKm5bohQ6HFL7D5ZCdnJk1K+ulOvqgM9RAAAADzUWAKzvQZ+zIgoC2VCkD9QAAAAEjICu0vvEQYXF3gevv5l9vF9oZmUg9AyecZZw5eTwc1lDjBuJgZDUtRsZwzbxxaX1vttFBABp76hg+1Cy00Dq0=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Temp\EPPlus.dll</Reference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>OfficeOpenXml.Style</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	//typeof(Source_Data).GetFields().Where (x => x.MemberType==MemberTypes.Field).Select (x => string.Format("this.{1}=sd.{1};",x.FieldType,x.Name)).Dump();
	var dasadarebeli = Source_Data.Where(sd => sd.Base_Type==0)
					.Where (sd => sd.Unnom == null);
	var shedarebadi  = Source_Data.Where(sd => sd.Unnom != null);
	var i = 0;
	dasadarebeli
			.SelectMany(d => shedarebadi
				.Where (s =>  s.First_Name.Substring(0,3) == d.First_Name.Substring(0,3) && 
						      s.Last_Name.Substring(0,4)  == d.Last_Name.Substring(0,4) && 
							  (s.Birth_Date ==  d.Birth_Date || s.Birth_Date == null) && 
							  (s.J_ID == null || d.J_ID == null))
					.OrderBy (s => s.Base_Type)		  
					.Select (s => new 
								{
									d=new Canac(d), 
									s = new Canac(s),
									O1 = new Canac(s.FromSource_Data_Kavshiris.Where (fsdk => fsdk.To!=d.ID).Select (fsdk => fsdk.Source_Data).OrderBy (x => x.ID).FirstOrDefault () ),
									O2 = new Canac(s.FromSource_Data_Kavshiris.Where (fsdk => fsdk.To!=d.ID).Select (fsdk => fsdk.Source_Data).OrderBy (x => x.ID).Skip(1).FirstOrDefault () ),
									O3 = new Canac(s.FromSource_Data_Kavshiris.Where (fsdk => fsdk.To!=d.ID).Select (fsdk => fsdk.Source_Data).OrderBy (x => x.ID).Skip(2).FirstOrDefault () ),
									
								}))
			.AsEnumerable()
			.SelectMany (x => new[]{new{N=++i,SD=x.d},new {N=i,SD=x.s},new{N=i,SD=x.O1},new{N=i,SD=x.O2},new{N=i,SD=x.O3}})
			.Where(x=>x.SD.ID!=0)
			.GroupBy (x => x.N)
			.Select (g => new {g.Key,Sd=g.Select (x => x.SD).ToList()})
			.Dump();
}


public class Canac
{
public Canac(Source_Data sd){
if(sd==null) return;
this.UnnomisKhariskhi=sd.UnnomisKhariskhi!=null?sd.UnnomisKhariskhi.UnomisKhariskhi:1;
this.ID=sd.ID;
this.Pirvelckaro=sd.Pirvelckaro;
this.Source_Rec_Id=sd.Source_Rec_Id;
this.Periodi=sd.Periodi;
this.FID=sd.FID;
this.PID=sd.PID;
this.First_Name=sd.First_Name;
this.Last_Name=sd.Last_Name;
this.Birth_Date=sd.Birth_Date;
this.Region_Name=sd.Region_Name;
this.Rai_Name=sd.Rai_Name;
this.City=sd.City;
this.Village=sd.Village;
this.Street=sd.Street;
this.Full_Address=sd.Full_Address;
this.Dacesebuleba=sd.Dacesebuleba;
this.Dac_Region_Name=sd.Dac_Region_Name;
this.Dac_Rai_Name=sd.Dac_Rai_Name;
this.Dac_City=sd.Dac_City;
this.Dac_Village=sd.Dac_Village;
this.Dac_Full_Address=sd.Dac_Full_Address;
this.Region_ID=sd.Region_ID;
this.Rai=sd.Rai;
this.Unnom=sd.Unnom;
this.Xarvezi=sd.Xarvezi;
this.J_ID=sd.J_ID;
this.Piroba=sd.Piroba;
}
public System.Int32 ID;
public System.String Pirvelckaro;
public System.Int32 Source_Rec_Id;
public System.String Rai;
public System.Int32? J_ID;
public System.Int32? Piroba;

public System.Int32 UnnomisKhariskhi;
public System.Int32? Unnom;
public System.String Xarvezi;
public System.DateTime Periodi;
public System.String FID;
public System.String PID;
public System.String First_Name;
public System.String Last_Name;
public System.DateTime? Birth_Date;
public System.Int32? Sex;
public System.String Region_Name;
public System.String Rai_Name;
public System.String City;
public System.String Village;
public System.String Street;
public System.String Full_Address;
public System.String Dacesebuleba;
public System.String Dac_Region_Name;
public System.String Dac_Rai_Name;
public System.String Dac_City;
public System.String Dac_Village;
public System.String Dac_Full_Address;
public System.String Region_ID;

}
// Define other methods and classes here
