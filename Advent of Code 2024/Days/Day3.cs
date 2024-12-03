using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day3
    {

        AdventOfCode2024Parser parser;
        public Day3()
        {
            this.parser = new AdventOfCode2024Parser();
        }

        public int Day3Part1Solver(string filename)
        {
            List<List<char>> input = this.parser.ParseInputNoRemovedWhitespace(filename);

            String inputString = input.Aggregate("", (acc, e) => acc + String.Join("", e) + "\n");

            var test = inputString.Count();

            String regex = @"mul[(]\d{1,3},\d{1,3}[)]";

            var matches = Regex.Matches(inputString, regex);


            return matches.Aggregate(0, (acc, e) => acc += Regex.Matches(e.ToString(), @"\d{1,3}").ToList().Aggregate(1, (acc2, e2) => acc2 * Int32.Parse(e2.ToString())));
        }

        public int Day3Part2Solver(string filename)
        {
            List<List<char>> input = this.parser.ParseInputNoRemovedWhitespace(filename);

            String inputString = input.Aggregate("", (acc, e) => acc + String.Join("", e) + "\n");

            var test = inputString.Count();

            String regex = @"(mul[(]\d{1,3},\d{1,3}[)]|do[(][)]|don't[(][)])";

            var matches = Regex.Matches(inputString, regex);

            int prod = 0;

            bool enableMult = true;

            foreach (var match in matches)
            {
                if (match.ToString() == "do()")
                {
                    enableMult = true;
                    continue;
                }
                else if (match.ToString() == "don't()")
                {
                    enableMult = false;
                    continue;
                }

                var ints = Regex.Matches(match.ToString(), @"\d{1,3}");
                prod = enableMult? prod + ints.Aggregate(1, (acc, e) => acc * Int32.Parse(e.ToString())) : prod;
            }
            return prod;
        }
    }
}
