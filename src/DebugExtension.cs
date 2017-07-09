using System;
using System.Collections.Generic;
using System.Linq;

namespace DokuWiki
{
    public static class DebugExtension
    {
        internal static void PrintNodes(this List<Node> nodes, bool recursively = false)
        {
            foreach (var node in nodes)
            {
                Console.WriteLine("Node " + node.Type);
                Console.WriteLine("start: " + node.StartPosition);
                Console.WriteLine("end: " + node.EndPosition);
                Console.WriteLine("Child Nodes: " + node.Nodes.Count);
                Console.WriteLine("content:\n--" + node.Content + "--");
                if (recursively && node.Nodes.Any())
                {
                    Console.WriteLine("printing child nodes recursively:");
                    PrintNodes(node.Nodes, recursively);
                }
            }
        }

        internal static void PrintNodes(this Node[] nodes, bool recursively = false)
        {
            PrintNodes(new List<Node>(nodes), recursively);
        }
    }
}