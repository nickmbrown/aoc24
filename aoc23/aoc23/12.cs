using System.Diagnostics;

public class Day12
{
    public static void Execute()
    {
        Stopwatch sw = Stopwatch.StartNew();
        string[] lines = File.ReadAllText("12.txt").Split("\r\n");

        int index = 0;
        int total = 0;

        // Parallel.ForEach(lines,
        //     line =>
        //     {
        //         string springs = line.Split(' ')[0];
        //         int[] groups = line.Split(' ')[1]
        //             .Split(',')
        //             .Where(x => int.TryParse(x, out _))
        //             .Select(int.Parse)
        //             .ToArray();

        //         // total += GetNumberOfPossibleArrangments(line);
        //         Interlocked.Add(ref total, GetNumberOfPossibleArrangments(line));
        //         Interlocked.Add(ref index, 1);

        //         Console.WriteLine($"{index}/{lines.Length} Total={total}");
        //     });

        foreach (var line in lines)
        {
            string springs = line.Split(' ')[0];
            int[] groups = line.Split(' ')[1]
                .Split(',')
                .Where(x => int.TryParse(x, out _))
                .Select(int.Parse)
                .ToArray();

            total += GetNumberOfPossibleArrangments(line);
        }
        
        Console.WriteLine($"Total: {total}");

        // First 500  = 
        // Second 500 = 
        // Total = 

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    private static int GetNumberOfPossibleArrangments(string line)
    {
        string springs = line.Split(' ')[0];
        int[] groups = line.Split(' ')[1]
            .Split(',')
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .ToArray();

        HashSet<string> visitedCombinations = new HashSet<string>();
        int possibilities = Evaluate(springs.ToCharArray(), groups, visitedCombinations);

        Console.Write($"\n{new string(springs)} - {possibilities} arrangement ");
        return possibilities;
    }

    private static int Evaluate(char[] springs, int[] groups, HashSet<string> visitedCombinations)
    {
        string currentCombination = new(springs);
        if (visitedCombinations.Contains(currentCombination))
            return 0;

        visitedCombinations.Add(currentCombination);

        if (!springs.Contains('?'))
            return IsValid(springs, groups) ? 1 : 0;
        
        int sum = 0;
        for (int i = 0; i < springs.Length; i++)
        {
            if(springs[i] == '?')
            {
                springs[i] = '.';
                sum += Evaluate(springs, groups, visitedCombinations);
                springs[i] = '#';
                sum += Evaluate(springs, groups, visitedCombinations);
                springs[i] = '?';
            }           
        }
        
        return sum;
    }

    private static bool IsValid(char[] springs, int[] groups)
    {                
        if(springs.Count(c => c == '#') != groups.Sum()) 
            return false;

        string springString = new string(springs);
        string[] springGroups = springString.Split('.').Where(s => s.Length > 0).ToArray();

        for (int i = 0; i < springGroups.Length; i++)
        {
            if(springGroups[i].Length != groups[i]) return false;
        }

        return true;
    }
}