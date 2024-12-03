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

            var regexTest = new Regex(@"mul[(]\d[)]");

            String regex = @"mul[(][0-9]*,[0-9]*[)]";

            var matches = Regex.Matches(inputString, regex);


            return matches.Aggregate(0, (acc, e) => acc += Int32.Parse(Regex.Match(e.ToString().Split(",")[0], @"\d+").Value) * Int32.Parse(Regex.Match(e.ToString().Split(",")[1], @"\d+").Value));
        }

        public int Day3Part2Solver(string filename)
        {
            List<List<char>> input = this.parser.ParseInputNoRemovedWhitespace(filename);

            String inputString = input.Aggregate("", (acc, e) => acc + String.Join("", e) + "\n");

            var test = inputString.Count();

            var regexTest = new Regex(@"mul[(]\d[)]");

            String regex = @"(mul[(][0-9]*,[0-9]*[)]|do[(][)]|don't[(][)])";

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
                var intOne = Int32.Parse(Regex.Match(match.ToString().Split(",")[0], @"\d+").Value);

                var intTwo = Int32.Parse(Regex.Match(match.ToString().Split(",")[1], @"\d+").Value);

                prod = enableMult ? prod + intOne * intTwo : prod;
            }
            return prod;
        }
    }
}
