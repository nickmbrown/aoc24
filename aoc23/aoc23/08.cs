using System.Collections;
using System.Diagnostics;

public class Day8
{
    public static void Execute()
    {
        Stopwatch sw = Stopwatch.StartNew();
        string[] lines = File.ReadAllText("08.txt").Split("\r\n");

        string instructions = lines[0];
        Dictionary<string, Node> nodes = new Dictionary<string, Node>(); 
        lines[2..].ToList().ForEach(node => nodes.Add(node[..3], new Node(node[7..10], node[12..15])));

        Console.WriteLine($"{instructions} - {instructions.Length}");

        double steps = 0;
        string currentNode = "AAA";
        while(currentNode != "ZZZ")
        {
            double index = steps % instructions.Length;
            int i = (int)index;
           
            //Console.WriteLine($"Step {steps} - Current Node = {currentNode} - Instruction = {instructions[i]}");
            currentNode = instructions[i] == 'R' ? nodes[currentNode].Right : nodes[currentNode].Left;
            steps++;
        }

        Console.WriteLine($"Steps: {steps}");
        Debug.Assert(steps == 14893);

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    public static void Execute2()
    {
        Stopwatch sw = Stopwatch.StartNew();
        string[] lines = File.ReadAllText("08.txt").Split("\r\n");

        string instructions = lines[0];
        Dictionary<string, Node> nodes = new Dictionary<string, Node>(); 
        List<string> currentNodes = new List<string>();

        foreach (var node in lines[2..])
        {
            nodes.Add(node[..3], new Node(node[7..10], node[12..15]));
            if(node[2] == 'A') 
                currentNodes.Add(node[..3]);
        }

        Console.WriteLine($"{instructions} - {instructions.Length}");

        double[] pathLengths = new double[currentNodes.Count];

        Parallel.ForEach(currentNodes, node =>
        {
            double steps = 0;
            string currentNode = node;                

            while(currentNode[2] != 'Z')
            {
                double index = steps % instructions.Length;
                int intIndex = (int)index;
                try
                {
                    // Console.WriteLine($"Step {index} - Current Node = {currentNode} - Instruction = {instructions[intIndex]}");
                    currentNode = instructions[intIndex] == 'R' ? nodes[currentNode].Right : nodes[currentNode].Left;
                    steps++;
                }
                catch
                {
                    Console.WriteLine("Index of out bounds!");
                    Console.WriteLine($"Steps: {steps} Length: {instructions.Length}");
                }
            }

            pathLengths[currentNodes.IndexOf(node)] = steps;
        });

        // Initially tried to brute force this and got nowhere.
        // I stole this idea from a reddit post so I'll admit defeat for part 2 :0
        long lcm = Utils.lcm_of_array_elements(pathLengths);
        Console.WriteLine(lcm);
        Debug.Assert(lcm == 10241191004509);

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }


    public class Node
    {
        public Node(string left, string right)
        {
            Left = left;
            Right = right;
        }

        public string Left;
        public string Right;
    }
}