using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    internal class Day9
    {
        AdventOfCode2024Parser parser;

        public Day9()
        {
            this.parser = new AdventOfCode2024Parser();
        }

        public long Day9Part1Solver(string filename)
        {
            List<int> input = this.parser.ParseInputAsArrayOfIntsFromSingleLine(filename);

            List<string> spacedInput = SpaceInput(input);

            int firstFreeSpaceIdx = spacedInput.IndexOf(".");
            int rightMostIdx = spacedInput.Count - 1;

            while (firstFreeSpaceIdx >= 0 && firstFreeSpaceIdx <= rightMostIdx)
            {
                spacedInput[firstFreeSpaceIdx] = spacedInput[rightMostIdx];
                spacedInput[rightMostIdx] = ".";
                firstFreeSpaceIdx = spacedInput.IndexOf(".");
                
                while (true)
                {
                    rightMostIdx -= 1;
                    if (rightMostIdx < 0 || spacedInput[rightMostIdx] != ".") 
                    {
                        break;
                    }
                }
            }

            return ComputeAggregatedTotal(spacedInput);
        }

        public long Day9Part2Solver(string filename)
        {
            List<int> input = this.parser.ParseInputAsArrayOfIntsFromSingleLine(filename);

            List<string> spacedInput = SpaceInput(input);

            int firstFreeSpaceIdx = spacedInput.IndexOf(".", 0);
            int rightMostIdx = spacedInput.Count - 1;
            int curFileId = int.Parse(spacedInput[spacedInput.Count - 1]);

            while (curFileId > 0)
            {
                while(firstFreeSpaceIdx >= 0 && firstFreeSpaceIdx <= rightMostIdx)
                {
                    curFileId = int.Parse(spacedInput[rightMostIdx]);
                    int curFileSize = input[curFileId * 2];

                    int curFreeSpaceBlockSize = FindFreeSpaceBlockSize(spacedInput, firstFreeSpaceIdx);

                    if (curFreeSpaceBlockSize >= curFileSize)
                    {
                        for (int i = 0; i < curFileSize; ++i)
                        {
                            spacedInput[firstFreeSpaceIdx + i] = spacedInput[rightMostIdx - i];
                            spacedInput[rightMostIdx - i] = ".";
                        }
                        break;
                    }
                    else
                    {
                        firstFreeSpaceIdx = spacedInput.IndexOf(".", firstFreeSpaceIdx + curFreeSpaceBlockSize);
                    }
                }
                curFileId -= 1;
                rightMostIdx = FindRightMostIndex(spacedInput, curFileId);
                firstFreeSpaceIdx = spacedInput.IndexOf(".", 0);
            }

            return ComputeAggregatedTotal(spacedInput);
        }

        public int FindRightMostIndex(List<string> spacedInput, int curId)
        {
            for (int i = spacedInput.Count - 1; i >= 0; --i) 
            {
                if (spacedInput[i] == curId.ToString())
                {
                    return i;
                }
            }
            return -1;
        }

        public int FindFreeSpaceBlockSize(List<string> spacedInput, int startIdx)
        {
            int freeSpaceCount = 0;
            for (int i = startIdx; i < spacedInput.Count; ++i) 
            {
                if (spacedInput[i] == ".")
                {
                    ++freeSpaceCount;
                }
                else
                {
                    break;
                }
            }
            return freeSpaceCount;
        }


        private long ComputeAggregatedTotal(List<string> spacedInput)
        {
            long aggregatedTotal = 0;

            for (int i = 0; i < spacedInput.Count; ++i)
            {
                if (spacedInput[i] == ".")
                {
                    continue;
                }
                aggregatedTotal += i * int.Parse(spacedInput[i]);
            }

            return aggregatedTotal;
        }

        public List<string> SpaceInput(List<int> input)
        {
            List<string> returnVal = new List<string>();
            for (int i = 0; i < input.Count; i += 2)
            {
                int id = i / 2;
                int numOfId = input[i];
                int freeSpace = i + 1 < input.Count ? input[i + 1] : 0;

                for (int j = 0; j < numOfId + freeSpace; ++j)
                {
                    if (j < numOfId)
                    {
                        returnVal.Add(id.ToString());
                    }
                    else
                    {
                        returnVal.Add(".");
                    }
                }
            }
            return returnVal;
        }

    }
}
