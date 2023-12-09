using System.Collections;
using System.Diagnostics;

public class Day9
{
    public static void Execute()
    {
        Stopwatch sw = Stopwatch.StartNew();
        string[] lines = File.ReadAllText("09.txt").Split("\r\n");

        int sumForward = 0;
        int sumBackward = 0;

        foreach (var line in lines)
        {
            // Console.WriteLine(line);

            var values = line.Split(' ')
                .Where(x => int.TryParse(x, out _))
                .Select(int.Parse)
                .ToList();

            List<List<int>> differences = new() { values };

            while(!values.All(n => n == 0))
            {
                values = values.Zip(values.Skip(1), (current, next) => next - current).ToList();
                differences.Add(values);
            }

            sumForward += ExtrapolateForward(differences);
            sumBackward += ExtrapolateBackward(differences);
        }

        // Correct Answer = 1584748274
        Console.WriteLine($"Sum of extrapolated values forward: {sumForward}");
        Debug.Assert(sumForward == 1584748274);

        // Correct Answer = 1026
        Console.WriteLine($"Sum of extrapolated values backward: {sumBackward}");
        Debug.Assert(sumBackward == 1026);

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    private static int ExtrapolateForward(List<List<int>> differences)
    {
        for (int j = differences.Count - 1; j > 0; j--)
        {
            List<int> currentRow = differences[j];
            List<int> rowAbove = differences[j - 1];

            int lastValue = currentRow[^1];
            int lastValueAbove = rowAbove[^1];

            int newValue = lastValue + lastValueAbove;

            rowAbove.Add(newValue);
        }

        return differences[0][^1];
    }

    private static int ExtrapolateBackward(List<List<int>> differences)
    {
        for (int j = differences.Count - 1; j > 0; j--)
        {
            List<int> currentRow = differences[j];
            List<int> rowAbove = differences[j - 1];

            int firstValue = currentRow[0];
            int firstValueAbove = rowAbove[0];
            int newValue = firstValueAbove - firstValue;
            rowAbove.Insert(0, newValue);
        }

        return differences[0][0];
    }
}