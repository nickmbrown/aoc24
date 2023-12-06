using System.Collections;
using System.Diagnostics;

public class Day6
{
    public static void Execute()
    {
        Stopwatch sw = Stopwatch.StartNew();
        string[] lines = File.ReadAllText("06.txt").Split("\r\n");

        var times = lines[0].Split(' ')
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .ToList();

        var distances = lines[1].Split(' ')
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .ToList();

        
        var bigTimeString = "";
        var bigDistanceString = "";
        for (int i = 0; i < times.Count; i++)
        {
            bigTimeString += times[i].ToString();
            bigDistanceString += distances[i].ToString();
        }

        var bigTime = long.Parse(bigTimeString);
        var bigDistance = long.Parse(bigDistanceString);


        int[] numWaysToWin = new int[times.Count];

        for (int i = 0; i < times.Count; i++)
        {
            // Console.WriteLine($"Time: {times[i]} Distance: {distances[i]}");
            numWaysToWin[i] = GetNumWaysToWin(times[i], distances[i]);
        }

        // Correct Answer = 4403592
        Console.WriteLine("Part 1: " + numWaysToWin.Aggregate(1, (a, b) => a * b));
        
        // Correct Answer = 38017587
        Console.WriteLine("Part 2: " + GetNumWaysToWin(bigTime, bigDistance));

        // Brute Force method is ~100ms
        // Doing it right is ~7ms

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    private static int GetNumWaysToWin(long time, long distance)
    {
        double sqrt = Math.Sqrt(time * time - 4 * (-1) * -distance);
        double x1 = (-time + sqrt) / -2;
        double x2 = (-time - sqrt) / -2;

        return (int)(Math.Ceiling(x2) - Math.Floor(x1)) - 1;
    }

    private static int GetNumWaysToWinBruteForce(long time, long distance)
    {
        int holdTime = 1;
        int waysToWin = 0;
        long travelTime, distanceTraveled;

        while (holdTime <= time)
        {
            travelTime = time - holdTime;
            distanceTraveled = travelTime * holdTime;

            if (distanceTraveled > distance)
            {
                waysToWin++;
            }

            holdTime++;
        }

        return waysToWin;
    }
}