public class Day1
{
    public static void Execute()
    {
        string[] lines = File.ReadAllText("01.txt").Split("\n");
        int sum = 0;
        
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            string firstDigit = GetDigit(line);
            string lastDigit = GetDigit(line, true);

            if (int.TryParse(firstDigit + lastDigit, out int n))
            {
                sum += n;
            }
        }

        // Wrong Answers 
        // 53124
        // 52834
        // 52841

        Console.WriteLine();
        Console.WriteLine(sum);

        // Correct Answer!
        // 52840
    }

    private static string GetDigit(string currentLine, bool shouldReverse = false)
    {
        string digit = "";

        if(shouldReverse)
        {
            currentLine = Utils.ReverseString(currentLine);
        }

        for (int i = 0; i < currentLine.Length; i++)
        {
            var isDigit = int.TryParse(currentLine[i].ToString(), out _);
            if (isDigit)
            {
                digit = currentLine[i].ToString();
                break;
            }
            else
            {
                int wordIndex = GetContainedWordIndex(currentLine.Substring(i), shouldReverse);
                if(wordIndex != -1)
                {
                    digit = (wordIndex + 1).ToString();
                    break;
                }
            }
        }

        return digit;
    }

    private static int GetContainedWordIndex(string substring, bool shouldReverse = false)
    {
        string[] words = new string[]
        {  
            "one",
            "two",
            "three",
            "four",
            "five",
            "six", 
            "seven",
            "eight",
            "nine"
        };

        if(shouldReverse)
        {
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = Utils.ReverseString(words[i]);
            }
        }

        for (int i = 0; i < words.Length; i++)
        {
            if(substring.Length < words[i].Length) continue;
            string subsubstring = substring.Substring(0, words[i].Length);

            if (subsubstring.Equals(words[i]))
            {
                return i;
            }
        }

        return -1;
    }
}
