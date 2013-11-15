<Query Kind="Program" />

void Main()
{
	var partiebi = new []{
		new { Partia=1, Ref=1, Dasakheleba="1111" },
		new { Partia=2, Ref=1, Dasakheleba="1111" },
		new { Partia=3, Ref=1, Dasakheleba="2222" },
		new { Partia=4, Ref=2, Dasakheleba="3333" }
	};
	var refisMinichebebi = new []{
		new { Ref=1, Dasakheleba="1111", AkhaliRef=10, V=1},
		new { Ref=1, Dasakheleba="2222", AkhaliRef=11, V=2},
		new { Ref=2, Dasakheleba="3333", AkhaliRef=2,  V=3}
	};
	var maps=
	(
		from d in partiebi
	 	select new {
			d.Ref, 
			Partiebi=new []{ new {d.Partia,d.Dasakheleba}} }
	).Concat(
	 	from d in refisMinichebebi
	 	select new {
			d.Ref, 
			Partiebi=new []{ }, 
			MinichebuliRefebi=new []{ new { d.Dasakheleba, d.AkhaliRef, d.V}}
		}
	);(
		from r in maps
		group r by r.Ref into g
		select new {Ref = g.Key,Dasakhelebebi=g.SelectMany (x => x.Dasakhelebebi)
											   .GroupBy (x => x.Dasakheleba)
											   .SelectMany (g2 => g2.OrderByDescending (x => x.V).Take(1))
											   }
	).Dump();
}

/// <SUMMARY>Computes the Levenshtein Edit Distance between two enumerables.</SUMMARY>
/// <TYPEPARAM name="T">The type of the items in the enumerables.</TYPEPARAM>
/// <PARAM name="x">The first enumerable.</PARAM>
/// <PARAM name="y">The second enumerable.</PARAM>
/// <RETURNS>The edit distance.</RETURNS>
public static int EditDistance<T>(IEnumerable<T> x, IEnumerable<T> y) 
    where T : IEquatable<T>
{
    // Validate parameters
    if (x == null) throw new ArgumentNullException("x");
    if (y == null) throw new ArgumentNullException("y");

    // Convert the parameters into IList instances
    // in order to obtain indexing capabilities
    IList<T> first = x as IList<T> ?? new List<T>(x);
    IList<T> second = y as IList<T> ?? new List<T>(y);

    // Get the length of both.  If either is 0, return
    // the length of the other, since that number of insertions
    // would be required.
    int n = first.Count, m = second.Count;
    if (n == 0) return m;
    if (m == 0) return n;

    // Rather than maintain an entire matrix (which would require O(n*m) space),
    // just store the current row and the next row, each of which has a length m+1,
    // so just O(m) space. Initialize the current row.
    int curRow = 0, nextRow = 1;
    int[][] rows = new int[][] { new int[m + 1], new int[m + 1] };
    for (int j = 0; j <= m; ++j) rows[curRow][j] = j;

    // For each virtual row (since we only have physical storage for two)
    for (int i = 1; i <= n; ++i)
    {
        // Fill in the values in the row
        rows[nextRow][0] = i;
        for (int j = 1; j <= m; ++j)
        {
            int dist1 = rows[curRow][j] + 1;
            int dist2 = rows[nextRow][j - 1] + 1;
            int dist3 = rows[curRow][j - 1] +
                (first[i - 1].Equals(second[j - 1]) ? 0 : 1);

            rows[nextRow][j] = Math.Min(dist1, Math.Min(dist2, dist3));
        }

        // Swap the current and next rows
        if (curRow == 0)
        {
            curRow = 1;
            nextRow = 0;
        }
        else
        {
            curRow = 0;
            nextRow = 1;
        }
    }

    // Return the computed edit distance
    return rows[curRow][m];
}