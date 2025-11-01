using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day12
    {

        private AdventOfCode2024Parser dayTwelveParser;

        public Day12()
        {
            dayTwelveParser = new AdventOfCode2024Parser();
        }

        public int Day12Part1Solver(string filename)
        {
            List<List<string>> input = dayTwelveParser.ParseInputAsArrayOfStrings(filename);

            int totalPrice = CalcPrice(input, true);

            return totalPrice;
        }

        public int Day12Part2Solver(string filename)
        {
            List<List<string>> input = dayTwelveParser.ParseInputAsArrayOfStrings(filename);

            int totalPrice = CalcPrice(input, false);

            return totalPrice;
        }

        public int CalcPrice(List<List<string>> input, bool totalPerimeter)
        {
            List<List<string>> inputTest = input.Select(e => e.Select(f => ".").ToList()).ToList();

            int totalAreaCount = 0;
            int totalPerimeterCount = 0;

            int totalPrice = 0;

            for (int i = 0; i < input.Count; ++i)
            {
                for (int j = 0; j < input[0].Count; ++j)
                {
                    string curChar = input[i][j];

                    if (inputTest[i][j] != ",")
                    {
                        List<List<int>> wallCoords = new List<List<int>>();
                        List<int> areaPerimeter = TraverseSubGraph(input, inputTest, curChar, [0, 0], wallCoords, j, i);
                        totalAreaCount += areaPerimeter[0];
                        if (totalPerimeter)
                        {
                            totalPerimeterCount += areaPerimeter[1];
                        }

                        else
                        {
                            int wallCount = 0;
                            wallCoords = wallCoords.OrderBy(e => e[0]).ThenBy(e => e[1]).ToList();
                            for (int curWallCoordsIdx = 0; curWallCoordsIdx < wallCoords.Count; ++curWallCoordsIdx)
                            {
                                bool incrementWallCount = true;
                                List<int> curCoords = wallCoords[curWallCoordsIdx];
                                for (int restWallCoordsIdx = curWallCoordsIdx + 1;  restWallCoordsIdx < wallCoords.Count; ++restWallCoordsIdx)
                                {
                                    List<int> compareWallCoords = wallCoords[restWallCoordsIdx];
                                    if (curCoords[2] == compareWallCoords[2])
                                    {
                                        incrementWallCount = incrementWallCount && !((curCoords[2] == 0 || curCoords[2] == 2) && curCoords[1] == compareWallCoords[1] && Math.Abs(curCoords[0] - compareWallCoords[0]) == 1
                                            || (curCoords[2] == 1 || curCoords[2] == 3) && curCoords[0] == compareWallCoords[0] && Math.Abs(curCoords[1] - compareWallCoords[1]) == 1);
                                    }
                                }
                                wallCount = incrementWallCount ? wallCount + 1 : wallCount;
                            }
                            totalPerimeterCount = wallCount;
                        }
                    }

                    totalPrice += totalAreaCount * totalPerimeterCount;

                    totalAreaCount = 0;
                    totalPerimeterCount = 0;
                }
            }

            return totalPrice;
        }

        public List<int> TraverseSubGraph(List<List<string>> input, List<List<string>> inputTest, string curChar, List<int> areaPerimeter, List<List<int>> wallCoords, int curXPOS, int curYPOS)
        {
            if (inputTest[curYPOS][curXPOS] == ",")
            {
                return areaPerimeter;
            }

            areaPerimeter[0] += 1;

            inputTest[curYPOS][curXPOS] = ",";

            int minXPOS = curXPOS - 1;
            int maxXPOS = curXPOS + 1;

            int minYPOS = curYPOS - 1;
            int maxYPOS = curYPOS + 1;

            for (int y = minYPOS; y <= maxYPOS; ++y)
            {
                for (int x = minXPOS; x <= maxXPOS; ++x)
                {
                    if (x == curXPOS && y == curYPOS || x != curXPOS && y != curYPOS)
                    {
                        continue;
                    }

                    if (y < 0 || y >= input.Count || x < 0 || x >= input[0].Count || input[y][x] != curChar)
                    {
                        areaPerimeter[1] += 1;
                        
                        if (y == curYPOS)
                        {
                            if (x < curXPOS)
                            {
                                wallCoords.Add([x, y, 1]);
                            }
                            else if (x > curXPOS)
                            {
                                wallCoords.Add([x, y, 3]);
                            }
                        }
                        else if (x == curXPOS)
                        {
                            if (y < curYPOS)
                            {
                                wallCoords.Add([x, y, 0]);
                            }
                            else if (y > curYPOS)
                            {
                                wallCoords.Add([x, y, 2]);
                            }
                        }
                    }

                    else
                    {
                        TraverseSubGraph(input, inputTest, curChar, areaPerimeter, wallCoords, x, y);
                    }
                }
            }

            return areaPerimeter;
        }
    }
}
