using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day11
    {

        private AdventOfCode2024Parser dayElevenParser;

        private Dictionary<(long, long), long> cache;

        public Day11()
        {
            dayElevenParser = new AdventOfCode2024Parser();
        }

        public long Day11Part1Solver(string filename)
        {
            cache = new Dictionary<(long, long), long>();

            List<long> input = dayElevenParser.ParseInputAsInts(filename)[0].Select(e => (long)e).ToList();

            return input.Aggregate(0, (acc, e) => (int)((long)acc + GetNumStones(e, 0, 25)));
        }

        public long Day11Part2Solver(string filename)
        {
            cache = new Dictionary<(long, long), long>();

            List<long> input = dayElevenParser.ParseInputAsInts(filename)[0].Select(e => (long)e).ToList();

            long result = 0;

            for (int i = 0; i < input.Count; ++i)
            {
                result += (long)GetNumStones(input[i], 0, 75);
            }

            return result;
        }


        public long GetNumStones(long curStone, long curItter, long cap)
        {
            if (curItter >= cap)
            {
                return 1;
            }

            if (cache.ContainsKey((curStone, curItter)))
            {
                return cache[(curStone, curItter)];
            }

            List<long> stones = splitStone(curStone);

            long totalStoneCount = 0;

            foreach (long stone in stones)
            {
                totalStoneCount += GetNumStones(stone, curItter + 1, cap);
            }

            cache.Add((curStone, curItter), totalStoneCount);

            return totalStoneCount;
        }

        public List<long> splitStone(long stone)
        {
            List<long> newStones = new List<long>();

            if (stone == 0)
            {
                newStones.Add(1);
            }
            else if (stone.ToString().Length % 2 == 0)
            {
                string stoneStr = stone.ToString();
                string firstHalf = stoneStr.Substring(0, stoneStr.Length / 2);
                string secondHalf = stoneStr.Substring(stoneStr.Length / 2);

                newStones.Add(long.Parse(firstHalf));
                newStones.Add(long.Parse(secondHalf));
            }
            else
            {
                newStones.Add(stone * 2024);
            }

            return newStones;
        }
    }
}
