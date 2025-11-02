using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day14
    {

        private AdventOfCode2024Parser dayFourteenParser;

        public Day14()
        {
            dayFourteenParser = new AdventOfCode2024Parser();
        }

        public List<List<int>> ParseInput(string filename)
        {
            List<string> rawInput = dayFourteenParser.ParseInputAsSingleArrayOfStrings(filename);

            List<List<int>> input = new List<List<int>>();

            for (int i = 0; i < rawInput.Count; ++i)
            {
                List<int> curInputLine = new List<int>();

                string curLine = rawInput[i];

                List<string> splitLine = curLine.Split(" ").ToList();

                string coords = splitLine[0].Substring(splitLine[0].IndexOf("=") + 1);

                string movement = splitLine[1].Substring(splitLine[1].IndexOf("=") + 1);

                List<int> integerCoords = coords.Split(",").Select(e => int.Parse(e)).ToList();

                List<int> integerMovement = movement.Split(",").Select(e => int.Parse(e)).ToList();

                input.Add(integerCoords.Concat(integerMovement).ToList());
            }

            return input;
        }

        public int Day14Part1Solver(string filename, int width, int height)
        {
            List<List<int>> input = ParseInput(filename);

            for (int i = 0; i < 10000; ++i)
            {
                for (int robotIdx = 0; robotIdx < input.Count; ++robotIdx)
                {
                    input[robotIdx][0] = (input[robotIdx][0] + width + input[robotIdx][2]) % width;
                    input[robotIdx][1] = (input[robotIdx][1] + height + input[robotIdx][3]) % height;
                }
                if (i > 2068)
                {
                    int midCount = 0;
                    for (int j = 0; j < input.Count; ++j)
                    {
                        List<int> curRobot = input[j];
                        if (width / 2 - (width / 4) < curRobot[0] && curRobot[0] < width / 2 + (width / 4))
                        {
                            midCount += 1;
                        }
                    }

                    if (midCount > .7 * input.Count)
                    {
                        PrintLayout(input, width, height, i + 1);
                    }

                }
                
                
            }

            int midwidth = (int)Math.Floor(width * 1.0 / 2);
            int midheight = (int)Math.Floor(height * 1.0 / 2);

            int firstQuadrantCount = 0;
            int secondQuadrantCount = 0;
            int thirdQuadrantCount = 0;
            int fourthQuadrentCount = 0;

            for (int i = 0; i < input.Count; ++i)
            {
                List<int> curRobot = input[i];

                if (curRobot[0] == midwidth || curRobot[1] == midheight)
                {
                    continue;
                }
                else if (curRobot[0] < midwidth && curRobot[1] < midheight)
                {
                    firstQuadrantCount++;
                }
                else if (curRobot[0] >= midwidth && curRobot[1] < midheight)
                {
                    secondQuadrantCount++;
                }
                else if (curRobot[0] < midwidth && curRobot[1] >= midheight)
                {
                    thirdQuadrantCount++;
                }
                else
                {
                    fourthQuadrentCount++;
                }
            }

            return firstQuadrantCount * secondQuadrantCount * thirdQuadrantCount * fourthQuadrentCount;
        }
        public void PrintLayout(List<List<int>> input, int width, int height, int curStep)
        {
            List<List<string>> inputVisualized = new List<List<string>>();
            for (int i = 0; i < height; ++i)
            {
                List<string> curRow = new List<string>();
                for (int j = 0; j < width; ++j)
                {
                    curRow.Add(".");
                }
                inputVisualized.Add(curRow);
            }

            for (int i = 0; i < input.Count; ++i)
            {
                List<int> curRobot = input[i];
                inputVisualized[curRobot[1]][curRobot[0]] = "1";
            }

            Console.WriteLine("STEP " + curStep);

            for (int i = 0; i < inputVisualized.Count; ++i)
            {
                Console.WriteLine(String.Join("", inputVisualized[i]));
            }
        }
    }
}
