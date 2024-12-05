using System;

public class Day5
{
    public static void Execute()
    {
        string[] input = File.ReadAllText("05.txt").Split(Environment.NewLine+Environment.NewLine);

        string[] rules = input[0].Split('\n');
        string[] updates = input[1].Split('\n');

        // Part 1 = 5651
        // Part1(rules, updates);

        // Part 2 = 4743
        Part2(rules, updates);
    }

    public static void Part1(string[] rules, string[] updates)
    {
        int total = 0;
        foreach(string update in updates)
        {
            List<string> pages = update.Trim().Split(',').ToList();
            if(IsInCorrectOrder(rules, pages))
            {
                int middlePage = int.Parse(pages[pages.Count/2]);
                total += middlePage;
            }
        }
        Console.WriteLine($"Total: {total}");
    }

    public static bool IsInCorrectOrder(string[] rules, List<string> pages)
    {
        foreach(string rule in rules)
        {
            string[] ruleSplit = rule.Split('|');

            int index1 = pages.IndexOf(ruleSplit[0].Trim());
            int index2 = pages.IndexOf(ruleSplit[1].Trim());

            if(index1 < 0 || index2 < 0) 
                continue;
            if(index1 > index2) 
                return false;
        }

        return true;
    }

    public static void Part2(string[] rules, string[] updates)
    {
        int total = 0;
        foreach(string update in updates)
        {
            List<string> pages = update.Trim().Split(',').ToList();
            if(!IsInCorrectOrder(rules, pages))
            {
                pages = ReorderPages(rules, pages);
                int middlePage = int.Parse(pages[pages.Count/2]);
                total += middlePage;
            }
        }
        Console.WriteLine($"Total: {total}");
    }

    public static List<string> ReorderPages(string[] rules, List<string> pages)
    {
        List<string> newPages = pages;

        foreach(string rule in rules)
        {
            string[] ruleSplit = rule.Split('|');

            int index1 = newPages.IndexOf(ruleSplit[0].Trim());
            int index2 = newPages.IndexOf(ruleSplit[1].Trim());

            if(index1 < 0 || index2 < 0) 
                continue;
            if(index1 > index2) 
            {
                (newPages[index2], newPages[index1]) = (newPages[index1], newPages[index2]);
                newPages = ReorderPages(rules, newPages);
            }
        }

        return newPages;
    }
}