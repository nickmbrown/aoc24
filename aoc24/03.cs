using System.Text.RegularExpressions;

public class Day3
{
    public static void Execute()
    {
        string input = File.ReadAllText("03.txt");

        // Part 1 = 169021493
        // Part1(input);

        // Part 2 = 111762583
        Part2(input);
    }

    public static void Part1(string input)
    {
        string pattern = @"(mul\()[0-9]{1,3}(\,)[0-9]{1,3}(\))";
        MatchCollection matches = Regex.Matches(input, pattern);

        int sum = 0;
        foreach (Match match in matches)
        {
            string numPattern = @"[0-9]{1,3}";
            string[] values = match.Value.Split(',');

            int one = int.Parse(Regex.Match(values[0], numPattern).Value);
            int two = int.Parse(Regex.Match(values[1], numPattern).Value);

            sum += one * two;
        }

        Console.WriteLine(sum);
    }

    public static void Part2(string input)
    {
        string pattern = @"(mul\()[0-9]{1,3}(\,)[0-9]{1,3}(\))|(don\'t\(\))|(do\(\))";
        MatchCollection matches = Regex.Matches(input, pattern);

        bool enabled = true;
        int sum = 0;
        foreach (Match match in matches)
        {
            if(match.Value == "don't()")
            {
                enabled = false;
                continue;
            }
            else if (match.Value == "do()")
            {
                enabled = true;
                continue;
            }

            if(!enabled) continue;

            string numPattern = @"[0-9]{1,3}";
            string[] values = match.Value.Split(',');

            int one = int.Parse(Regex.Match(values[0], numPattern).Value);
            int two = int.Parse(Regex.Match(values[1], numPattern).Value);

            sum += one * two;
        }

        Console.WriteLine(sum);
    }
}