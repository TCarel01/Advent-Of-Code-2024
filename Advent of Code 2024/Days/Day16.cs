using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day16
    {

        private AdventOfCode2024Parser daySixteenParser;

        public Day16()
        {
            daySixteenParser = new AdventOfCode2024Parser();
        }


        public int Day16Part1Solver(string filename)
        {
            List<List<string>> input = daySixteenParser.ParseInputAsArrayOfStrings(filename);

            var configGraph = GetConfigGraph(input);

            var reachedNodes = FindReachedNodes(configGraph);

            (int, int, int, int) curNode = (-1, -1, -1, -1);

            while (curNode == (-1, -1, -1, -1) || input[curNode.Item2][curNode.Item1] != "E")
            {
                curNode = DijkstraSearchStep(input, configGraph, reachedNodes);
            }

            return configGraph[curNode];
        }

        public int Day16Part2Solver(string filename)
        {
            List<List<string>> input = daySixteenParser.ParseInputAsArrayOfStrings(filename);

            var configGraph = GetConfigGraph(input);

            var reachedNodes = FindReachedNodes(configGraph);

            (int, int, int, int) curNode = (-1, -1, -1, -1);

            while (curNode == (-1, -1, -1, -1) || reachedNodes.Count != 0)
            {
                curNode = DijkstraSearchStep(input, configGraph, reachedNodes);
            }

            var configs = GetStartingConfigs(input, configGraph);

            input[configs[0].Item2][configs[0].Item1] = "O";

            foreach (var item in configs)
            {
                TraverseMazeBackwards(item, configGraph, input);
            }

            int count = 0;
            foreach (var item in input)
            {
                foreach (var item1 in item)
                {
                    if (item1 == "O")
                    {
                        count += 1;
                    }
                }
            }
            return count;
        }

        public Dictionary<(int, int, int, int), int> GetConfigGraph(List<List<string>> input)
        {
            Dictionary<(int, int, int, int), int> configGraph = new();

            for (int i = 0; i < input.Count; ++i)
            {
                for (int j = 0; j < input[i].Count; ++j)
                {
                    if (input[i][j] == "S")
                    {
                        configGraph[(j, i, 1, 0)] = 0;
                    }
                    else
                    {
                        configGraph[(j, i, 1, 0)] = -1;
                    }
                    configGraph[(j, i, -1, 0)] = -1;
                    configGraph[(j, i, 0, 1)] = -1;
                    configGraph[(j, i, 0, -1)] = -1;
                }
            }
             return configGraph;
        }

        public List<(int, int, int, int)> FindReachedNodes(Dictionary<(int, int, int, int), int> configGraph)
        {
            List<(int, int, int, int)> reachedNodes = new();

            foreach (var node in configGraph)
            {
                if (node.Value >= 0)
                {
                    reachedNodes.Add(node.Key);
                }
            }

            return reachedNodes;
        }

        public (int, int, int, int) DijkstraSearchStep(List<List<string>> input, Dictionary<(int, int, int, int), int> configGraph, List<(int, int, int, int)> reachedNodes)
        {
            int minCost = int.MaxValue;
            (int, int, int, int) curNode = (-1, -1, -1, -1);

            for (int i = 0; i < reachedNodes.Count; ++i)
            {
                int costToReach = configGraph[reachedNodes[i]];
                if (costToReach < minCost)
                {
                    minCost = costToReach;
                    curNode = reachedNodes[i];
                }
            }
            reachedNodes.Remove(curNode);

            (int, int) direction1 = curNode.Item3 % 2 == 0 ? (1, 0) : (0, 1);
            (int, int) direction2 = curNode.Item3 % 2 == 0 ? (-1, 0) : (0, -1);

            int reachCost = minCost + 1000;

            bool replaceNodeDir1 = replaceNode(configGraph, reachCost, (curNode.Item1, curNode.Item2, direction1.Item1, direction1.Item2));
            bool replaceNodeDir2 = replaceNode(configGraph, reachCost, (curNode.Item1, curNode.Item2, direction2.Item1, direction2.Item2));

            if (replaceNodeDir1)
            {
                reachedNodes.Add((curNode.Item1, curNode.Item2, direction1.Item1, direction1.Item2));
            }
            if (replaceNodeDir2)
            {
                reachedNodes.Add((curNode.Item1, curNode.Item2, direction2.Item1, direction2.Item2));
            }

            if (input[curNode.Item2 + curNode.Item4][curNode.Item1 + curNode.Item3] != "#")
            {
                int adjacentReachCost = minCost + 1;
                bool replaceNodeAdjacent = replaceNode(configGraph, adjacentReachCost, (curNode.Item1 + curNode.Item3, curNode.Item2 + curNode.Item4, curNode.Item3, curNode.Item4));
                if (replaceNodeAdjacent)
                {
                    reachedNodes.Add((curNode.Item1 + curNode.Item3, curNode.Item2 + curNode.Item4, curNode.Item3, curNode.Item4));
                }
            }

            return curNode;
        }

        public bool replaceNode(Dictionary<(int, int, int, int), int> configGraph, int reachCost, (int, int, int, int) curNode)
        {
            int testCost = configGraph[curNode];

            if (testCost == -1 || testCost > reachCost)
            {
                configGraph[curNode] = reachCost;
                return true;
            }

            return false;
        }

        public void TraverseMazeBackwards((int, int, int, int) curNode, Dictionary<(int, int, int, int), int> configGraph, List<List<string>> input)
        {
            int curNodeCost = configGraph[curNode];

            var traverseBackwards = (curNode.Item1 + (-1 * curNode.Item3), curNode.Item2 + (-1 * curNode.Item4), curNode.Item3, curNode.Item4);

            if (configGraph[traverseBackwards] != -1 && configGraph[traverseBackwards] == curNodeCost - 1)
            {
                input[traverseBackwards.Item2][traverseBackwards.Item1] = "O";
                TraverseMazeBackwards(traverseBackwards, configGraph, input);
            }

            (int, int) direction1 = curNode.Item3 % 2 == 0 ? (1, 0) : (0, 1);
            (int, int) direction2 = curNode.Item3 % 2 == 0 ? (-1, 0) : (0, -1);

            var directionNode1 = (curNode.Item1, curNode.Item2, direction1.Item1, direction1.Item2);
            var directionNode2 = (curNode.Item1, curNode.Item2, direction2.Item1, direction2.Item2);

            if (configGraph[directionNode1] != -1 && configGraph[directionNode1] == curNodeCost - 1000)
            {
                TraverseMazeBackwards(directionNode1, configGraph, input);
            }
            if (configGraph[directionNode2] != -1 && configGraph[directionNode2] == curNodeCost - 1000)
            {
                TraverseMazeBackwards(directionNode2, configGraph, input);
            }
        }

        public List<(int, int, int, int)> GetStartingConfigs(List<List<string>> input, Dictionary<(int, int, int, int), int> configGraph)
        {
            List<(int, int, int, int)> returnList = new();
            for (int i = 0; i < input.Count; ++i)
            {
                for (int j = 0; j < input[i].Count; ++j)
                {
                    if (input[i][j] == "E")
                    {
                        int minCost = MinAndGreaterThanOrEqualToZero([configGraph[(j, i, 0, 1)], configGraph[(j, i, 1, 0)], configGraph[(j, i, 0, -1)], configGraph[(j, i, -1, 0)]]);
                        if (configGraph[(j, i, 0, 1)] == minCost)
                        {
                            returnList.Add((j, i, 0, 1));
                        }
                        if (configGraph[(j, i, 1, 0)] == minCost)
                        {
                            returnList.Add((j, i, 1, 0));
                        }
                        if (configGraph[(j, i, 0, -1)] == minCost)
                        {
                            returnList.Add((j, i, 0, -1));
                        }
                        if (configGraph[(j, i, -1, 0)] == minCost)
                        {
                            returnList.Add((j, i, -1, 0));
                        }
                    }
                }
            }
            return returnList;
        }

        public int MinAndGreaterThanOrEqualToZero(List<int> values)
        {
            int minVal = int.MaxValue;
            foreach (var item in values)
            {
               if (item < minVal && item >= 0)
                {
                    minVal = item;
                } 
            };
            return minVal;
        }

        public void PrintInput(List<List<string>> input)
        {
            for (int i = 0; i < input.Count; ++i)
            {
                Console.WriteLine(string.Join("", input[i]));
            }
        }
    }
}
