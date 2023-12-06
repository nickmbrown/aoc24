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
            Console.WriteLine($"Time: {times[i]} Distance: {distances[i]}");
            numWaysToWin[i] = GetNumWaysToWin(times[i], distances[i]);
        }

        // Correct Answer = 4403592
        Console.WriteLine("Part 1: " + numWaysToWin.Aggregate(1, (a, b) => a * b));
        
        Console.WriteLine("Part 2: " + GetNumWaysToWin(bigTime, bigDistance));

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    private static int GetNumWaysToWin(long time, long distance)
    {
        int holdTime = 0;
        int waysToWin = 0;

        while (holdTime <= time)
        {
            long travelTime = time - holdTime;
            int speed = holdTime;

            long distanceTraveled = travelTime * speed;

            if (distanceTraveled > distance)
            {
                waysToWin++;
            }

            holdTime++;
        }

        return waysToWin;
    }
}