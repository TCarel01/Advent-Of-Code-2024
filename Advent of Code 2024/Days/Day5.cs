using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day5
    {
        AdventOfCode2024Parser parser;

        public Day5()
        {
            this.parser = new AdventOfCode2024Parser();
        }

        public List<List<List<int>>> Day5Parser(string filename)
        {
            var input = this.parser.ParseInputAsArrayOfStrings(filename);

            List<List<int>> returnList = new List<List<int>>();

            List<List<int>> dependencies = new List<List<int>>();

            List<List<int>> pages = new List<List<int>>();

            foreach (var item in input)
            {
                if (item.Contains("|"))
                {
                    int num1 = Int32.Parse(String.Join("", item.Take(item.IndexOf("|")).ToArray()));
                    int num2 = Int32.Parse(String.Join("", item.Skip(item.IndexOf("|") + 1).ToArray()));
                    dependencies.Add(new List<int> { num1, num2 });
                }
                else if (item.Count() != 0)
                {
                    string curLine = String.Join("", item.ToArray());

                    var curLineInts = new List<int>();

                    var splitCurLine = curLine.Split(",");

                    foreach (var integer in splitCurLine) 
                    {
                        curLineInts.Add(Int32.Parse(integer));
                    }
                    pages.Add(curLineInts);
                }
            }

            return new List<List<List<int>>> { dependencies, pages };
        }

        public int Day5Part1Solver(string filename)
        {
            var input = Day5Parser(filename);

            var dependences = input[0];

            var pages = input[1];

            int totalSum = 0;

            Dictionary<int, HashSet<int>> dependencyDict = new Dictionary<int, HashSet<int>>();

            foreach (var dependency in dependences)
            {
                if (!dependencyDict.ContainsKey(dependency[0]))
                {
                    HashSet<int> curDependencySet = new HashSet<int>();

                    dependencyDict.Add(dependency[0], curDependencySet);
                }

                var curDependencySets = dependencyDict[dependency[0]];

                curDependencySets.Add(dependency[1]);

            }

            foreach (var pageSequence in pages)
            {
                bool addSequence = true;
                for(int i = 0; i < pageSequence.Count(); ++i)
                {
                    int curPage = pageSequence[i];
                    for (int j = i; j < pageSequence.Count(); ++j)
                    {
                        int testPage = pageSequence[j];
                        if (dependencyDict.ContainsKey(testPage) && dependencyDict[testPage].Contains(curPage))
                        {
                            addSequence = false;
                        }
                    }
                }

                totalSum = addSequence ? totalSum + pageSequence[(int)Math.Floor((double)pageSequence.Count() / 2)] : totalSum;
            }

            return totalSum;
        }

        public int Day5Part2Solver(string filename)
        {
            var input = Day5Parser(filename);

            var dependences = input[0];

            var pages = input[1];

            int totalSum = 0;

            Dictionary<int, HashSet<int>> dependencyDict = new Dictionary<int, HashSet<int>>();

            foreach (var dependency in dependences)
            {
                if (!dependencyDict.ContainsKey(dependency[0]))
                {
                    HashSet<int> curDependencySet = new HashSet<int>();

                    dependencyDict.Add(dependency[0], curDependencySet);
                }

                var curDependencySets = dependencyDict[dependency[0]];

                curDependencySets.Add(dependency[1]);

            }

            for (int pageIdx = 0; pageIdx < pages.Count(); ++pageIdx)
            {
                var pageSequence = pages[pageIdx];
                bool addSequence = false;
                for (int i = 0; i < pageSequence.Count(); ++i)
                {
                    int curPage = pageSequence[i];
                    for (int j = i + 1; j < pageSequence.Count(); ++j)
                    {
                        int testPage = pageSequence[j];
                        if (dependencyDict.ContainsKey(testPage) && dependencyDict[testPage].Contains(curPage))
                        {
                            addSequence = true;
                            var tempVal = pageSequence[i];

                            pageSequence[i] = pageSequence[j];
                            pageSequence[j] = tempVal;

                            i = 0;
                            j = 0;

                            curPage = pageSequence[i];
                        }
                    }
                }

                totalSum = addSequence ? totalSum + pageSequence[(int)Math.Floor((double)pageSequence.Count() / 2)] : totalSum;
            }

            return totalSum;
        }
    }
}
