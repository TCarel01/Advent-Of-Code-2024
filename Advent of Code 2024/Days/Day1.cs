using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day1
    {
        private AdventOfCode2024Parser dayOneParser;

        public Day1()
        {
            dayOneParser = new AdventOfCode2024Parser();
        }

        public List<List<int>> DayOneParser(string filename)
        {
            List<List<string>> dayOneInput = dayOneParser.ParseInput(filename);

            List<int> left = new List<int>();
            List<int> right = new List<int>();

            foreach (List<string> list in dayOneInput)
            {
                left.Add(Int32.Parse(list[0]));
                right.Add(Int32.Parse(list[1]));
            }

            left.Sort((elem1, elem2) => elem1 - elem2);

            right.Sort((elem1, elem2) => elem1 - elem2);

            return new List<List<int>> { { left }, { right } };
        }

        public int DayOneSolver(string filename)
        {
            var tuple = DayOneParser(filename);

            List<int> left = tuple.ElementAt(0);

            List<int> right = tuple.ElementAt(1);

            List<int> differences = left.Select((elem, idx) => Math.Abs(elem - right[idx])).ToList();

            return differences.Sum();
        }

        public int DayOnePart2Solver(string filename)
        {
            var lists = DayOneParser(filename);

            List<int> left = lists.ElementAt(0);

            List<int> right = lists.ElementAt(1);

            return left.Aggregate(0, (acc, e) => acc + e * right.Where(f => f == e).Count());

        }
    }
}
