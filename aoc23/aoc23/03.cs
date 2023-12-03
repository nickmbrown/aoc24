public class Day3
{
    public struct Coord
    {
        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }

    public static void Execute()
    {
        string[] lines = File.ReadAllText("03.txt").Split("\n");
        Dictionary<Coord, string> numbers = GetNumbers(lines);
        Dictionary<Coord, int> possibleGears = new Dictionary<Coord, int>();
        List<int> gearRatios = new List<int>();
        List<int> validNumebrs = new List<int>();

        foreach (var item in numbers)
        {
            // Console.WriteLine(item.Value + " - " + item.Key.ToString() + " - " + item.Value.Length);
            bool hasAdjacentSymbol = false;

            for (int y = item.Key.Y - 1; y <= item.Key.Y + 1; y++)
            {
                if(y < 0 || y >= lines.Length) continue;

                for (int x = item.Key.X - item.Value.Length - 1; x <= item.Key.X; x++)
                {        
                    if(x < 0 || x >= lines[y].Length) continue;

                    // Console.WriteLine($"[{x},{y}]: {lines[y][x]}");

                    if(IsSymbol(lines[y][x]))
                    {
                        if(lines[y][x] == '*')
                        {
                            Coord coord = new Coord(x,y);
                            if(possibleGears.Keys.Contains(coord))
                            {
                                gearRatios.Add(possibleGears[coord] * int.Parse(item.Value));
                            }
                            else
                            {
                                possibleGears.Add(coord, int.Parse(item.Value));
                            }
                        }

                        hasAdjacentSymbol = true;
                        continue;
                    }
                }
            }

            if(hasAdjacentSymbol)
            {
                validNumebrs.Add(int.Parse(item.Value));
            }
        }

        int sum = 0;
        foreach (var item in validNumebrs)
        {
            sum += item;
        }

        Console.WriteLine("Sum: " + sum);

        int gearRatiosSum = 0;
        foreach (var item in gearRatios)
        {
            gearRatiosSum += item;
        }
        Console.WriteLine("Gear Ratio Sum: " + gearRatiosSum);
    }

    private static Dictionary<Coord, string> GetNumbers(string[] lines)
    {
        Dictionary<Coord, string> numbers = new Dictionary<Coord, string>();

        int y = 0;
        foreach (var line in lines)
        {
            int x = -1;
            string currentString = "";
            foreach (char c in line)
            {
                x++;
                if (char.IsNumber(c))
                {
                    currentString += c;
                }
                else
                {
                    if (string.IsNullOrEmpty(currentString)) continue;

                    numbers.Add(new Coord(x, y), currentString);
                    currentString = "";
                }
            }
            y++;
        }
        return numbers;
    }

    public static bool IsSymbol(char c)
    {
        if(c == '.') return false;
        if(char.IsSymbol(c) || char.IsPunctuation(c))
        {
            return true;
        }
        return false;
    }
}