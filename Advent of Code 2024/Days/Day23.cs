using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day23
    {

        AdventOfCode2024Parser DayTwentyThreeParser = new AdventOfCode2024Parser();

        public Day23()
        {
            DayTwentyThreeParser = new AdventOfCode2024Parser();
        }

        public int Day23Part1Solver(string filename)
        {
            List<string> input = DayTwentyThreeParser.ParseInputAsSingleArrayOfStrings(filename);

            Dictionary<string, HashSet<string>> graph = new();

            HashSet<(string, string, string)> uniqueTriples = new();

            ConstructGraph(input, graph);

            ConstructUniqueTriplesSet(graph, uniqueTriples);

            return uniqueTriples.ToList().Where(e => e.Item1.ToCharArray()[0] == 't'|| e.Item2.ToCharArray()[0] == 't' || e.Item3.ToCharArray()[0] == 't').Count();
        }

        public string Day23Part2Solver(string filename)
        {
            Dictionary<int, HashSet<HashSet<string>>> cliqueStorage = new Dictionary<int, HashSet<HashSet<string>>>();

            List<string> input = DayTwentyThreeParser.ParseInputAsSingleArrayOfStrings(filename);

            List<List<string>> test = input.Select(e => e.Split('-').ToList()).ToList();

            HashSet<string> computerNames = new HashSet<string>();

            Dictionary<string, HashSet<string>> graph = new();

            ConstructGraph(input, graph);

            int counter = 1;

            ConstructClique(1, cliqueStorage, graph);


            while (cliqueStorage.ContainsKey(counter))
            {
                counter += 1;
                ConstructClique(counter, cliqueStorage, graph);
            }

            counter -= 1;

            List<string> largestClique = cliqueStorage[counter].First().ToList();

            largestClique.Sort();

            return string.Join(",", largestClique);
        }

        public void ConstructClique(int cliqueSize, Dictionary<int, HashSet<HashSet<string>>> cliqueStorage, Dictionary<string, HashSet<string>> graph)
        {
            HashSet<HashSet<string>> curCliqueHashSetList = new HashSet<HashSet<string>>();
            if (cliqueSize == 1)
            {
                foreach (string key in graph.Keys)
                {
                    HashSet<string> curCliqueHashSet = new HashSet<string>();

                    curCliqueHashSet.Add(key);

                    curCliqueHashSetList.Add(curCliqueHashSet);
                }
            }
            else
            {
                HashSet<HashSet<string>> curComparisonCliques = cliqueStorage[cliqueSize - 1];

                foreach (string key in graph.Keys)
                {
                    foreach(HashSet<string> clique in curComparisonCliques)
                    {
                        bool possibleClique = true;
                        if (!clique.Contains(key))
                        {
                            foreach (string node in clique)
                            {
                                if (!graph[node].Contains(key))
                                {
                                    possibleClique = false;
                                    break;
                                }
                            }
                            if (possibleClique)
                            {
                                HashSet<string> newClique = new HashSet<string>(clique);
                                newClique.Add(key);

                                bool dupe = CheckForEqualHashset(newClique, curCliqueHashSetList);
                                if (!dupe)
                                {
                                    curCliqueHashSetList.Add(newClique);
                                }
                            }
                        }
                    }
                }

            }

            if (curCliqueHashSetList.Count() > 0)
            {
                cliqueStorage.Add(cliqueSize, curCliqueHashSetList);
            }
        }

        public bool CheckForEqualHashset(HashSet<string> clique, HashSet<HashSet<string>> curCliqueHashSetList)
        {
            foreach (var curSet in curCliqueHashSetList)
            {
                bool curSetCollision = true;
                foreach (var elem in clique)
                {
                    if (!curSet.Contains(elem))
                    {
                        curSetCollision = false;
                        break;
                    }
                }
                if (curSetCollision)
                {
                    return true;
                }
            }
            return false;
        }

        public void ConstructUniqueTriplesSet(Dictionary<string, HashSet<string>> graph, HashSet<(string, string, string)> uniqueTriples)
        {
            foreach (var key in graph.Keys)
            {
                List<string> adjacentNodes = graph[key].ToList();
                for (int i = 0; i < adjacentNodes.Count; ++i)
                {
                    string firstNode = adjacentNodes[i];
                    for (int j = i + 1; j < adjacentNodes.Count; ++j)
                    {
                        string secondNode = adjacentNodes[j];

                        if (graph[firstNode].Contains(secondNode))
                        {
                            List<string> triple = [key, firstNode, secondNode];
                            triple.Sort();
                            uniqueTriples.Add((triple[0], triple[1], triple[2]));
                        }
                    }
                }
            }
        }

        public void ConstructGraph(List<string> input, Dictionary<string, HashSet<string>> graph)
        {
            foreach (var curInputStr in input)
            {
                List<string> splitStr = curInputStr.Split("-").Where(e => e != "").ToList();

                if (!graph.ContainsKey(splitStr[0]))
                {
                    graph.Add(splitStr[0], [splitStr[1]]);
                }
                else
                {
                    graph[splitStr[0]].Add(splitStr[1]);
                }
                if (!graph.ContainsKey(splitStr[1]))
                {
                    graph.Add(splitStr[1], [splitStr[0]]);
                }
                else
                {
                    graph[splitStr[1]].Add(splitStr[0]);
                }
            }
        }
    }
}
