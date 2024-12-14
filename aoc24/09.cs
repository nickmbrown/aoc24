using System.Diagnostics.Tracing;

public class Day9
{    
    public static void Execute()
    {
        string input = File.ReadAllText("09.txt");

        // Part 1 = 
        Part1(input);

        // Part 2 = 
        // Part2(input);
    }

    private static void Part1(string input)
    {
        Console.WriteLine(input);
        int index = 0;
        bool isFreeSpace = false;

        foreach(char digit in input)
        {
            int num = digit  - '0';
            for(int i = 0; i < num; i ++)
            {
                if(!isFreeSpace)
                {
                    Console.Write(index);
                }
                else
                {
                    Console.Write('.');
                }
            }

            if(!isFreeSpace) index++;
            isFreeSpace = !isFreeSpace;
        }

        Console.WriteLine();
    }

    private static void Part2(string input)
    {

    }

}
