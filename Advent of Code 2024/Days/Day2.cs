using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day2
    {
        AdventOfCode2024Parser parser;
        public Day2()
        {
            this.parser = new AdventOfCode2024Parser();
        }

        public List<List<int>> Day2Parser(string filename)
        {
            List<List<string>> day2Input = this.parser.ParseInput(filename);

            List<List<int>> input = new List<List<int>>();

            foreach (var curReport in day2Input)
            {
                List<int> curLine = new List<int>();
                foreach (var item in curReport)
                {
                    curLine.Add(Int32.Parse(item));
                }
                input.Add(curLine);
            }

            return input;
        }

        public int Day2Part1Solver(string filename)
        {
            List<List<int>> input = Day2Parser(filename);

            int safeCount = input.Where(e => IsCurReportValid(e)).Count();

            return safeCount;
        }

        public int Day2Part2Solver(string filename)
        {
            List<List<int>> input = Day2Parser(filename);

            int safeCount = 0;

            foreach (var curReport in input)
            {
                bool curIterationValid = false;
                if (IsCurReportValid(curReport))
                {
                    ++safeCount;
                    continue;
                }
                for (int i = 0; i < curReport.Count(); ++i)
                {
                    var curCopy = curReport.ToList();

                    curCopy.RemoveAt(i);

                    if (IsCurReportValid(curCopy))
                    {
                        curIterationValid = true;
                        break;
                    }
                }
                safeCount = curIterationValid ? safeCount + 1 : safeCount;
            }

            return safeCount;
        }

        public bool IsCurReportValid(List<int> report)
        {
            if (report.Count() == 0 || report.Count() == 1)
            {
                return true;
            }

            if (report[0] == report[1])
            {
                return false;
            }
            bool isIncreasing = report[0] < report[1];

            return report.Where((e, idx) => idx == 0 ? true : isIncreasing ? e > report[idx - 1] && e - report[idx - 1] <= 3 : e < report[idx - 1] && report[idx - 1] - e <= 3).Count() == report.Count();
        }
    }
}
