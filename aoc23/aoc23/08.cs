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
        long lcm = lcm_of_array_elements(pathLengths);
        Console.WriteLine(lcm);

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    // Grabbed from Geeks For Geeks
    // https://www.geeksforgeeks.org/lcm-of-given-array-elements/
    public static long lcm_of_array_elements(double[] element_array)
    {
        long lcm_of_array_elements = 1;
        int divisor = 2;
         
        while (true) {
             
            int counter = 0;
            bool divisible = false;
            for (int i = 0; i < element_array.Length; i++) {
 
                // lcm_of_array_elements (n1, n2, ... 0) = 0.
                // For negative number we convert into
                // positive and calculate lcm_of_array_elements.
                if (element_array[i] == 0) {
                    return 0;
                }
                else if (element_array[i] < 0) {
                    element_array[i] = element_array[i] * (-1);
                }
                if (element_array[i] == 1) {
                    counter++;
                }
 
                // Divide element_array by devisor if complete
                // division i.e. without remainder then replace
                // number with quotient; used for find next factor
                if (element_array[i] % divisor == 0) {
                    divisible = true;
                    element_array[i] = element_array[i] / divisor;
                }
            }
 
            // If divisor able to completely divide any number
            // from array multiply with lcm_of_array_elements
            // and store into lcm_of_array_elements and continue
            // to same divisor for next factor finding.
            // else increment divisor
            if (divisible) {
                lcm_of_array_elements = lcm_of_array_elements * divisor;
            }
            else {
                divisor++;
            }
 
            // Check if all element_array is 1 indicate 
            // we found all factors and terminate while loop.
            if (counter == element_array.Length) {
                return lcm_of_array_elements;
            }
        }
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