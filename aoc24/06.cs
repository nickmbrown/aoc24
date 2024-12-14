using System.Diagnostics;

public class Day6
{
    public enum Direction { N, S, E, W, NE, NW, SE, SW }
    public static Dictionary<Direction, Coord> directions = [];
    public struct Coord(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;

        public override readonly string ToString()
        {
            return $"{X}, {Y}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Coord other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    public struct Vector2(Coord coord, Direction direction)
    {
        public Coord Coord { get; set; } = coord;
        public Direction Direction { get; set; } = direction;
        public override bool Equals(object? obj)
        {
            if (obj is Vector2 other)
            {
                return Coord.Equals(other.Coord) && Direction == other.Direction;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Coord, Direction);
        }
    }

    public static void Execute()
    {
        string[] input = File.ReadAllText("06.txt").Split('\n');

        directions = new Dictionary<Direction, Coord>
        {
            {Direction.N, new Coord(0, -1)},
            {Direction.S, new Coord(0, 1)},
            {Direction.E, new Coord(1, 0)},
            {Direction.W, new Coord(-1, 0)},
            {Direction.NE, new Coord(1, 1)},
            {Direction.NW, new Coord(-1, 1)},
            {Direction.SE, new Coord(1, -1)},
            {Direction.SW, new Coord(-1, -1)},
        };

        int x = 0;
        char[,] grid = new char[input[0].Trim().Length, input.Length];
        foreach (var row in input)
        {
            int y = 0;
            foreach (var col in row.Trim())
            {
                grid[x, y] = col;
                y++;
            }
            x++;
        }

        // Part 1 = 5551
        Part1(grid);

        // 1734 - too low
        // 1735 - too low
        // Part 2 = 
        Part2(grid);
    }

    private static void Part1(char[,] grid)
    {
        char[,] newGrid = new char[grid.GetLength(0), grid.GetLength(1)];
        Array.Copy(grid, newGrid, grid.Length);

        Coord startingPos = GetStartingPosition(newGrid);
        Direction startingDirection = GetStartingDirection(newGrid, startingPos);

        int numPositions = Patrol(newGrid, startingPos, startingDirection).Count;

        Console.WriteLine(numPositions);
    }

    private static void Part2(char[,] grid)
    {
        char[,] g1 = new char[grid.GetLength(1), grid.GetLength(1)];
        Array.Copy(grid, g1, grid.Length);

        Coord startingPos = GetStartingPosition(g1);
        Direction startingDirection = GetStartingDirection(g1, startingPos);

        HashSet<Vector2> positions = Patrol(g1, startingPos, startingDirection);

        int count = 0;

        foreach(var position in positions)
        {
            char[,] g2 = new char[grid.GetLength(1), grid.GetLength(1)];
            Array.Copy(grid, g2, grid.Length);
            g2[position.Coord.Y, position.Coord.X] = 'O';
            count += PatrolCountLoops(g2, startingPos, startingDirection);
        }

        Console.WriteLine(count);
    }

    private static HashSet<Vector2> Patrol(char[,] grid, Coord position, Direction direction)
    {
        HashSet<Vector2> positions = [];
        while (true)
        {
            if (grid[position.Y, position.X] != 'X')
            {
                grid[position.Y, position.X] = 'X';
                positions.Add(new Vector2(position, direction));
            }

            Coord nextPosition = new(position.X + directions[direction].X, position.Y + directions[direction].Y);

            if (nextPosition.X >= grid.GetLength(0) || nextPosition.Y >= grid.GetLength(1) || nextPosition.X < 0 || nextPosition.Y < 0)
            {
                return positions;
            }
            else if (grid[nextPosition.Y, nextPosition.X] == '#' || grid[nextPosition.Y, nextPosition.X] == 'O')
            {
                direction = TurnRight(direction);
                nextPosition = new(position.X + directions[direction].X, position.Y + directions[direction].Y);
            }

            position = nextPosition;
        }
    }

    private static int PatrolCountLoops(char[,] grid, Coord position, Direction direction)
    {
        HashSet<Vector2> positions = [];
        while (true)
        {
            //DrawCurrentGrid(grid, positions);

            if (positions.Contains(new Vector2(position, direction)))
            {
                //DrawCurrentGrid(grid, positions);
                return 1;
            }

            if (grid[position.Y, position.X] != 'X')
            {
                grid[position.Y, position.X] = 'X';
                positions.Add(new Vector2(position, direction));
            }

            Coord nextPosition = new(position.X + directions[direction].X, position.Y + directions[direction].Y);

            if (nextPosition.X >= grid.GetLength(0) || nextPosition.Y >= grid.GetLength(1) || nextPosition.X < 0 || nextPosition.Y < 0)
            {
                return 0;
            }
            else if (grid[nextPosition.Y, nextPosition.X] == '#' || grid[nextPosition.Y, nextPosition.X] == 'O')
            {
                direction = TurnRight(direction);
                nextPosition = new(position.X + directions[direction].X, position.Y + directions[direction].Y);
            }

            //Console.WriteLine($"{nextPosition} - {newGrid[nextPosition.Y, nextPosition.X]}");
            position = nextPosition;
        }
    }

    private static Direction TurnRight(Direction direction)
    {
        if (direction == Direction.N) return Direction.E;
        if (direction == Direction.E) return Direction.S;
        if (direction == Direction.S) return Direction.W;
        if (direction == Direction.W) return Direction.N;

        return Direction.N;
    }

    private static Direction GetStartingDirection(char[,] grid, Coord startingPos)
    {
        if (grid[startingPos.X, startingPos.Y] == '^') return Direction.N;
        if (grid[startingPos.X, startingPos.Y] == 'v') return Direction.S;
        if (grid[startingPos.X, startingPos.Y] == '>') return Direction.E;
        if (grid[startingPos.X, startingPos.Y] == '<') return Direction.W;

        return Direction.N;
    }

    private static Coord GetStartingPosition(char[,] grid)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[y, x] != '.' && grid[y,x] != '#')
                {
                    grid[y, x] = '.';
                    return new Coord(x, y);
                }
            }
        }

        return new Coord(0, 0);
    }

    private static void DrawCurrentGrid(char[,] grid, Coord currentPos, Direction currentDirection)
    {
        string toPrint = "";
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if(x == currentPos.X &&  y == currentPos.Y)
                {
                    if (currentDirection == Direction.N) toPrint += ('^');
                    else if (currentDirection == Direction.S) toPrint += ('v');
                    else if (currentDirection == Direction.E) toPrint += ('>');
                    else if (currentDirection == Direction.W) toPrint += ('<');
                }
                else
                {
                    toPrint += (grid[y, x]);
                }

            }
            toPrint += '\n';
        }
        Console.WriteLine(toPrint);
    }

    private static void DrawCurrentGrid(char[,] grid, HashSet<Vector2> positions)
    {
        char[,] newGrid = new char[grid.GetLength(0), grid.GetLength(1)];
        Array.Copy(grid, newGrid, grid.Length);

        string toPrint = "";
        foreach (Vector2 position in positions)
        {
            if (position.Direction == Direction.N || position.Direction == Direction.S)
                newGrid[position.Coord.Y, position.Coord.X] = '|';
            if (position.Direction == Direction.E || position.Direction == Direction.W)
                newGrid[position.Coord.Y, position.Coord.X] = '-';
        }

        for (int y = 0; y < newGrid.GetLength(1); y++)
        {
            for (int x = 0; x < newGrid.GetLength(0); x++)
            {
                toPrint += (newGrid[y, x]);
            }
            toPrint += '\n';
        }

        Console.WriteLine(toPrint);
    }
}
