using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Advent_of_Code_2024.Days
{
    public class Day7
    {

        private AdventOfCode2024Parser parser;

        public Day7()
        {
            this.parser = new AdventOfCode2024Parser();
        }


        public Int128 Day7Part1Solver(string filename)
        {
            List<string> input = this.parser.ParseInputAsSingleArrayOfStrings(filename);

            Int128 totalSum = 0;

            foreach (var item in input)
            {
                String[] targets_values = item.Split(':');

                Int128 target = Int128.Parse(targets_values[0]);

                List<Int128> values = targets_values[1].Split(" ").Where((f) => f != "").Select((e) => Int128.Parse(e)).ToList();

                totalSum += Day7Part1Helper(values, 0, target);


            }

            return totalSum;
        }


        public Int128 Day7Part2Solver(string filename)
        {
            List<string> input = this.parser.ParseInputAsSingleArrayOfStrings(filename);

            Int128 totalSum = 0;

            foreach (var item in input)
            {
                String[] targets_values = item.Split(':');

                Int128 target = Int128.Parse(targets_values[0]);

                List<Int128> values = targets_values[1].Split(" ").Where((f) => f != "").Select((e) => Int128.Parse(e)).ToList();

                totalSum += Day7Part2Helper(values, 0, target);
            }

            return totalSum;
        }

        public Int128 Day7Part1Helper(List<Int128> remainingInput, Int128 curValue, Int128 target)
        {
            if (remainingInput.Count() == 0)
            {
                return curValue;
            }

            Int128 val = remainingInput[0];

            List<Int128> newRemainingInput = remainingInput.Where((e, idx) => idx != 0).ToList();

            Int128 val1 = Day7Part1Helper(newRemainingInput, curValue * val, target);

            Int128 val2 = Day7Part1Helper(newRemainingInput, curValue + val, target);

            if (val1 == target || val2 == target)
            {
                return target;
            }
            return 0;
        }

        public Int128 Day7Part2Helper(List<Int128> remainingInput, Int128 curValue, Int128 target)
        {
            if (remainingInput.Count() == 0)
            {
                return curValue;
            }

            Int128 val = remainingInput[0];

            List<Int128> newRemainingInput = remainingInput.Where((e, idx) => idx != 0).ToList();

            Int128 val1 = Day7Part2Helper(newRemainingInput, curValue * val, target);

            Int128 val2 = Day7Part2Helper(newRemainingInput, curValue + val, target);

            Int128 val3 = Day7Part2Helper(newRemainingInput, Int128.Parse($"{curValue}{val}"), target);

            if (val1 == target || val2 == target || val3 == target)
            {
                return target;
            }
            return 0;
        }
    }
}
