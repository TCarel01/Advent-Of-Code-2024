using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day8
    {
        AdventOfCode2024Parser parser;

        public Day8()
        {
            this.parser = new AdventOfCode2024Parser();
        }


        public Dictionary<string, List<Tuple<int, int>>> GetInputAsDictionaryOfCoordsToTuples(List<List<string>> input)
        {
            var uniqueCharsAndCoords = new Dictionary<string, List<Tuple<int, int>>>();

            int testParserCount = 0;

            for (int line = 0; line < input.Count(); ++line)
            {
                var curLine = input[line];
                for (int character = 0; character < curLine.Count(); ++character)
                {
                    var curCharacter = curLine[character];
                    if (curCharacter != ".")
                    {
                        testParserCount += 1;
                        if (!uniqueCharsAndCoords.ContainsKey(curCharacter))
                        {
                            uniqueCharsAndCoords.Add(curCharacter, new List<Tuple<int, int>>());
                        }
                        uniqueCharsAndCoords[curCharacter].Add(new Tuple<int, int>(line, character));
                    }
                }
            }
            return uniqueCharsAndCoords;
        }

        public int Day8Part1Solver(string filename)
        {
            var input = this.parser.ParseInputAsArrayOfStrings(filename);

            var inputCopy = input.ToList();

            var uniqueCharsAndCoords = GetInputAsDictionaryOfCoordsToTuples(input);

            var signalCoords = new HashSet<Tuple<(int, int)>>();

            int testCount = 0;
            foreach (var pair in uniqueCharsAndCoords)
            {
                var curChar = pair.Key;
                var curCoords = pair.Value;

                for (int i = 0; i < curCoords.Count(); ++i)
                {
                    var firstCoords = curCoords[i];
                    for (int j = 0; j < curCoords.Count(); ++j)
                    {
                        var secondCoords = curCoords[j];
                        if (i == j)
                        {
                            continue;
                        }
                        int xdifference = secondCoords.Item2 - firstCoords.Item2;

                        int ydifference = secondCoords.Item1 - firstCoords.Item1;

                        var antiNodeCoordsX = secondCoords.Item2 + xdifference;

                        var antiNodeCoordsY = secondCoords.Item1 + ydifference;

                        if (0 <= antiNodeCoordsX && antiNodeCoordsX < input.Count() && 0 <= antiNodeCoordsY && antiNodeCoordsY < input[0].Count())
                        {
                            Tuple<(int, int)> coordsToAdd = new Tuple<(int, int)>((antiNodeCoordsY, antiNodeCoordsX));
                            signalCoords.Add(coordsToAdd);
                        }
                    }
                }
            }

            return signalCoords.Count();
        }

        public int Day8Part2Solver(string filename)
        {
            var input = this.parser.ParseInputAsArrayOfStrings(filename);

            var inputCopy = input.ToList();

            var uniqueCharsAndCoords = GetInputAsDictionaryOfCoordsToTuples(input);

            var signalCoords = new HashSet<Tuple<int, int>>();

            int testCount = 0;
            foreach (var pair in uniqueCharsAndCoords)
            {
                var curChar = pair.Key;
                var curCoords = pair.Value;

                for (int i = 0; i < curCoords.Count(); ++i)
                {
                    var firstCoords = curCoords[i];
                    for (int j = 0; j < curCoords.Count(); ++j)
                    {
                        var secondCoords = curCoords[j];
                        if (i == j)
                        {
                            continue;
                        }
                        int xdifference = secondCoords.Item2 - firstCoords.Item2;

                        int ydifference = secondCoords.Item1 - firstCoords.Item1;

                        var antiNodeCoordsX = firstCoords.Item2;

                        var antiNodeCoordsY = firstCoords.Item1;

                        while (0 <= antiNodeCoordsX && antiNodeCoordsX < input.Count() && 0 <= antiNodeCoordsY && antiNodeCoordsY < input[0].Count())
                        {
                            Tuple<int, int> coordsToAdd = new Tuple<int, int>(antiNodeCoordsY, antiNodeCoordsX);
                            signalCoords.Add(coordsToAdd);

                            antiNodeCoordsX += xdifference;
                            antiNodeCoordsY += ydifference;
                        }
                    }
                }
            }

            return signalCoords.Count();
        }


    }
}
