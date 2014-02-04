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
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
	
	
	ToPassword("").Dump();
	
	GetPasswordFor("").Dump();
	//notice scream sleep oppose cobol buzzes evaporator ong cheerleaders exacted needling donnie corporate donors disposes dry conventional
}

public static string GetPasswordFor(string text){
	return CalculateSHA1(text,Encoding.UTF8).Substring(0,11);
}

public static string ToPassword(string text)
{
	var symbols=Enumerable.Range((int)'A', 26)
						  .Concat(Enumerable.Range((int)'a', 26))
						  .Concat(Enumerable.Range((int)'0', 10))
						  .Select (i => (char)i)
						  .ToArray();
	
    byte[] buffer = Encoding.UTF8.GetBytes(text);
    SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
	return string.Join("",
	cryptoTransformSHA1.ComputeHash(buffer)
		.Select (x => (int)x)
		.Select (x => symbols[x % 62]))
		;
}
public static string CalculateSHA1(string text, Encoding enc)
{
    byte[] buffer = enc.GetBytes(text);
    SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
    return BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer).Dump()).Replace("-", "");
}