using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day20
    {

        public AdventOfCode2024Parser dayTwentyParser = new AdventOfCode2024Parser();

        public int Day20Part1Solver(string filename)
        {
            List<List<string>> input = dayTwentyParser.ParseInputAsArrayOfStrings(filename);

            (int, int) startPos = FindCharPos(input, "S");
            (int, int) endPos = FindCharPos(input, "E");

            List<(int, int)> pathThroughMaze = FindShortestPath(input, startPos, endPos);

            int counter = 0;

            for (int i = 0; i < pathThroughMaze.Count; ++i)
            {
                (int, int) curPos = pathThroughMaze[i];
                int curPathX = curPos.Item1;
                int curPathY = curPos.Item2;

                List<List<int>> directions = [[1, 0], [0, 1], [-1, 0], [0, -1]];

                foreach (List<int> direction in directions)
                {
                    (int, int) curLocationToTest = (curPathX + direction[0] * 2, curPathY + direction[1] * 2);

                    int testLocationIndex = pathThroughMaze.IndexOf(curLocationToTest);

                    int pathDifference = testLocationIndex - i - 2;

                    if (testLocationIndex >= 0 && pathDifference >= 100)
                    {
                        counter += 1;
                    }
                }
            }
            return counter;
        }

        public long Day20Part2Solver(string filename)
        {
            List<List<string>> input = dayTwentyParser.ParseInputAsArrayOfStrings(filename);

            (int, int) startPos = FindCharPos(input, "S");
            (int, int) endPos = FindCharPos(input, "E");

            List<(int, int)> pathThroughMaze = FindShortestPath(input, startPos, endPos);

            HashSet<((int, int), (int, int))> knownCheats = new HashSet<((int, int), (int, int))>();

            long counter = 0;
            int curTestNodeCount = 0;

            foreach ((int, int) startNode in pathThroughMaze)
            {
                ++curTestNodeCount;
                for (int i = -20; i <= 20; ++i)
                {
                    for (int j = -1 * (20 - Math.Abs(i)); j <= (20 - Math.Abs(i)); j++)
                    {
                        (int, int) cheatEndPos = (startNode.Item1 + i,  startNode.Item2 + j);
                        
                        if (pathThroughMaze.IndexOf(cheatEndPos) >= 0)
                        {
                            int pathLength = pathThroughMaze.IndexOf(cheatEndPos) - pathThroughMaze.IndexOf(startNode);

                            int pathDifference = pathLength - (Math.Abs(i) + Math.Abs(j));
                            
                            if (pathDifference >= 100)
                            {
                                knownCheats.Add((startNode, cheatEndPos));
                            }
                        }
                    }
                }
            }
            return knownCheats.Count;
        }


        public (int, int) FindCharPos(List<List<string>> input, string curChar)
        {
            for (int i = 0; i < input.Count; ++i)
            {
                for (int j = 0; j < input[i].Count; ++j)
                {
                    if (input[j][i] == curChar)
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }

        public List<(int, int)> FindShortestPath(List<List<string>> input, (int, int) startPos, (int, int) endPos)
        {
            Dictionary<(int, int), int> cache = new Dictionary<(int, int), int>();

            List<(int, int)> pathThroughMaze = new List<(int, int)>();

            List<(int, int)> nextNodes = new List<(int, int)>();
            List<(int, int)> curNodes = new List<(int, int)>();

            curNodes.Add(startPos);

            int totalStepCount = 0;

            while (curNodes.Count != 0)
            {
                foreach (var node in curNodes)
                {
                    if (!cache.ContainsKey(node))
                    {
                        pathThroughMaze.Add(node);
                        cache.Add(node, totalStepCount);

                        for (int y = Math.Max(0, node.Item2 - 1); y <= Math.Min(input.Count - 1, node.Item2 + 1); ++y)
                        {
                            for (int x = Math.Max(0, node.Item1 - 1); x <= Math.Min(input[0].Count - 1, node.Item1 + 1); ++x)
                            {
                                if ((x == node.Item1 || y == node.Item2) && input[y][x] != "#")
                                {
                                    nextNodes.Add((x, y));
                                }
                            }
                        }
                    }
                }

                curNodes = nextNodes;
                nextNodes = new List<(int, int)>();
                totalStepCount += 1;
            }

            if (!cache.ContainsKey(endPos))
            {
                return new List<(int, int)>();
            }

            return pathThroughMaze;
        }
    }
}
