using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{

    public class Day6
    {
        private AdventOfCode2024Parser parser;

        public Day6()
        {
            this.parser = new AdventOfCode2024Parser();
        }

        public int Day6Part1Solver(string filename)
        {
            Dictionary<String, String> directionConversionMap = new Dictionary<string, string> { { "^", ">" }, { ">", "v" }, { "v", "<" }, { "<", "^" } };

            Dictionary<string, List<int>> directionChanges = new Dictionary<string, List<int>> { { "^", new List<int> { -1, 0 } }, { ">", new List<int> { 0, 1 } }, { "v", new List<int> { 1, 0 } }, { "<", new List<int> { 0, -1 } } };

            String curDirSymbol = "^";

            List<int> curDir = directionChanges["^"];

            var input = this.parser.ParseInputAsArrayOfStrings(filename);

            var guardPath = input.ToList();

            List<int> startIdx = input.Aggregate(new List<int>(), (acc, e) => e.IndexOf("^") >= 0 ? new List<int> { input.IndexOf(e), e.IndexOf("^") } : acc);

            int guardPathCount = 0;

            int steps = 0;


            while (true)
            {
                guardPathCount = guardPath[startIdx[0]][startIdx[1]] != "X" ? guardPathCount + 1 : guardPathCount;

                guardPath[startIdx[0]][startIdx[1]] = "X";

                var nextIdx = startIdx.Select((e, idx) => e + curDir[idx]).ToList();

                if (nextIdx[0] < 0 || nextIdx[0] >= input.Count() || nextIdx[1] < 0 || nextIdx[1] >= input.Count())
                {
                    break;
                }

                while (input[nextIdx[0]][nextIdx[1]] == "#")
                {
                    curDirSymbol = directionConversionMap[curDirSymbol];
                    curDir = directionChanges[curDirSymbol];
                    nextIdx = startIdx.Select((e, idx) => e + curDir[idx]).ToList();
                }

                startIdx = nextIdx;

                steps += 1;

                if (steps > input.Count() * input[0].Count())
                {
                    return -1;
                }
            }

            return guardPathCount;
        }

        public int Day6Part2Solver(string filename)
        {
            Dictionary<String, String> directionConversionMap = new Dictionary<string, string> { { "^", ">" }, { ">", "v" }, { "v", "<" }, { "<", "^" } };

            Dictionary<string, List<int>> directionChanges = new Dictionary<string, List<int>> { { "^", new List<int> { -1, 0 } }, { ">", new List<int> { 0, 1 } }, { "v", new List<int> { 1, 0 } }, { "<", new List<int> { 0, -1 } } };

            List<List<int>> allSpotIdx = new List<List<int>>();

            String curDirSymbol = "^";

            List<int> curDir = directionChanges["^"];

            var input = this.parser.ParseInputAsArrayOfStrings(filename);

            var guardPath = input.ToList();

            List<int> startIdx = input.Aggregate(new List<int>(), (acc, e) => e.IndexOf("^") >= 0 ? new List<int> { input.IndexOf(e), e.IndexOf("^") } : acc);

            int guardPathCount = 0;

            List<Tuple<int, int>> foundIndices = new List<Tuple<int, int>>();


            while (true)
            {
                guardPathCount = guardPath[startIdx[0]][startIdx[1]] != "X" ? guardPathCount + 1 : guardPathCount;

                guardPath[startIdx[0]][startIdx[1]] = "X";

                allSpotIdx.Add(startIdx.ToList());

                var nextIdx = startIdx.Select((e, idx) => e + curDir[idx]).ToList();

                if (nextIdx[0] < 0 || nextIdx[0] >= input.Count() || nextIdx[1] < 0 || nextIdx[1] >= input.Count())
                {
                    break;
                }

                while (input[nextIdx[0]][nextIdx[1]] == "#")
                {
                    curDirSymbol = directionConversionMap[curDirSymbol];
                    curDir = directionChanges[curDirSymbol];
                    nextIdx = startIdx.Select((e, idx) => e + curDir[idx]).ToList();
                }

                startIdx = nextIdx;
            }

            int totalCount = 0;

            for (int i = 1; i < allSpotIdx.Count(); ++i)
            {
                var curInputCopy = this.parser.ParseInputAsArrayOfStrings(filename);

                if (foundIndices.Contains(new Tuple<int, int>(allSpotIdx[i][0], allSpotIdx[i][1])))
                {
                    continue;
                }

                curInputCopy[allSpotIdx[i][0]][allSpotIdx[i][1]] = "#";

                int steps = Day6Part1SolverHelper(curInputCopy);

                foundIndices.Add(new Tuple<int, int>(allSpotIdx[i][0], allSpotIdx[i][1]));

                if (steps == -1)
                {
                    totalCount += 1;
                }
            }

            return totalCount;
        }



        public int Day6Part1SolverHelper(List<List<string>> input)
        {
            Dictionary<String, String> directionConversionMap = new Dictionary<string, string> { { "^", ">" }, { ">", "v" }, { "v", "<" }, { "<", "^" } };

            Dictionary<string, List<int>> directionChanges = new Dictionary<string, List<int>> { { "^", new List<int> { -1, 0 } }, { ">", new List<int> { 0, 1 } }, { "v", new List<int> { 1, 0 } }, { "<", new List<int> { 0, -1 } } };

            String curDirSymbol = "^";

            List<int> curDir = directionChanges["^"];

            var guardPath = input.ToList();

            List<int> startIdx = input.Aggregate(new List<int>(), (acc, e) => e.IndexOf("^") >= 0 ? new List<int> { input.IndexOf(e), e.IndexOf("^") } : acc);

            if (startIdx.Count() == 0)
            {
                return 0;
            }

            int guardPathCount = 0;

            int steps = 0;


            while (true)
            {
                guardPathCount = guardPath[startIdx[0]][startIdx[1]] != "X" ? guardPathCount + 1 : guardPathCount;

                guardPath[startIdx[0]][startIdx[1]] = "X";

                var nextIdx = startIdx.Select((e, idx) => e + curDir[idx]).ToList();

                if (nextIdx[0] < 0 || nextIdx[0] >= input.Count() || nextIdx[1] < 0 || nextIdx[1] >= input.Count())
                {
                    break;
                }

                while (input[nextIdx[0]][nextIdx[1]] == "#")
                {
                    curDirSymbol = directionConversionMap[curDirSymbol];
                    curDir = directionChanges[curDirSymbol];
                    nextIdx = startIdx.Select((e, idx) => e + curDir[idx]).ToList();

                    if (nextIdx[0] < 0 || nextIdx[0] >= input.Count() || nextIdx[1] < 0 || nextIdx[1] >= input.Count())
                    {
                        break;
                    }
                }

                startIdx = nextIdx;

                steps += 1;

                if (steps > input.Count() * input[0].Count())
                {
                    return -1;
                }
            }

            return guardPathCount;
        }

    }
}
