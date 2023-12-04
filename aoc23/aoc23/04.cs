public class Day4
{
    public static void Execute()
    {
        string[] lines = File.ReadAllText("04.txt").Split("\n");
        int points = GetPoints(lines);
        
        // Correct Answer = 20829
        Console.WriteLine(points);

        int numScratchcards = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            numScratchcards += GetNumScratchcards(lines, i);
        }
        // Correct Answer = 12648035
        Console.WriteLine("Number of Scratchcards: " + numScratchcards);
    }

    private static int GetPoints(string[] lines)
    {
        int sum = 0;

        foreach (var line in lines)
        {
            int matches = 0;
            int intValue = 0;
            double points = 0;

            string valuesString = line.Remove(0, line.IndexOf(':') + 1);
            string[] winningStrings = valuesString.Remove(valuesString.IndexOf('|'), valuesString.Length - valuesString.IndexOf('|')).Split(' ');
            string[] yourStrings = valuesString.Remove(1, valuesString.IndexOf('|')).Split(' ');

            var winningValues = from value in winningStrings
                                where int.TryParse(value, out intValue)
                                select intValue;

            var yourValues = from value in yourStrings
                             where int.TryParse(value, out intValue)
                             select intValue;

            foreach (var winningValue in winningValues)
            {
                foreach (var value in yourValues)
                {
                    if (value == winningValue)
                    {
                        Console.WriteLine("Match found: " + value + " and " + winningValue);
                        matches++;
                    }
                }
            }

            points = Math.Pow(2, matches - 1);
            Console.WriteLine("Matches: " + matches);
            Console.WriteLine("Points: " + points);
            sum += (int)points;
        }

        return sum;
    }


    // Woof this is so inefficient that it took like 60 seconds to calculate on the full input
    private static int GetNumScratchcards(string[] lines, int index)
    {
        // Console.WriteLine(lines[index]);
        string line = lines[index];
        int intValue = 0;
        int i = index;
        int number = 1;

        string valuesString = line.Remove(0, line.IndexOf(':') + 1);
        string[] winningStrings = valuesString.Remove(valuesString.IndexOf('|'), valuesString.Length - valuesString.IndexOf('|')).Split(' ');
        string[] yourStrings = valuesString.Remove(1, valuesString.IndexOf('|')).Split(' ');

        var winningValues = from value in winningStrings
                            where int.TryParse(value, out intValue)
                            select intValue;

        var yourValues = from value in yourStrings
                            where int.TryParse(value, out intValue)
                            select intValue;

        foreach (var winningValue in winningValues)
        {
            foreach (var value in yourValues)
            {
                if (value == winningValue)
                {
                    // Console.WriteLine("Match found: " + value + " and " + winningValue);
                    i++;
                    if(i >= lines.Length) continue;
                
                    number += GetNumScratchcards(lines, i);
                }
            }
        }

        return number;
    }

    private static void Part1(string[] lines)
    {

    }
}