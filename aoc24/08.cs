using System.Diagnostics.Tracing;

public class Day8
{    
    public struct CoordPair(Coord a, Coord b)
    {
        public Coord A { get; set; } = a;
        public Coord B { get; set; } = b;
        public override readonly string ToString()
        {
            return $"{A} - {B}";
        }
        public CoordPair GetInverse()
        {
            return new CoordPair(B, A);
        }
    }
    public struct Coord(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;

        public override readonly string ToString()
        {
            return $"{X}, {Y}";
        }

        public override readonly bool Equals(object? obj)
        {
            if (obj is Coord other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
    public static void Execute()
    {
        string[] input = File.ReadAllText("08.txt").Split('\n');

        int xLength = input[0].Trim().Length;
        int yLength = input.Length;

        int x = 0, y = 0;
        char[,] grid = new char[xLength, yLength];
        foreach (var row in input)
        {
            y = 0;
            foreach (var col in row.Trim())
            {
                grid[x, y] = col;
                y++;
            }
            x++;
        }

        // Part 1 = 357
        // Part1(grid);

        // Part 2 = 1266
        Part2(grid);
    }

    private static void Part1(char[,] input)
    {
        List<CoordPair> coordPairs = GetCoordinatePairs(input);
        HashSet<Coord> antinodes = GetAntinodeCoordinates(input, coordPairs);

        foreach(var node in antinodes)
        {
            Console.WriteLine(node);
        }

        PrintAntinodes(input, antinodes);

        Console.WriteLine("Antinodes: " + antinodes.Count);
    }

    private static HashSet<Coord> GetAntinodeCoordinates(char[,] input, List<CoordPair> coordPairs)
    {
        HashSet<Coord> antinodes = [];
        foreach (var coordPair in coordPairs)
        {
            int xDiff = Math.Abs(coordPair.A.X - coordPair.B.X);
            int yDiff = Math.Abs(coordPair.A.Y - coordPair.B.Y);

            int antinodeAX = coordPair.A.X >= coordPair.B.X ? coordPair.A.X + xDiff : coordPair.A.X - xDiff;
            int antinodeAY = coordPair.A.Y >= coordPair.B.Y ? coordPair.A.Y + yDiff : coordPair.A.Y - yDiff;
            int antinodeBX = coordPair.B.X >= coordPair.A.X ? coordPair.B.X + xDiff : coordPair.B.X - xDiff;
            int antinodeBY = coordPair.B.Y >= coordPair.A.Y ? coordPair.B.Y + yDiff : coordPair.B.Y - yDiff;

            Coord antinodeA = new(antinodeAX, antinodeAY);
            Coord antinodeB = new(antinodeBX, antinodeBY);

            if (!antinodes.Contains(antinodeA)
                && antinodeA.X < input.GetLength(0)
                && antinodeA.Y < input.GetLength(1)
                && antinodeA.X >= 0
                && antinodeA.Y >= 0)
                antinodes.Add(antinodeA);

            if (!antinodes.Contains(antinodeB)
                && antinodeB.X < input.GetLength(0)
                && antinodeB.Y < input.GetLength(1)
                && antinodeB.X >= 0
                && antinodeB.Y >= 0)
                antinodes.Add(antinodeB);
        }

        return antinodes;
    }

    private static List<CoordPair> GetCoordinatePairs(char[,] input)
    {
        List<CoordPair> coordPairs = [];
        for (int y = 0; y < input.GetLength(1); y++)
        {
            for (int x = 0; x < input.GetLength(0); x++)
            {
                if (input[y, x] == '.') continue;

                CoordPair coordPair = GetCoordPair(x, y, input, coordPairs);
                if (coordPair.A.X == -1) continue;

                coordPairs.Add(coordPair);

                x--;
            }
        }

        return coordPairs;
    }

    private static void PrintAntinodes(char[,] input, HashSet<Coord> antinodes)
    {
        int total = 0;
        for (int y = 0; y < input.GetLength(1); y++)
        {
            for (int x = 0; x < input.GetLength(0); x++)
            {
                // if (antinodes.Contains(new Coord(x, y)) && input[y,x] == '.')
                if (antinodes.Contains(new Coord(x, y)) || input[y,x] != '.')
                {
                    total++;
                    Console.Write('#');
                }
                else
                {
                    Console.Write(input[y, x]);
                }

            }
            Console.WriteLine();
        }
        Console.WriteLine("Total: " + total);
    }

    private static CoordPair GetCoordPair(int xCoord, int yCoord, char[,] input, List<CoordPair> coordPairs)
    {
        for(int y = 0; y < input.GetLength(1); y++ )
        {
            for( int x = 0; x < input.GetLength(0); x++)
            {
                if (input[y, x] == '.' || input[y, x] != input[yCoord, xCoord]) continue;

                CoordPair coordPair = new(new Coord(x, y), new Coord(xCoord, yCoord));

                if (coordPairs.Contains(coordPair)
                    || coordPairs.Contains(coordPair.GetInverse())
                    || coordPair.A.Equals(coordPair.B))
                {
                    continue;
                }
                else
                {
                    return coordPair;
                }
            }

        }

        return new CoordPair(new Coord(-1,-1), new Coord(-1,-1));
    }

    private static void Part2(char[,] input)
    {
        List<CoordPair> coordPairs = GetCoordinatePairs(input);
        HashSet<Coord> antinodes = GetAntinodeCoordinates2(input, coordPairs);

        foreach(var node in antinodes)
        {
            Console.WriteLine(node);
        }

        PrintAntinodes(input, antinodes);

        Console.WriteLine("Antinodes: " + antinodes.Count);
    }

    private static HashSet<Coord> GetAntinodeCoordinates2(char[,] input, List<CoordPair> coordPairs)
    {
        HashSet<Coord> antinodes = [];
        foreach (var coordPair in coordPairs)
        {
            int xDiff = Math.Abs(coordPair.A.X - coordPair.B.X);
            int yDiff = Math.Abs(coordPair.A.Y - coordPair.B.Y);

            int antinodeAX = coordPair.A.X >= coordPair.B.X ? coordPair.A.X + xDiff : coordPair.A.X - xDiff;
            int antinodeAY = coordPair.A.Y >= coordPair.B.Y ? coordPair.A.Y + yDiff : coordPair.A.Y - yDiff;

            while(antinodeAX < input.GetLength(0) && antinodeAY < input.GetLength(1) 
                    && antinodeAX >= 0 && antinodeAY >= 0)
            {
                Coord antinodeA = new(antinodeAX, antinodeAY);

                if (!antinodes.Contains(antinodeA)
                    && antinodeA.X < input.GetLength(0)
                    && antinodeA.Y < input.GetLength(1)
                    && antinodeA.X >= 0
                    && antinodeA.Y >= 0)
                    antinodes.Add(antinodeA);
                
                antinodeAX = coordPair.A.X >= coordPair.B.X ? antinodeAX + xDiff : antinodeAX - xDiff;
                antinodeAY = coordPair.A.Y >= coordPair.B.Y ? antinodeAY + yDiff : antinodeAY - yDiff;
            }

            int antinodeBX = coordPair.B.X >= coordPair.A.X ? coordPair.B.X + xDiff : coordPair.B.X - xDiff;
            int antinodeBY = coordPair.B.Y >= coordPair.A.Y ? coordPair.B.Y + yDiff : coordPair.B.Y - yDiff;

            while(antinodeBX < input.GetLength(0) && antinodeBY < input.GetLength(1) 
                    && antinodeBX >= 0 && antinodeBY >= 0)
            {
                Coord antinodeB = new(antinodeBX, antinodeBY);


                if (!antinodes.Contains(antinodeB)
                    && antinodeB.X < input.GetLength(0)
                    && antinodeB.Y < input.GetLength(1)
                    && antinodeB.X >= 0
                    && antinodeB.Y >= 0)
                    antinodes.Add(antinodeB);
                
                antinodeBX = coordPair.B.X >= coordPair.A.X ? antinodeBX + xDiff : antinodeBX - xDiff;
                antinodeBY = coordPair.B.Y >= coordPair.A.Y ? antinodeBY + yDiff : antinodeBY - yDiff;
            }
        }

        return antinodes;
    }
}
