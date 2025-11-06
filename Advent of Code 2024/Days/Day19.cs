using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day19
    {
        private AdventOfCode2024Parser dayNineteenParser;

        public Day19()
        {
            dayNineteenParser = new AdventOfCode2024Parser();
        }

        public int Day19Part1Solver(string filename)
        {
            List<string> input = dayNineteenParser.ParseInputAsSingleArrayOfStrings(filename);

            List<string> availableTowels = input[0].Split(", ").ToList();

            List<string> patterns = input.Skip(2).ToList();

            int count = 0;

            foreach (string pattern in patterns)
            {
                count = PatternsPossibleDP(availableTowels, pattern) != 0 ? count + 1 : count;
            }

            return count;
        }

        public long Day19Part2Solver(string filename)
        {
            List<string> input = dayNineteenParser.ParseInputAsSingleArrayOfStrings(filename);

            List<string> availableTowels = input[0].Split(", ").ToList();

            List<string> patterns = input.Skip(2).ToList();

            long count = 0;

            foreach (string pattern in patterns)
            {
                count += PatternsPossibleDP(availableTowels, pattern);
            }
            return count;
        }

        public bool PatternPossible(List<string> availableTowels, string curRemainingPattern)
        {
            if (curRemainingPattern.Length == 0)
            {
                return true;
            }

            bool foundTowel = false;
            for (int i = 0; i < availableTowels.Count; ++i)
            {
                string curTowel = availableTowels[i];

                if (curTowel.Length <= curRemainingPattern.Length)
                {
                    string firstHalf = curRemainingPattern.Substring(0, curTowel.Length);
                    string secondHalf = curRemainingPattern.Substring(curTowel.Length);

                    if (firstHalf == curTowel)
                    {
                        foundTowel = foundTowel || PatternPossible(availableTowels, secondHalf);
                    }
                }
            }
            return foundTowel;
        }

        public long PatternsPossibleDP(List<string> availableTowels, string pattern)
        {
            List<long> possibleWays = new long[pattern.Length + 1].ToList();

            possibleWays[pattern.Length] = 1;

            for (int i = pattern.Length - 1; i >= 0; --i)
            {
                string curPatternSubstring = pattern.Substring(i);

                for (int j = 0; j < availableTowels.Count; ++j)
                {
                    string curTowel = availableTowels[j];
                    if (curTowel.Length <= curPatternSubstring.Length && curTowel == curPatternSubstring.Substring(0, curTowel.Length))
                    {
                        possibleWays[i] += possibleWays[i + curTowel.Length];
                    }
                }
            }

            return possibleWays[0];
        }
    }
}
