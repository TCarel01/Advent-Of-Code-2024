using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day13
    {

        private AdventOfCode2024Parser dayThirteenParser;

        public Day13()
        {
            dayThirteenParser = new AdventOfCode2024Parser();
        }

        public List<List<long>> ParseInput(string filename)
        {
            List<string> rawInput = dayThirteenParser.ParseInputAsSingleArrayOfStrings(filename);

            List<List<long>> input = new List<List<long>>();

            for (int i = 0; i < rawInput.Count; ++i)
            {
                string curLine = rawInput[i];
                if (i % 4 == 0 || i % 4 == 1)
                {
                    int firstPlusIdx = curLine.IndexOf('+');
                    int commaIdx = curLine.IndexOf(",");
                    int secondPlusIdx = curLine.IndexOf("+", firstPlusIdx + 1);

                    long offsetOne = long.Parse(curLine.Substring(firstPlusIdx + 1, commaIdx - firstPlusIdx - 1));
                    long offsetTwo = long.Parse(curLine.Substring(secondPlusIdx + 1));
                    input.Add([offsetOne, offsetTwo]);
                }
                else if (i % 4 == 2)
                {
                    int firstEqualIdx = curLine.IndexOf('=');
                    int firstCommaIdx = curLine.IndexOf(',');
                    int secondEqualIdx = curLine.IndexOf('=', firstCommaIdx + 1);

                    long X = long.Parse(curLine.Substring(firstEqualIdx + 1, firstCommaIdx - firstEqualIdx - 1));
                    long Y = long.Parse(curLine.Substring(secondEqualIdx + 1));

                    input.Add([X, Y]);
                }

            }
            return input;
        }

        public long Day13Part1Solver(string filename)
        {
            List<List<long>> input = ParseInput(filename);

            return ComputeCostOptimized(input);
        }

        public long Day13Part2Solver(string filename)
        {
            List<List<long>> input = ParseInput(filename);
            for (int index = 0; index < input.Count; index += 3)
            {
                input[index + 2][0] += 10000000000000;
                input[index + 2][1] += 10000000000000;
            }

            return ComputeCostOptimized(input);
        }

        public long ComputeCostOptimized(List<List<long>> input)
        {
            long totalCost = 0;
            for (int curInputIdx = 0; curInputIdx < input.Count; curInputIdx += 3)
            {
                long curPrizeCost = long.MaxValue;

                long totalX = input[curInputIdx + 2][0];
                long totalY = input[curInputIdx + 2][1];

                long AButtonOffsetX = input[curInputIdx][0];
                long AButtonOffsetY = input[curInputIdx][1];

                long totalAPress = (int)Math.Min(Math.Ceiling(totalX / (AButtonOffsetX * 1.0)),
                    Math.Ceiling(totalY / (AButtonOffsetY * 1.0)));

                long BButtonOffsetX = input[curInputIdx + 1][0];
                long BButtonOffsetY = input[curInputIdx + 1][1];

                decimal proportion = (decimal)AButtonOffsetX / (decimal)AButtonOffsetY;

                decimal BButtonOffsetXDouble = (decimal)BButtonOffsetX - BButtonOffsetY * proportion;

                decimal totalXDouble = totalX - proportion * totalY;

                long numBPresses = (long)Math.Round(totalXDouble / BButtonOffsetXDouble);

                long numAPresses = (totalX - BButtonOffsetX * numBPresses) / AButtonOffsetX;

                if (numAPresses * AButtonOffsetX + numBPresses * BButtonOffsetX == totalX &&
                    numAPresses * AButtonOffsetY + numBPresses * BButtonOffsetY == totalY)
                {
                    totalCost += numAPresses * 3 + numBPresses * 1;
                }
            }

            return totalCost;
        }
    }
}
