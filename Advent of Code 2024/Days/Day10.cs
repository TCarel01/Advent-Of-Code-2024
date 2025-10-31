using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day10
    {

        private AdventOfCode2024Parser dayTenParser;

        public Day10()
        {
            dayTenParser = new AdventOfCode2024Parser();
        }

        public int Day10Part1Solver(string filename)
        {
            List<List<int>> input = dayTenParser.ParseInputAs2DArrayOfInts(filename);

            List<List<int>> trailHeadPairs = new();

            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input[0].Count; ++j)
                {
                    if (input[i][j] == 0)
                    {
                        trailHeadPairs.Add([j, i]);
                    }
                }
            }

            return trailHeadPairs.Aggregate(0, (acc, e) => acc + TraverseTrail(input, e[0], e[1], new HashSet<(int, int)>(), true));
        }

        public int Day10Part2Solver(string filename)
        {
            List<List<int>> input = dayTenParser.ParseInputAs2DArrayOfInts(filename);

            List<List<int>> trailHeadPairs = new();

            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input[0].Count; ++j)
                {
                    if (input[i][j] == 0)
                    {
                        trailHeadPairs.Add([j, i]);
                    }
                }
            }

            return trailHeadPairs.Aggregate(0, (acc, e) => acc + TraverseTrail(input, e[0], e[1], new HashSet<(int, int)>(), false));
        }

        public int TraverseTrail(List<List<int>> trails, int xPos, int yPos, HashSet<(int, int)> set, bool unique)
        {
            int curTrailElevation = trails[yPos][xPos];

            if (curTrailElevation == 9)
            {
                if (!set.Contains((xPos, yPos))) {
                    set.Add((xPos, yPos));
                    return 1;
                }

                if (!unique) {
                    return 1;
                }
                return 0;
            }

            int minStartXPos = Math.Max(0, xPos - 1);
            int maxStartXPos = Math.Min(trails[0].Count - 1, xPos + 1);

            int minStartYPos = Math.Max(0, yPos - 1);
            int maxStartYPos = Math.Min(trails.Count - 1, yPos + 1);

            int trailScore = 0;

            if (curTrailElevation == 0)
            {
                int breakpoint = 2;
            }

            for (int y = minStartYPos; y <= maxStartYPos; ++y)
            {
                for (int x = minStartXPos; x <= maxStartXPos; ++x)
                {
                    if (x == xPos && y == yPos || x != xPos && y != yPos)
                    {
                        continue;
                    }

                    int curTopology = trails[y][x];
                    if (curTopology == curTrailElevation + 1)
                    {
                        trailScore += TraverseTrail(trails, x, y, set, unique);
                    }
                }
            }
            return trailScore;
        }
    }
}
