using System.Collections;
using System.Diagnostics;

public class Day7
{

    static Dictionary<char, int> cardValues = new Dictionary<char, int>
    {
        {'A', 13}, 
        {'K', 12}, 
        {'Q', 11}, 
        {'J', 0}, // changed from 10 to 0 for part 2
        {'T', 9},
        {'9', 8},
        {'8', 7},
        {'7', 6},
        {'6', 5},
        {'5', 4},
        {'4', 3},
        {'3', 2},
        {'2', 1},
    }; 

    public static void Execute()
    {
        Stopwatch sw = Stopwatch.StartNew();
        string[] lines = File.ReadAllText("07.txt").Split("\r\n");
        string[] hands = new string[lines.Length];
        Dictionary<string, int> handsWithBids = new Dictionary<string, int>();

        for (int i = 0; i < lines.Length; i++)
        {
            string[] split = lines[i].Split(" ");
            hands[i] = split[0];
            handsWithBids.Add(hands[i], int.Parse(split[1]));
        }

        Sort(hands, hands.Length);

        int rank = 1;
        int totalWinnings = 0;
        foreach(var hand in hands)
        {
            // int evalA = EvaulateHand(hand);
            // Console.ForegroundColor = hand.Contains('J') ? ConsoleColor.Red : ConsoleColor.White;
            // Console.WriteLine($"{hand} : Rank {rank} * Bid {handsWithBids[hand]} - {MapHandValue(evalA)}");
            totalWinnings += rank * handsWithBids[hand];
            rank++;
        }
        Console.ForegroundColor = ConsoleColor.White;

        // Correct Answer = 251927063
        Console.WriteLine("Part 1: " + totalWinnings);

        // Incorrect Answers
        // 253996199 - too low
        // 255144280 - too low
        // 255187293 - too low

        // Correct Answer = 255632664
        Console.WriteLine("Part 2: " + totalWinnings);
        Debug.Assert(totalWinnings == 255632664);
        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    static void Sort(string[] array, int n)
    {
        string temp;
        bool swapped;

        for(int i = 0; i < n - 1; i++) 
        {
            swapped = false;
            for(int j = 0; j < n - i - 1; j++) 
            {     
                // Console.WriteLine($"Comparing {array[j]} to {array[j+1]}");      
                if(CompareHands(array[j], array[j+1])) 
                {
                    // Swap arr[j] and arr[j+1]
                    temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                    swapped = true;
                }
            }
 
            if (swapped == false)
                break;
        }
    }

    static bool CompareHands(string a, string b)
    {
        int evalA = EvaulateHand(a);
        int evalB = EvaulateHand(b);
        // Console.WriteLine($"Comparing {a} to {b}: {MapHandValue(evalA)} > {MapHandValue(evalB)}");
        
        if (evalA == evalB)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if(cardValues[a[i]] == cardValues[b[i]]) continue;
                return cardValues[a[i]] > cardValues[b[i]];
            }

            return false;
        }
        else return evalA > evalB;

    }

    static int EvaulateHand(string hand)
    {  
        // Console.WriteLine("-- " + hand + " --");
        Dictionary<char,int> labels = new Dictionary<char,int>();
        for (int i = 0; i < hand.Length; i++)
        {
            int count = hand.Count(x => x == hand[i]);
            
            if(!labels.ContainsKey(hand[i]))
                labels.Add(hand[i], count);
        }

        var sortedLabels = from entry in labels orderby entry.Value descending select entry;

        int biggest = 0;
        int secondBiggest = 0;
        foreach(KeyValuePair<char,int> label in sortedLabels)
        {
            if(biggest == 0 && label.Key != 'J') 
            {
                biggest = label.Value;
                continue;
            }
            if(secondBiggest == 0 && label.Key != 'J') 
            {
                secondBiggest = label.Value;
                continue;
            }
        }


        int j = labels.ContainsKey('J') ? labels['J'] : 0;

        if(biggest + j == 5) return 7;                          // five of a kind
        if(biggest + j == 4) return 6;                          // four of a kind
        if(biggest + j == 3 && secondBiggest == 2) return 5;    // full house
        if(biggest + j == 3) return 4;                          // three of a kind
        if(biggest == 2 && secondBiggest + j == 2) return 3;    // two pairs
        if(biggest + j == 2) return 2;                          // one pair
        else return 1;                                          // high card
    }

    public static string MapHandValue(int value)
    {
        if(value == 7) return "Five of a Kind";
        else if(value == 6) return "Four of a Kind";
        else if(value == 5) return "Full House";
        else if(value == 4) return "Three of a Kind";
        else if(value == 3) return "Two Pair";
        else if(value == 2) return "One Pair";
        else return "High Card";
    } 
}