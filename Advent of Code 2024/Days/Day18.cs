using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day18
    {
        private AdventOfCode2024Parser dayEighteenParser;

        public Day18()
        {
            dayEighteenParser = new AdventOfCode2024Parser();
        }

        public int Day18Part1Solver(string filename, int gridSize, int numSteps)
        {
            List<string> input = dayEighteenParser.ParseInputAsSingleArrayOfStrings(filename);

            List<List<int>> coords = input.Select(e => e.Split(",").Select(f => int.Parse(f)).ToList()).ToList();

            List<List<string>> board = GenerateBoard(gridSize);

            AddObstaclesToGrid(board, numSteps, coords);

            int returnVal = BFS(board, (0, 0), (gridSize - 1, gridSize - 1));

            return returnVal;
        }

        public List<List<string>> GenerateBoard(int gridsize)
        {
            List<List<string>> board = new List<List<string>>();
            for (int i = 0; i < gridsize; ++i)
            {
                List<string> curBoardLine = new();
                for (int j = 0; j < gridsize; ++j)
                {
                    curBoardLine.Add(".");
                }
                board.Add(curBoardLine);
            }

            return board;
        }

        public string Day18Part2Solver(string filename, int gridSize)
        {
            int counter = 0;
            while (Day18Part1Solver(filename, gridSize, counter) != -1)
            {
                ++counter;
            }

            List<string> input = dayEighteenParser.ParseInputAsSingleArrayOfStrings(filename);

            List<List<int>> coords = input.Select(e => e.Split(",").Select(f => int.Parse(f)).ToList()).ToList();

            return $"{coords[counter - 1][0]},{coords[counter - 1][1]}";
        }

        public int BFS(List<List<string>> graph, (int, int) startNode, (int, int) endNode)
        {
            Dictionary<(int, int), int> cache = new Dictionary<(int, int), int>();

            cache.Add(startNode, 0);

            List<(int, int)> curNodes = new List<(int, int)>();

            curNodes.Add(startNode);

            List<(int, int)> nextNodes = new List<(int, int)>();

            int iterationCounter = 0;

            bool endNodeFound = false;

            while (!endNodeFound)
            {
                if (iterationCounter > graph.Count * graph.Count)
                {
                    return -1;
                }
                while (curNodes.Count != 0)
                {
                    (int, int) nextNode = curNodes[0];

                    int nextNodeY = nextNode.Item1;
                    int nextNodeX = nextNode.Item2;

                    for (int y = nextNodeY - 1; y <= nextNodeY + 1; ++y)
                    {
                        for (int x = nextNodeX - 1; x <= nextNodeX + 1; ++x)
                        {
                            if (x != nextNodeX && y != nextNodeY)
                            {
                                continue;
                            }
                            if (Math.Max(y, 0) == y && Math.Min(y, graph.Count - 1) == y
                                && Math.Max(x, 0) == x && Math.Min(x, graph[0].Count - 1) == x)
                            {
                                if (graph[y][x] == "#")
                                {
                                    continue;
                                }

                                if (!cache.ContainsKey((y, x)))
                                {
                                    cache.Add((y, x), iterationCounter + 1);
                                    nextNodes.Add((y, x));
                                }

                                if ((y, x) == (graph.Count - 1, graph[0].Count - 1))
                                {
                                    endNodeFound = true;
                                }
                            }
                        }
                    }
                    curNodes.Remove(nextNode);
                }
                curNodes = nextNodes;
                nextNodes = new List<(int, int)>();
                ++iterationCounter;
            }
            return cache[endNode];
        }

        public void AddObstaclesToGrid(List<List<string>> board, int numObstacles, List<List<int>> coords)
        {
            for (int i = 0; i < numObstacles && i < coords.Count; ++i)
            {
                List<int> curObstacle = coords[i];

                board[curObstacle[0]][curObstacle[1]] = "#";
            }
        }
    }
}
