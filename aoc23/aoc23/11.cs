using System.Diagnostics;

public class Day11
{
    public struct Galaxy
    {
        public Galaxy(int x = -1, int y = -1)
        {
            X = x;
            Y = y;
        }
        public int X { get; }
        public int Y { get; }
        public readonly bool Equals(Galaxy other) => X == other.X && Y == other.Y;
        public readonly bool IsEmpty => X == -1 || Y == -1;
        public override readonly string ToString() => $"[{X},{Y}]";
    }

    public struct GalaxyPair
    {
        public GalaxyPair(Galaxy a, Galaxy b)
        {
            A = a;
            B = b;
        }

        public Galaxy A { get; }
        public Galaxy B { get; }
        public override readonly string ToString() => $"A: {A} - B: {B}";

        public override bool Equals(object obj)
        {                      
            GalaxyPair other = (GalaxyPair) obj;
            return A.Equals(other.A) && B.Equals(other.B) || A.Equals(other.B) && B.Equals(other.A);
        }
        public override readonly int GetHashCode() => A.X + A.Y + B.X + B.Y;
    }

    public static void Execute()
    {
        Stopwatch sw = Stopwatch.StartNew();
        string[] lines = File.ReadAllText("11.txt").Split("\r\n");
        char[,] universe = Get2DArray(lines);

        List<List<char>> expandedUniverse = new List<List<char>>();
        for (int y = 0; y < universe.GetLength(0); y++)
        {
            expandedUniverse.Add(new List<char>());
            for (int x = 0; x < universe.GetLength(1); x++)
            {
                expandedUniverse[y].Add(universe[y, x]);
            }
        }

        // Utils.Print2DList(expandedUniverse);

        List<int> emptyRows = GetEmptyRows(universe);
        List<int> emptyColumns = GetEmptyColumns(universe);

        List<Galaxy> galaxies = GetGalaxies(expandedUniverse);
        List<GalaxyPair> galaxyPairs = GetGalaxyPairs(expandedUniverse, galaxies);

        Console.WriteLine("Number of pairs: " + galaxyPairs.Count);

        double totalDistance = 0;
        int multiplier = 1000000;
        foreach (var pair in galaxyPairs)
        {
            double xDistance = Math.Abs(pair.A.X - pair.B.X);
            double yDistance = Math.Abs(pair.A.Y - pair.B.Y);

            foreach (var emptyColumn in emptyColumns)
            {
                if(emptyColumn <= pair.A.X && pair.B.X <= emptyColumn || emptyColumn <= pair.B.X && pair.A.X <= emptyColumn)
                    xDistance += multiplier - 1;
            }
            foreach (var emptyRow in emptyRows)
            {
                if(emptyRow <= pair.A.Y && pair.B.Y <= emptyRow || emptyRow <= pair.B.Y && pair.A.Y <= emptyRow)
                    yDistance += multiplier - 1;
            }
            totalDistance += xDistance + yDistance;
            // Console.WriteLine($"{galaxies.IndexOf(pair.A) + 1}-{galaxies.IndexOf(pair.B) + 1} - {pair} X:{xDistance} Y:{yDistance} total:{xDistance + yDistance}");
        }

        // Part 1 Correct Answer = 9312968
        // Part 2 Correct Answer = 597714117556
        Console.WriteLine(totalDistance);

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    private static List<GalaxyPair> GetGalaxyPairs(List<List<char>> expandedUniverse, List<Galaxy> allGalaxies)
    {
        var combinations = from item1 in allGalaxies
                           from item2 in allGalaxies
                           where !item1.Equals(item2)
                           select new GalaxyPair(item1, item2);

        return combinations.ToList().Distinct().ToList();
    }

    private static List<Galaxy> GetGalaxies(List<List<char>> expandedUniverse)
    {
        List<Galaxy> allGalaxies = new List<Galaxy>();

        for (int y = 0; y < expandedUniverse.Count; y++)
        {
            for (int x = 0; x < expandedUniverse[y].Count; x++)
            {
                if (expandedUniverse[y][x] == '#')
                {
                    allGalaxies.Add(new Galaxy(x, y));
                }
            }
        }
        return allGalaxies;
    }

    public static char[,] Get2DArray(string[] lines)
    {
        char[,] grid = new char[lines.Length, lines[0].Length];

        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                grid[y,x] = lines[y][x];
            }
        }
        return grid;
    }

    private static List<int> GetEmptyColumns(char[,] universe)
    {
        List<int> columns = new List<int>();
        for (int x = 0; x < universe.GetLength(1); x++)
        {
            for (int y = 0; y < universe.GetLength(0); y++)
            {
                if (universe[y, x] == '#') break;

                if (y == universe.GetLength(0) - 1)
                {
                    columns.Add(x);
                }
            }
        }
        return columns;
    }

    private static List<int> GetEmptyRows(char[,] universe)
    {
        List<int> rows = new List<int>();
        for (int y = 0; y < universe.GetLength(0); y++)
        {
            for (int x = 0; x < universe.GetLength(1); x++)
            {
                if (universe[y, x] == '#') break;

                if (x == universe.GetLength(1) - 1)
                {
                    rows.Add(y);
                }
            }
        }
        return rows;
    }
}