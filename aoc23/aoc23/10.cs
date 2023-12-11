using System.Collections;
using System.Diagnostics;
using System.Numerics;

public class Day10
{
    public struct Pipe
    {
        public Pipe(char value, int x, int y)
        {
            Value = value;
            X = x;
            Y = y;
        }
        public char Value { get; }
        public int X {get;}
        public int Y {get;}

        public int[,] GetDirections()
        {
            switch(Value)
            {
                case '|':
                    return new int[,] { { 0, -1 }, { 0, 1 } };
                case '-':
                    return new int[,] { { -1, 0 }, { 1, 0 } };
                case 'L':
                    return new int[,] { { 0, -1 }, { 1, 0 } };
                case 'J':
                    return new int[,] { { 0, -1 }, { -1, 0 } };
                case '7':
                    return new int[,] { { 0, 1 }, { -1, 0 } };
                case 'F':
                    return new int[,] { { 0, 1 },  { 1, 0 } };
            }

            return new int[,] { { 0, 0 }, { 0, 0 }};
        }

        public bool Equals(Pipe other) => X == other.X && Y == other.Y;
        public override string ToString() => $"({Value} : [{X},{Y}])";
    }

    public static void Execute()
    {
        Stopwatch sw = Stopwatch.StartNew();
        string[] lines = File.ReadAllText("10.txt").Split("\r\n");
        char[,] grid = Get2DArray(lines, out Pipe begin);

        bool[,] clearLoop = new bool[grid.GetLength(0),grid.GetLength(1)];

        for (int y = 0; y < clearLoop.GetLength(0); y++)
        {
            for (int x = 0; x < clearLoop.GetLength(1); x++)
            {
                clearLoop[y,x] = false;
            }
        }

        int length = 0;

        Pipe previous = new Pipe();
        Pipe current = begin;

        while(current.Value != 'S')
        {
            Pipe nextNode = GetNextNode(grid, current, previous);
            previous = current;
            current = nextNode;

            clearLoop[current.Y, current.X] = true;
            length++;
        }

        // Correct answer = 7107
        Console.WriteLine("Length: " + (length+1)/2);

        int containedTiles = 0;
        
        for (int y = 0; y < clearLoop.GetLength(0); y++)
        {
            int timesCrossed = 0;
            for (int x = 0; x < clearLoop.GetLength(1); x++)
            {
                int lastValue = GetLastValueOnRow(clearLoop, y);
                // char printValue = clearLoop[y,x] ? '1' : '0';
                char printValue = grid[y,x];
                if(clearLoop[y,x]) 
                {
                    timesCrossed++;
                }

                if(timesCrossed % 2 != 0 && !clearLoop[y,x] && !OnEdge(clearLoop, x, y) && x < lastValue) 
                {
                    printValue = '*';
                    containedTiles++;
                }

                Console.Write(printValue);
            }
            Console.WriteLine();
        }

        // Incorrect Answers
        // 1142 - too high
        Console.WriteLine($"Contained Tiles: {containedTiles}");

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    private static int GetLastValueOnRow(bool[,] array, int y)
    {
        bool[] row = Enumerable.Range(0, array.GetLength(1))
                        .Select(x => array[y, x])
                        .ToArray();

        for (int i = row.Length-1; i >= 0; i--)
        {
            if(row[i]) return i+1;
        }
        return 0;
    }

    private static bool OnEdge(bool[,] array, int x, int y)
    {
        return x == array.GetLength(1)-1 || x == 0 || y == array.GetLength(0)-1 || y == 0;
    }

    private static Pipe[] GetConnectedPipes(char[,] grid, Pipe pipe)
    {
        var connectedPipes = new List<Pipe>();
        int[,] directions = pipe.GetDirections();

        // Console.Write(pipe);

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newX = pipe.X + directions[i, 0];
            int newY = pipe.Y + directions[i, 1];

            // Console.WriteLine($"{grid[newY, newX]} [{newX}, {newY}]");

            if (newX >= 0 && newX < grid.GetLength(1) &&
                newY >= 0 && newY < grid.GetLength(0) &&
                grid[newY, newX] != '.')
            {
                Pipe connectedPipe = new Pipe(grid[newY, newX], newX, newY);
                connectedPipes.Add(connectedPipe);
            }
        }
        return connectedPipes.ToArray();
    }

    private static Pipe GetNextNode(char[,] grid, Pipe current, Pipe previous)
    {
        // Console.WriteLine("Current:  " + current );
        Pipe[] connectedPipes = GetConnectedPipes(grid, current);
        Pipe pipe = connectedPipes[connectedPipes[0].Equals(previous) ? 1 : 0];

        // Console.WriteLine("Previous Pipe : " + previous);
        // Console.WriteLine("Connected Pipe : " + pipe + "\n");
        current = pipe;

        return current;
    }
    public static char[,] Get2DArray(string[] lines, out Pipe begin)
    {
        char[,] grid = new char[lines.Length, lines[0].Length];
        begin = new Pipe('.', 0, 0);

        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                grid[y,x] = lines[y][x];
                if(grid[y,x] == 'S')
                {
                    begin = new Pipe('F', x, y); // test cases
                    // begin = new Pipe('-', x, y); // data
                }
            }
        }
        return grid;
    }
}