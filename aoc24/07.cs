public class Day7
{
    public static void Execute()
    {
        string[] input = File.ReadAllText("07.txt").Split('\n');

        // 68549683 is too low
        // 1095331337 is too low
        // Part 1 = 
        Part1(input);

        // Part 2 = 
        //Part2(input);
    }

    private static void Part1(string[] input)
    {
        long total = 0;
        foreach(string line in input)
        {
            string[] split = line.Split(':');
            long answer = long.Parse(split[0].Trim());

            string[] numberStrings = split[1].Trim().Split(' ');

            //Console.Write(answer + " : ");

            //foreach(string number in numberStrings)
            //{
            //    Console.Write(number.Trim() + " ");
            //}
            //Console.WriteLine();


            List<int> numbers = ConvertStringsToInts(numberStrings);

            // 81 40 27
            // 81 * 40 * 27
            // 81 + 40 + 27 
            // 81 * 40 + 27
            // 81 + 40 * 27

            bool isValid = TryOperators(answer, numbers);

            Console.WriteLine("Output: " + isValid);
            if (isValid)
            {
                Console.WriteLine(answer  + " - Is valid");
                total += answer;
            }
            else
            {
                Console.WriteLine(answer + " - Is NOT valid");
            }
            Console.WriteLine();
        }

        Console.WriteLine("Total: " + total);

    }

    private static bool TryOperators(long answer, List<int> numbers)
    {
        for(int i = 0; i < numbers.Count * numbers.Count; i++)
        {
            int total = numbers[0];
            string printString = i + ": " + total.ToString();

            for (int j = 1; j < numbers.Count; j++)
            {
                if ((i/j) == i)
                {
                    total += numbers[j];
                    printString += $" + {numbers[j]}";
                }
                else
                {
                    total *= numbers[j];
                    printString += $" * {numbers[j]}";
                }

                if (j < numbers.Count-1)
                    continue;
                else
                    Console.WriteLine(printString + " = " + total);

                if (total == answer)
                {
                    return true;
                } 
            }
        }

        return false;
    }


    private static void Part2(char[,] grid)
    {
    }

    private static List<int> ConvertStringsToInts(string[] input)
    {
        List<int> numbers = [];
        foreach (var item in input)
        {
            numbers.Add(int.Parse(item.Trim()));
        }

        return numbers;
    }


}
