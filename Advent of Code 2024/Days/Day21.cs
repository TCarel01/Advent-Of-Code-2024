using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day21
    {

        AdventOfCode2024Parser dayTwentyOneParser;

        Dictionary<string, List<string>> KeypadGraph;

        Dictionary<(string, string), string> NumbersToDirectionsVar;

        Dictionary<string, List<string>> DirectionGraph;

        Dictionary<(string, string), List<string>> OptimalDirectionPaths;

        Dictionary<(string, string), string> ArrowCombosToDirection;

        public Day21() 
        { 
            dayTwentyOneParser = new AdventOfCode2024Parser();

            KeypadGraph = new Dictionary<string, List<string>>()
            {
                { "0", ["A", "2"] },
                { "1", ["2", "4"] },
                { "2", ["1", "0", "3", "5"] },
                { "3", ["2", "A", "6"] },
                { "4", ["1", "5", "7"] },
                { "5", ["4", "2", "6", "8"] },
                { "6", ["5", "3", "9"]},
                { "7", ["4", "8"] },
                { "8", ["7", "5", "9"] },
                { "9", ["8", "6"] },
                { "A", ["0", "3"] }
            };


            NumbersToDirectionsVar = new Dictionary<(string, string), string>
            {
                { ("0", "2"), "^" },
                { ("0", "A"), ">"},
                { ("1", "4"), "^" },
                { ("1", "2"), ">"},
                { ("2", "0"), "v" },
                { ("2", "1"), "<" },
                { ("2", "3"), ">" },
                { ("2", "5"), "^"},
                { ("3", "A"), "v" },
                { ("3", "2"), "<"},
                { ("3", "6"), "^"},
                { ("4", "1"), "v"},
                { ("4", "5"), ">"},
                { ("4", "7"), "^"},
                { ("5", "2"), "v" },
                { ("5", "4"), "<" },
                { ("5", "6"), ">"},
                { ("5", "8"),  "^"},
                { ("6", "3"), "v"},
                { ("6", "5"), "<"},
                { ("6", "9"), "^" },
                { ("7", "4"), "v" },
                { ("7", "8"),  ">"},
                { ("8", "5"), "v" },
                { ("8", "7"), "<" },
                { ("8", "9"), ">" },
                { ("9", "6"), "v" },
                { ("9", "8"), "<" },
                { ("A", "0"), "<" },
                { ("A", "3"), "^" }
            };

            DirectionGraph = new Dictionary<string, List<string>>()
            {
                { "A", ["^", ">"] },
                { "^", ["v", "A"]},
                { ">", ["v", "A"] },
                { "v", ["<", ">", "^"] },
                { "<", ["v"] },

            };

            OptimalDirectionPaths = new Dictionary<(string, string), List<string>>
            {
                { ("A", "^"), ["A", "^"] },
                { ("A", ">"), ["A", ">"] },
                { ("A", "v"), ["A", "^", "v"] },
                { ("A", "<"), ["A", ">", "v", "<"] },
                { ("A", "A"), ["A"] },

                { ("<", "v"), ["<", "v"] },
                { ("<", ">"), ["<", "v", ">"] },
                { ("<", "^"), ["<", "v", "^"] },
                { ("<", "A"), ["<", "v", ">", "A"] },
                { ("<", "<"), ["<"] },

                { ("v", "<"), ["v", "<"] },
                { ("v", "^"), ["v", "^"] },
                { ("v", ">"), ["v", ">"] },
                { ("v", "A"), ["v", "^", "A"] },
                { ("v", "v"), ["v"] },

                { (">", "<"), [">", "v", "<"] },
                { (">", "v"), [">", "v"] },
                { (">", "^"), [">", "v", "^"] },
                { (">", "A"), [">", "A"] },
                { (">", ">"), [">"] },

                { ("^", "A"), ["^", "A"] },
                { ("^", "v"), ["^", "v"] },
                { ("^", ">"), ["^", "v", ">"] },
                { ("^", "<"), ["^", "v", "<"] },
                { ("^", "^"), ["^"] }
            };

            ArrowCombosToDirection = new Dictionary<(string, string), string>
            {
                { ("A", "^"), "<" },
                { ("A", ">"), "v" },
                { ("^", "A"), ">" },
                { ("^", "v"), "v" },
                { ("<", "v"), ">" },
                { ("v", "<"), "<" },
                { ("v", "^"), "^" },
                { ("v", ">"), ">" },
                { (">", "v"), "<" },
                { (">", "A"), "^" }
            };
        }

        public Dictionary<string, List<string>> GetKeypadGraph()
        {
            return KeypadGraph;
        }

        public Dictionary<(string, string), string> NumbersToDirections()
        {
            return NumbersToDirectionsVar;
        }

        public Dictionary<string, List<string>> GetDirectionalGraph()
        {
            return DirectionGraph;
        }

        public Dictionary<(string, string), List<string>> GetOptimalDirectionPaths()
        {
            return OptimalDirectionPaths;
        }

        public Dictionary<(string, string), string> MapArrowCombosToDirection()
        {
            return ArrowCombosToDirection;
        }

        public Dictionary<(string, string), List<string>> FindShortestPathBetweenAllCombos(Dictionary<string, List<string>> graph)
        {
            List<string> keys = graph.Keys.ToList();

            Dictionary<(string, string), int> distances = new();

            Dictionary<(string, string), List<string>> paths = new();

            for (int i = 0; i < keys.Count; ++i)
            {
                string key = keys[i];

                List<string> curNodes = new();
                List<string> nextNodes = new();

                Dictionary<string, List<string>> previousPath = new();

                curNodes.Add(key);

                previousPath.Add(key, [key]);

                int curDepthCount = 0;

                while (curNodes.Count > 0)
                {
                    foreach (var node in curNodes)
                    {
                        distances.Add((key, node), curDepthCount);

                        foreach (var nextNode in graph[node])
                        {
                            if (!distances.ContainsKey((key, nextNode)) && !nextNodes.Contains(nextNode))
                            {
                                nextNodes.Add(nextNode);

                                List<string> nextNodePath = [..previousPath[node]];
                                nextNodePath.Add(nextNode);

                                previousPath.Add(nextNode, nextNodePath);
                            }
                        }
                    }
                    curNodes = nextNodes;
                    nextNodes = new();

                    curDepthCount += 1;
                }

                foreach (var curKey in previousPath.Keys)
                {
                    paths.Add((key, curKey), previousPath[curKey]);
                }
            }

            return paths;
        }

        public Dictionary<(string, string), List<List<string>>> FindAllPossibleShortestPaths(Dictionary<string, List<string>> graph)
        {
            List<string> keys = graph.Keys.ToList();

            Dictionary<(string, string), List<List<string>>> returnDict = new();

            foreach (var key1 in keys)
            {
                foreach (var key2 in keys)
                {
                    List<List<string>> allPaths = FindAllPossibleShortestPathsBetweenTwoNodes(graph, key1, key2, []);

                    int minCount = allPaths.Aggregate(int.MaxValue, (acc, e) => e.Count < acc ? e .Count: acc);

                    allPaths = allPaths.Where(e => e.Count == minCount).ToList();

                    returnDict.Add((key1, key2), allPaths);
                }
            }

            return returnDict;
        }

        public List<List<string>> FindAllPossibleShortestPathsBetweenTwoNodes(Dictionary<string, List<string>> graph, string node1, string node2, List<string> currentPath)
        {
            List<List<string>> allPaths = new();

            List<string> adjacentNodes = graph[node1];

            if (node1 == node2)
            {
                List<string> curCurrentPath = new List<string>(currentPath);
                curCurrentPath.Add(node1);
                allPaths.Add(curCurrentPath);
            }

            else
            {
                foreach (var node in adjacentNodes)
                {
                    if (!currentPath.Contains(node))
                    {
                        List<string> curCurrentPath = new List<string>(currentPath);
                        curCurrentPath.Add(node1);
                        allPaths = allPaths.Concat(FindAllPossibleShortestPathsBetweenTwoNodes(graph, node, node2, curCurrentPath)).ToList();
                    }
                }
            }
            return allPaths;
        }

        public List<string> GetArrowPathFromCode(List<string> code, Dictionary<(string, string), List<string>> allCombosShortestPath)
        {
            List<string> codeCopy = new List<string>(code);

            codeCopy = codeCopy.Prepend("A").ToList();

            List<string> curCodePath = new List<string>();

            Dictionary<(string, string), string> numbersToDirections = NumbersToDirections();

            for (int i = 0; i < codeCopy.Count - 1; ++i)
            {
                string curCodeEntry = codeCopy[i];
                string nextCodeEntry = codeCopy[i + 1];

                List<string> codePath = new List<string>(allCombosShortestPath[(curCodeEntry, nextCodeEntry)]);

                for (int j = 0; j < codePath.Count - 1; ++j) 
                {
                    string curPathEntry = codePath[j];
                    string nextPathEntry = codePath[j + 1];

                    curCodePath.Add(numbersToDirections[(curPathEntry, nextPathEntry)]);
                }

                curCodePath.Add("A");
            }

            return curCodePath;
        }

        public List<string> ConvertNumberPathToArrowPath(List<string> codePath)
        {
            List<string> arrowPath = new List<string>();

            var map = NumbersToDirections();

            for (int i = 0; i < codePath.Count - 1; ++i)
            {
                arrowPath.Add(map[(codePath[i], codePath[i + 1])]);
            }
            return arrowPath;
        }

        public List<string> ConvertArrowPathToNextLevelArrowPath(List<string> codePath)
        {
            List<string> arrowPath = new List<string>();

            var map = MapArrowCombosToDirection();

            for (int i = 0; i < codePath.Count - 1; ++i)
            {
                arrowPath.Add(map[(codePath[i], codePath[i + 1])]);
            }
            return arrowPath;
        }

        public List<List<string>> GetAllArrowPathsFromCode(List<string> code, Dictionary<(string, string), List<List<string>>> allCombosAllPaths)
        {
            List<string> codeCopy = new List<string>(code);

            codeCopy = codeCopy.Prepend("A").ToList();

            List<List<List<string>>> curStringCache = new(); 

            List<string> curCodePath = new List<string>();

            Dictionary<(string, string), string> numbersToDirections = NumbersToDirections();

            for (int i = 0; i < codeCopy.Count - 1; ++i)
            {
                string curCodeEntry = codeCopy[i];
                string nextCodeEntry = codeCopy[i + 1];

                List<List<string>> codePaths = new List<List<string>>(allCombosAllPaths[(curCodeEntry, nextCodeEntry)]);

                List<List<string>> nextEntryCodePaths = new List<List<string>>();

                if (i == 0)
                {
                    foreach (var path in codePaths)
                    {
                        List<string> arrowPath = ConvertNumberPathToArrowPath(path);
                        arrowPath.Add("A");
                        nextEntryCodePaths.Add(arrowPath);
                    }
                }
                else
                {
                    foreach (var codePath in codePaths)
                    {
                        List<List<string>> previousEntryPaths = new(curStringCache[i - 1]);

                        var curArrowCodePath = ConvertNumberPathToArrowPath(codePath);
                        curArrowCodePath.Add("A");

                        foreach (var path in previousEntryPaths)
                        {
                            List<string> newPath = path.Concat(curArrowCodePath).ToList();
                            nextEntryCodePaths.Add(newPath);
                        }
                    }
                }
                curStringCache.Add(nextEntryCodePaths);
            }
            return curStringCache[curStringCache.Count - 1];
        }

        public List<string> UndoArrowRecursionLevel(List<string> directionalPath, string pathStart)
        {
            Dictionary<(string, string), List<string>> arrowsShortestPath = GetOptimalDirectionPaths();

            List<string> directionalPathCopy = new List<string>(directionalPath);

            List<string> aggregatedArrowPath = new List<string>();

            directionalPathCopy = directionalPathCopy.Prepend(pathStart).ToList();

            var arrowCombosToDirection = MapArrowCombosToDirection();

            for (int i = 0; i < directionalPathCopy.Count - 1; ++i)
            {
                string curArrowPathEntry = directionalPathCopy[i];
                string nextArrowPathEntry = directionalPathCopy[i + 1];

                List<string> minArrowPath = new List<string>(arrowsShortestPath[(curArrowPathEntry, nextArrowPathEntry)]);

                for (int j = 0; j < minArrowPath.Count - 1; ++j)
                {
                    List<string> curArrowSequence = new List<string>();

                    string minArrowPathCurEntry = minArrowPath[j];
                    string minArrowPathNextEntry = minArrowPath[j + 1];

                    curArrowSequence.Add(arrowCombosToDirection[(minArrowPathCurEntry, minArrowPathNextEntry)]);

                    aggregatedArrowPath = aggregatedArrowPath.Concat(curArrowSequence).ToList();
                }

                aggregatedArrowPath.Add("A");

            }

            return aggregatedArrowPath;
        }

        public int Day21Part1Solver(string filename)
        {
            List<List<string>> input = dayTwentyOneParser.ParseInputAsArrayOfStrings(filename);

            Dictionary<string, List<string>> graph = GetKeypadGraph();

            Dictionary<string, List<string>> directionalGraph = GetDirectionalGraph();

            Dictionary<(string, string), List<string>> allCombosShortestPath = FindShortestPathBetweenAllCombos(graph);

            Dictionary<(string, string), List<List<string>>> keypadPaths = FindAllPossibleShortestPaths(graph);

            Dictionary<(string, string), List<List<string>>> arrowPaths = FindAllPossibleShortestPaths(directionalGraph);

            int count = 0;

            foreach (var curCode in input)
            {
                List<string> firstRecursionLevel = GetArrowPathFromCode(curCode, allCombosShortestPath);

                List<List<string>> firstRecursionLevelTest = GetAllArrowPathsFromCode(curCode, keypadPaths);

                int curCount = int.MaxValue;

                foreach (var path in firstRecursionLevelTest)
                {
                    List<string> secondRecursionLevelTest = UndoArrowRecursionLevel(path, "A");
                    List<string> thirdRecursionLevelTest = UndoArrowRecursionLevel(secondRecursionLevelTest, "A");

                    if (thirdRecursionLevelTest.Count < curCount)
                    {
                        curCount = thirdRecursionLevelTest.Count;
                    }
                }

                count += curCount * int.Parse(string.Join("", curCode.Take(3)));
            }

            return count;
        }


        public long Day21Part2Solver(string filename)
        {
            List<List<string>> input = dayTwentyOneParser.ParseInputAsArrayOfStrings(filename);

            Dictionary<string, List<string>> graph = GetKeypadGraph();

            Dictionary<string, List<string>> directionalGraph = GetDirectionalGraph();

            Dictionary<(string, string), List<string>> allCombosShortestPath = FindShortestPathBetweenAllCombos(graph);

            Dictionary<(string, string), List<List<string>>> keypadPaths = FindAllPossibleShortestPaths(graph);

            Dictionary<(string, string), List<List<string>>> arrowPaths = FindAllPossibleShortestPaths(directionalGraph);

            Dictionary<(string, string, int), long> curRecursionLevelCache = new();

            long count = 0;

            foreach (var curCode in input)
            {

                List<List<string>> firstRecursionLevelTest = GetAllArrowPathsFromCode(curCode, keypadPaths);

                long curCount = long.MaxValue;

                foreach (var path in firstRecursionLevelTest)
                {
                    curCount = Math.Min(RecursiveApproach(path, 0, 25, curRecursionLevelCache), curCount);
                }

                count += curCount * int.Parse(string.Join("", curCode.Take(3)));
            }

            return count;
        }

        public long RecursiveApproach(List<string> curArrowPath, int curIteration, int maxIterations, Dictionary<(string, string, int), long> recursionCache)
        {
            if (curIteration == maxIterations)
            {
                return curArrowPath.Count;
            }

            string curArrowPathStr = string.Join("", curArrowPath);

            List<string> splitPaths = SplitStringCustom(curArrowPathStr).Where(e => e != "").ToList();

            long totalCounter = 0;

            for (int i = 0; i < splitPaths.Count; ++i)
            {
                string segment = splitPaths[i];

                string startChar = i == 0 ? "A" : splitPaths[i - 1].ToCharArray()[splitPaths[i - 1].Length - 1].ToString();

                if (segment != "A" && recursionCache.ContainsKey((segment, startChar, curIteration)))
                {
                    totalCounter += recursionCache[(segment, startChar, curIteration)];
                }
                else
                {
                    if (curIteration == 0)
                    {
                        int test = 2;
                    }
                    List<string> splitSegment = segment.ToCharArray().Select(e => e.ToString()).ToList();
                    List<string> undoneRecursion = UndoArrowRecursionLevel(splitSegment, startChar);
                    long curTotal = RecursiveApproach(undoneRecursion, curIteration + 1, maxIterations, recursionCache);
                    totalCounter += curTotal;
                    if (segment != "A")
                    {
                        recursionCache.Add((segment, startChar, curIteration), curTotal);
                    }

                }
            }

            if (curIteration == 0)
            {
                int test = 2;
            }

            return totalCounter;
        }


        public int ComputeCurCodeLength(List<string> code, Dictionary<(string, string), List<string>> shortestPaths)
        {
            int count = 0;

            string curNode = "A";

            for (int i = 0; i < code.Count; ++i)
            {
                string nextNode = code[i];
                count += shortestPaths[(curNode, nextNode)].Count - 1;
                curNode = nextNode;
            }

            int codeIntValue = int.Parse(string.Join("", code.Take(3).ToList()));

            return count * codeIntValue;
        }
        

        public List<string> SplitStringCustom(string strToSplit)
        {
            List<string> splitStr = strToSplit.ToCharArray().Select(e => e.ToString()).ToList();

            for (int i = 1; i < splitStr.Count; ++i)
            {
                if (splitStr[i] == "A" && (splitStr[i - 1] != "," && splitStr[i - 1] != "A"))
                {
                    splitStr.Insert(i, ",");
                    splitStr.Insert(i + 2, ",");
                }
            }
            return string.Join("", splitStr).Split(",").ToList();
        }
    }
}
