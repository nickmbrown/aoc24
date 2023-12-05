using System.Collections;
using System.Diagnostics;

public class Day5
{
    public static void Execute()
    {
        Stopwatch sw = Stopwatch.StartNew();
        string[] lines = File.ReadAllText("05.txt").Split("\r\n");

        long[] seeds = Array.ConvertAll(lines[0].Remove(0, 7).Split(' '), long.Parse);
        int[] mapIndices = GetMapStartIndices(lines);

        List<long> inputs = seeds.ToList();
        long lowestLocationNumber = long.MaxValue;

        for (long i = 0; i < mapIndices.Length; i++)
        {
            List<long> outputs = new List<long>();
            int index = mapIndices[i] + 1;
            int length = 0;
            while (index < lines.Length && lines[index] != "")
            {
                index++;
                length++;
            }

            Map map = new Map(new ArraySegment<string>(lines, mapIndices[i] + 1, length));
            foreach (var input in inputs)
            {
                long mapValue = map.Lookup(input);
                outputs.Add(mapValue);
                // Console.WriteLine("Input: " + input + " Mapping: " + mapValue);

                if(i == mapIndices.Length-1)
                {
                    if(mapValue < lowestLocationNumber) lowestLocationNumber = mapValue;
                }
            }

            inputs = outputs;
        }

        // Correct Answer = 825516882
        Console.WriteLine("Lowest Location Number: " + lowestLocationNumber);

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    public static void Execute2()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine(DateTime.Now.ToShortTimeString());
        string[] lines = File.ReadAllText("05.txt").Split("\r\n");
        long lowestLocationNumber = long.MaxValue;
        BlockRange[] blockRanges = GetBlockRanges(lines);
        string[] seeds = lines[0].Remove(0, 7).Split(' ');

        Map[] maps = new Map[blockRanges.Length];
        for (int i = 0; i < maps.Length; i++)
        {
            maps[i] = new Map(new ArraySegment<string>(lines, blockRanges[i].Start, blockRanges[i].Length));
        }

        Parallel.ForEach(GetSeeds(seeds), seed =>
        {
            long result = GetLocationFromSeed(maps, seed);
            if(result < lowestLocationNumber) 
                lowestLocationNumber = result;
        });
        
        // Correct Answer = 136096660
        // This took 159925ms to complete :0)
        Console.WriteLine("Lowest Location Number: " + lowestLocationNumber);
        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    private static long GetLocationFromSeed(Map[] maps, long seed)
    {
        long location = seed;
        for (long i = 0; i < maps.Length; i++)
        {
            location = maps[i].Lookup(location);
        }
        return location;
    }

    private static long GetSeedFromLocation(string[] lines, BlockRange[] blocks, long location)
    {
        long seed = location;
        for (long i = blocks.Length - 1; i >= 0; i--)
        {
            Map map = new Map(new ArraySegment<string>(lines, blocks[i].Start, blocks[i].Length));
            location = map.Lookup(location);
        }

        return seed;
    }
    
    private static IEnumerable<long> GetSeeds(string[] seeds)
    {
        for (int i = 0; i < seeds.Length; i+=2)
        {
            long start = long.Parse(seeds[i]);
            long length = long.Parse(seeds[i+1]);

            for (long j = start; j < start + length; j++)
            {
                yield return j;
            }
        }
    }

    private static int[] GetMapStartIndices(string[] lines)
    {
        int[] mapIndices = new int[7];
        int startIndex = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains('-'))
            {
                mapIndices[startIndex] = i;
                startIndex++;
            }
        }
        return mapIndices;
    }

    private static BlockRange[] GetBlockRanges(string[] lines)
    {
        int[] mapIndices = GetMapStartIndices(lines);
        BlockRange[] ranges = new BlockRange[mapIndices.Length];
        for (long i = 0; i < mapIndices.Length; i++)
        {
            ranges[i].Start = mapIndices[i] + 1;
            int index = ranges[i].Start;
            int length = 0;
            while (index < lines.Length && lines[index] != "")
            {
                index++;
                length++;
            }
            ranges[i].Length = length;
        }

        return ranges;
    }

    public class Map
    {
        public MapRange[] MapRanges;
        public Map(ArraySegment<string> mapLines)
        {
            MapRange[] mapRanges = new MapRange[mapLines.Count]; 
            for (int i = 0; i < mapLines.Count; i++)
            {
                long[] values = Array.ConvertAll(mapLines[i].Split(' '), long.Parse);
                mapRanges[i] = new MapRange(values[0], values[1], values[2]);
                
            }

            MapRanges = mapRanges;
        }

        public long Lookup(long source)
        {

            foreach (var range in MapRanges)
            {                
                if(source < range.SourceRangeStart || source > range.SourceRangeStart + range.RangeLength)
                    continue;
                
                return range.DestinationRangeStart - range.SourceRangeStart + source;
            }


            return source;
        }
    }

    public struct MapRange
    {
        public MapRange(long destinationRangeStart, long sourceRangeStart, long rangeLength)
        {
            DestinationRangeStart = destinationRangeStart;
            SourceRangeStart = sourceRangeStart;
            RangeLength = rangeLength;
        }

        public long DestinationRangeStart { get; } 
        public long SourceRangeStart { get; } 
        public long RangeLength { get; } 
    }

    public struct BlockRange
    {
        public BlockRange(int start, int length)
        {
            Start = start;
            Length = length;
        }
        public int Start;
        public int Length;
    }
}