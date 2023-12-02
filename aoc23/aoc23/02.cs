public class Day2
{
    static Dictionary<string, int> possibilities = new Dictionary<string, int>()
    {
        { "red", 12 },
        { "green", 13 },
        { "blue", 14 }
    };

    public static void Execute()
    {
        string[] games = File.ReadAllText("02.txt").Split("\n");
        int sum = 0;
        int powerSum = 0;

        foreach (var game in games)
        {
            string gameID = game.Split(':')[0].Split(' ')[1];
            string gameSets = game.Split(':')[1];
            string[] sets = gameSets.Split(';');

            if(IsGamePossible(sets))
            {
                int gameIDValue = int.Parse(gameID);
                sum += gameIDValue;
            }

            int power = 1;
            var minimums = FindMinimumPossibleValues(sets);
            foreach (var item in minimums)
            {
                power *= item.Value;
            }
            powerSum += power;
        }

        Console.WriteLine("\nSum of IDs: " + sum);
        Console.WriteLine("\nSum of Powers: " + powerSum);

        // Correct Answer
        // 2720

        // Correct Answer part deux
        // 71535
    }

    public static bool IsGamePossible(string[] sets)
    {
        foreach (var set in sets)
        {
            if(!IsSetPossible(set))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsSetPossible(string set)
    {
        string[] groups = set.Split(",");
        foreach(var group in groups)
        {
            string intString = group.Split(' ')[1];
            int value = int.Parse(intString);
            string color = group.Split(' ')[2].Trim();

            if(value > possibilities[color]) return false;
        }

        return true;
    }

    public static Dictionary<string, int> FindMinimumPossibleValues(string[] sets)
    {
        Dictionary<string, int> minimums = new Dictionary<string, int>
        {
            { "red", 0 },
            { "green", 0 },
            { "blue", 0 }
        };

        foreach (var set in sets)
        {
            string[] groups = set.Split(",");
            foreach(var group in groups)
            {
                string intString = group.Split(' ')[1];
                int value = int.Parse(intString);
                string color = group.Split(' ')[2].Trim();

                if(value > minimums[color])
                {
                    minimums[color] = value;
                }
            }
        }
        
        return minimums;
    }
}