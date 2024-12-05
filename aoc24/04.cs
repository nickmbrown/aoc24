public class Day4
{
    public const string word = "XMAS";
    public static Dictionary<Direction, Int2> directions = new();
    public static int total = 0;
    public enum Direction { N,S,E,W,NE,NW,SE,SW}

    public readonly struct Int2(int x, int y)
    {
        public int X { get; init; } = x;
        public int Y { get; init; } = y;
    }


    public static void Execute()
    {
        string[] input = File.ReadAllText("04.txt").Split('\n');

        directions = new Dictionary<Direction, Int2>
        {
            {Direction.N, new Int2(0, 1)},
            {Direction.S, new Int2(0, -1)},
            {Direction.E, new Int2(1, 0)},
            {Direction.W, new Int2(-1, 0)},
            {Direction.NE, new Int2(1, 1)},
            {Direction.NW, new Int2(-1, 1)},
            {Direction.SE, new Int2(1, -1)},
            {Direction.SW, new Int2(-1, -1)},
        };

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

        // Part 1 = 2569
        // Part1(grid);

        // Part 2 = 1998
        Part2(grid);
    }

    public static void Part1(char[,] grid)
    {
        total = 0;
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if(grid[x,y] == 'X')
                {
                    Search(grid,x,y);
                }
            }
        }

        Console.WriteLine(total);
    }

    public static void Search(char[,] grid, int x, int y)
    {
        foreach( var direction in directions)
        {
            SearchDirection(direction.Key, 1, grid, x + direction.Value.X, y + direction.Value.Y);
        }
    }

    public static void SearchDirection(Direction direction, int index, char[,] grid, int x, int y)
    {
        if(index == word.Length)
        {
            total ++;
            return;
        }

        if(x >= grid.GetLength(0) || y >= grid.GetLength(1) || x < 0 || y < 0) 
            return;


        if(grid[x, y] == word[index])
        {
            SearchDirection(direction, index + 1, grid, x + directions[direction].X, y + directions[direction].Y);
        }
    }

    public static void Part2(char[,] grid)
    {
        total = 0;

        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if(grid[x,y] == 'A')
                {
                    if(IsXMAS(grid, x, y))
                    {
                        total ++;
                    }
                }
            }
        }

        Console.WriteLine(total);
    }

    public static bool IsChar(char c, char[,] grid, int x, int y)
    {
        if(x >= grid.GetLength(0) || y >= grid.GetLength(1) || x < 0 || y < 0) 
            return false;

        if (grid[x,y] == c) 
            return true;

        return false;
    }

    public static bool IsXMAS(char[,] grid, int x, int y)
    {
        if( IsChar('S', grid, x + directions[Direction.NE].X, y + directions[Direction.NE].Y) &&
            IsChar('M', grid, x + directions[Direction.SW].X, y + directions[Direction.SW].Y) &&
            IsChar('S', grid, x + directions[Direction.NW].X, y + directions[Direction.NW].Y) &&
            IsChar('M', grid, x + directions[Direction.SE].X, y + directions[Direction.SE].Y))
            {
                return true;
            }
        if( IsChar('M', grid, x + directions[Direction.NE].X, y + directions[Direction.NE].Y) &&
            IsChar('S', grid, x + directions[Direction.SW].X, y + directions[Direction.SW].Y) &&
            IsChar('S', grid, x + directions[Direction.NW].X, y + directions[Direction.NW].Y) &&
            IsChar('M', grid, x + directions[Direction.SE].X, y + directions[Direction.SE].Y))
            {
                return true;
            }
        if( IsChar('S', grid, x + directions[Direction.NE].X, y + directions[Direction.NE].Y) &&
            IsChar('M', grid, x + directions[Direction.SW].X, y + directions[Direction.SW].Y) &&
            IsChar('M', grid, x + directions[Direction.NW].X, y + directions[Direction.NW].Y) &&
            IsChar('S', grid, x + directions[Direction.SE].X, y + directions[Direction.SE].Y))
            {
                return true;
            }
        if( IsChar('M', grid, x + directions[Direction.NE].X, y + directions[Direction.NE].Y) &&
            IsChar('S', grid, x + directions[Direction.SW].X, y + directions[Direction.SW].Y) &&
            IsChar('M', grid, x + directions[Direction.NW].X, y + directions[Direction.NW].Y) &&
            IsChar('S', grid, x + directions[Direction.SE].X, y + directions[Direction.SE].Y))
            {
                return true;
            }
        return false;
    }
}