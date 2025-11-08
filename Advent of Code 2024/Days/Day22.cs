using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day22
    {
        AdventOfCode2024Parser dayTwentyTwoParser;

        public Day22()
        {
            dayTwentyTwoParser = new AdventOfCode2024Parser();
        }

        public long AdvanceRNG(long RNG)
        {
            long tempRNG = RNG * 64;

            RNG = Mix(RNG, tempRNG);

            RNG = Prune(RNG);

            long tempResult = (int)Math.Floor(RNG * 1.0 / 32);

            RNG = Mix(RNG, tempResult);

            RNG = Prune(RNG);

            long tempResult3 = RNG * 2048;

            RNG = Mix(RNG, tempResult3);

            RNG = Prune(RNG);

            return RNG;
        }

        public long Mix(long value, long mixNum)
        {
            return value ^ mixNum;
        }

        public long Prune(long value)
        {
            return value % 16777216;
        }

        public long Day22Part1Solver(string filename)
        {
            List<long> input = dayTwentyTwoParser.ParseInputAsInts(filename).Select(e => (long)e[0]).ToList();

            for (int i = 0; i < 2000; ++i)
            {
                input = input.Select(e => AdvanceRNG(e)).ToList();
            }
            return input.Sum();
        }

        public long Day22Part2Solver(string filename)
        {
            List<long> input = dayTwentyTwoParser.ParseInputAsInts(filename).Select(e => (long)e[0]).ToList();

            List<long> inputCopy = input.Select(e => AdvanceRNG(e)).ToList();

            List<List<long>> SingleDigits = input.Select(e => new List<long>
            {
                ComputeSinglesDigitInt(e)
            }).ToList();

            List<List<long>> differences = input.Select(e => new List<long>
            {
                ComputeSinglesDigitInt(e)
            }).ToList();

            for (int i = 0; i < 1999; ++i)
            {
                for (int j = 0; j < inputCopy.Count; ++j)
                {
                    SingleDigits[j].Add(ComputeSinglesDigitInt(inputCopy[j]));

                    differences[j].Add(ComputeSinglesDigitInt(inputCopy[j]) - ComputeSinglesDigitInt(input[j]));
                }
                input = input.Select(e => AdvanceRNG(e)).ToList();
                inputCopy = inputCopy.Select(e => AdvanceRNG(e)).ToList();
            }

            Dictionary<(long, long, long, long), long> CumulativeBananaCount = new();

            HashSet<(int, long, long, long, long)> added = new();

            for (int i = 0; i < differences.Count; ++i)
            {
                for (int j = 3; j < differences[0].Count; ++j)
                {
                    long difference0 = differences[i][j - 3];
                    long difference1 = differences[i][j - 2];
                    long difference2 = differences[i][j - 1];
                    long difference3 = differences[i][j];

                    if (!CumulativeBananaCount.ContainsKey((difference0, difference1, difference2, difference3)))
                    {
                        CumulativeBananaCount.Add((difference0, difference1, difference2, difference3), 0);
                    }

                    if (!added.Contains((i, difference0, difference1, difference2, difference3)))
                    {
                        CumulativeBananaCount[(difference0, difference1, difference2, difference3)] += SingleDigits[i][j];
                        added.Add((i, difference0, difference1, difference2, difference3));
                    }

                }
            }

            return CumulativeBananaCount.Values.Max();
        }

        public long ComputeSinglesDigitInt(long val1)
        {
            string val1Str = val1.ToString();

            long val1SingleDigit = long.Parse(val1Str.Substring(val1Str.Length - 1));

            return val1SingleDigit;
        }
    }
}
