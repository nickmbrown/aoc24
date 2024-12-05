public class Day2
{
    public static void Execute()
    {
        string[] lines = File.ReadAllText("02.txt").Split('\n');

        // Part 1 = 326
        // Part1(lines);

        // Part 2 = 381
        Part2(lines);
    }

    public static void Part1(string[] lines)
    {
        int safeReports = 0;
        foreach (var report in lines)
        {
            if(IsReportSafe(report))
            {
                safeReports++;
            }
        }

        Console.WriteLine(safeReports);
    }

    public static void Part2(string[] lines)
    {
        int safeReports = 0;
        foreach (var report in lines)
        {
            if(IsReportSafe(report))
            {
                safeReports++;
            }
            else if(IsReportSafeWithDampener(report))
            {
                safeReports++;
            }
        }

        Console.WriteLine("Safe: " + safeReports);
    }

    public static bool IsReportSafeWithDampener(string report)
    {
        for(int i = 0; i < report.Split(" ").Length; i++)
        {
            List<int> levels = report.Split(" ").Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList();
            levels.RemoveAt(i);
                
            bool isIncreasing = levels[0] < levels[1];

            bool isSafe = true;
            for(int j = 1; j < levels.Count; j++)
            {
                if(isIncreasing && levels[j-1] >= levels[j]) 
                {
                    isSafe = false;
                }
                else if (!isIncreasing && levels[j-1] <= levels[j])
                {
                    isSafe = false;
                }

                int difference = Math.Abs(levels[j-1]-levels[j]);

                if(difference < 1 || difference > 3)
                {
                    isSafe = false;
                }
                
            }

            if (isSafe) return true;
        }

        return false;
    }

    public static bool IsReportSafe(string report)
    {
        List<int> levels = report.Split(" ").Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList();

        bool isIncreasing = levels[0] < levels[1];

        for(int i = 1; i < levels.Count; i++)
        {
            if(isIncreasing && levels[i-1] >= levels[i]) 
            {
                return false;
            }
            else if (!isIncreasing && levels[i-1] <= levels[i])
            {
                return false;
            }

            int difference = Math.Abs(levels[i-1]-levels[i]);

            if(difference < 1 || difference > 3)
            {
                return false;
            }
            
        }

        return true;
    }
}