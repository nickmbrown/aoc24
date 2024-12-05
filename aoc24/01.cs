public class Day1
{
    public static void Execute()
    {
        string[] lines = File.ReadAllText("01.txt").Split('\n');
        List<int> left  = new List<int>();
        List<int> right = new List<int>();

        foreach (string line in lines)
        {
            string[] lists = line.Split("   ");
            left.Add(int.Parse(lists[0]));
            right.Add(int.Parse(lists[1]));
        }

        // Part 1 = 2756096
        Part1(left, right);
        
        // Part 2 = 23117829
        Part2(left, right);
    }

    public static void Part1(List<int> left, List<int> right)
    {
        left.Sort();
        right.Sort();

        int totalDistance = 0;

        for (int i = 0; i < left.Count; i++)
        {
            int difference = Math.Abs(left[i] - right[i]);
            totalDistance += difference;
        }

        // Part 1 Correct Answer = 2756096
        Console.WriteLine(totalDistance);
    }

    public static void Part2(List<int> left, List<int> right)
    {
        Dictionary<int, int> numbers = new Dictionary<int, int>();

        foreach (var item in right)
        {
            if(!numbers.ContainsKey(item))
            {
                numbers.Add(item, 1);
            }
            else
            {
                numbers[item] = numbers[item] + 1;
            }
        }

        int similarityScore = 0;
        foreach (var item in left)
        {
            if(numbers.ContainsKey(item))
            {
                similarityScore += item * numbers[item];  
            }
        }

        Console.WriteLine(similarityScore);
    }
}