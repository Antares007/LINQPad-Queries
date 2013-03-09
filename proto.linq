<Query Kind="Program">
  <Connection>
    <ID>b80047fa-bbc6-4c50-97ff-0a369e02fa91</ID>
    <Persist>true</Persist>
    <Server>Triton</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIAg+Bd0cFE2ekrmjntd3ggAAAAACAAAAAAAQZgAAAAEAACAAAAAD89x4SL38S/4r7NUU2iHNNTcmnVTi1xQPx1AC1vAtFAAAAAAOgAAAAAIAACAAAABgO+xVBio0IKzceIXWbWfgFv0jcxQpOA9YilhDtPA8XxAAAABJcM5+MLInsGd5jUUGfXtnQAAAALHy7sVof5cKLhfpSHxbLdPESALwKOWLElOgeYcZmaeqO1sDG9SHnPN5xOzSlBHlSD7agk+KC9dH/4XMZn4gYvM=</Password>
    <Database>Pirvelckaroebi</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <GACReference>System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</GACReference>
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>protobuf-net</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>ProtoBuf</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Runtime.Serialization</Namespace>
</Query>

[DataContract]
public class Umceo
{
    [DataMember(Order=1)]  public int RecId {get; set;} 
    [DataMember(Order=2)]  public DateTime RecDate {get; set;} 
    [DataMember(Order=3)]  public string FID {get; set;} 
    [DataMember(Order=4)]  public int FAMILY_SCORE {get; set;} 
    [DataMember(Order=5)]  public string ZIP_CODE {get; set;} 
    [DataMember(Order=6)]  public string CITY {get; set;} 
    [DataMember(Order=7)]  public string GOVERNMENT {get; set;} 
    [DataMember(Order=8)]  public string VILLAGE {get; set;} 
    [DataMember(Order=9)]  public string FULL_ADDRESS {get; set;} 
    [DataMember(Order=10)] public string PID {get; set;} 
    [DataMember(Order=11)] public DateTime? BIRTH_DATE {get; set;} 
    [DataMember(Order=12)] public string FIRST_NAME {get; set;} 
    [DataMember(Order=13)] public string LAST_NAME {get; set;} 
    [DataMember(Order=14)] public DateTime SCORE_DATE {get; set;} 
    [DataMember(Order=15)] public DateTime VISIT_DATE {get; set;} 
    [DataMember(Order=16)] public string RESORE_DOC_NO {get; set;} 
    [DataMember(Order=17)] public DateTime? RESTORE_DOC_DATE {get; set;} 
    [DataMember(Order=18)] public int? N {get; set;} 
    [DataMember(Order=19)] public string RAI_ID {get; set;} 
    [DataMember(Order=20)] public string COMMENT {get; set;} 
    [DataMember(Order=21)] public string RAI_NAME {get; set;} 
}

void Main()
{
    this.Connection.Open();
    
    var cmd = new SqlCommand(InsuranceW, (SqlConnection)this.Connection);
    var reader = cmd.ExecuteReader();
    File.Delete("d:\\temp\\InsuranceW.protobuf"); 
    var ms = new FileStream("d:\\temp\\InsuranceW.protobuf", FileMode.CreateNew);
    while(reader.Read())
    {
        var se = new SimpleExtensible();
        for (var i = 0; i < reader.FieldCount ; i++)
        {
            var v = reader[i];
            if(v != null)
                Extensible.AppendValue(se, i+1, v);
        }
        ProtoBuf.Serializer.SerializeWithLengthPrefix(ms, se, PrefixStyle.Base128);
    }
    ms.Flush();
    ms.Close();
    ms.Dispose();
    
    return;
    

return;
var xs=new List<Umceo>(8000000);
    var fs = new FileStream("d:\\temp\\data.protobuf", FileMode.Open, FileAccess.Read, FileShare.Read);
    var j = 0;
    while (true)
    {
        var o = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Umceo>(fs, PrefixStyle.Base128);
        if (o==null) break;
        j++;
        xs.Add(o);
    }
    fs.Dispose();
    j.Dump();
    xs.Count.Dump();
    return;    



 
}

[DataContract]
public class A
{
    [DataMember(Order = 1)] public string Name { get;  set; }
    [DataMember(Order = 2)] public string LastName { get;  set; }
}

[ProtoContract]
public class SimpleExtensible : Extensible
{
}

string InsuranceW = @"select
     201108 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201108_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201108_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201108_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201108 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201108 d
Union all
select
     201109 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201109_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201109_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201109_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201109 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201109 d
Union all
select
     201110 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201110_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201110_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201110_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201110 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201110 d
Union all
select
     201111 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201111_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201111_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201111_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201111 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201111 d
Union all
select
     201112 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201112_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201112_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201112_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201112 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201112 d
Union all
select
     201201 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201201_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201201_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201201_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201201 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201201 d
Union all
select
     201202 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201202_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201202_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201202_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201202 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201202 d
Union all
select
     201203 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201203_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201203_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201203_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201203 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201203 d
Union all
select
     201204 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201204_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201204_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201204_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201204 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201204 d
Union all
select
     201205 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201205_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201205_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201205_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201205 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201205 d
Union all
select
     201206 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201206_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201206_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201206_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201206 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201206 d
Union all
select
     201207 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201207_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201207_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201207_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201207 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201207 d
Union all
select
     201208 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201208_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201208_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201208_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201208 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201208 d
Union all
select
     201209 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201209_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201209_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201209_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201209 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201209 d
Union all
select
     201210 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201210_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201210_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201210_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201210 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201210 d
Union all
select
     201211 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201211_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201211_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201211_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201211 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201211 d
Union all
select
     201212 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201212_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201212_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201212_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201212 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201212 d
Union all
select
     201301 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201301_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201301_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201301_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201301 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201301 d
Union all
select
     201302 Periodi
    ,d.AkhaliUnnomi, d.Unnom, d.Base_type, d.RECORD_ADD_DATE
    ,d.[dagv-tar], d.End_Date
    ,d.ADD_DATE, d.ADD_DATE_201302_TMP ADD_DATE_Shemdegi
    ,d.CONTINUE_DATE, d.CONTINUE_DATE_201302_TMP CONTINUE_DATE_Shemdegi
    ,d.STOP_DATE, d.STOP_DATE_201302_TMP STOP_DATE_Shemdegi
    ,d.STATE, d.STATE_201302 STATE_Shemdegi
from INSURANCEW..DAZGVEVA_201302 d
";