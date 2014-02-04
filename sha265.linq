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

string hashToString(byte [] hash)
{
	string hashString = string.Empty;
	foreach (byte x in hash)
	{
		hashString += String.Format("{0:x2}", x);
	}
	return hashString;
}
byte[] getHashSha256(byte[] bytes)
{
   using(var hashstring = new SHA256Managed()){
		return hashstring.ComputeHash(bytes);
	}
}
void Main()
{
	hashToString(
		getHashSha256(
			getHashSha256(
				Encoding.UTF8.GetBytes("hello")
			)
		)
	).Dump();
	
}

// Define other methods and classes here
