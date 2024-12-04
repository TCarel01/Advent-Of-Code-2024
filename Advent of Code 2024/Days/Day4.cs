using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day4
    {
        AdventOfCode2024Parser parser;
        public Day4()
        {
            this.parser = new AdventOfCode2024Parser();
        }

        public int Day4Part1Solver(string filename)
        {
            var input = this.parser.ParseInputAsArrayOfStrings(filename);

            int totalSum = 0;

            for (int row = 0; row < input.Count(); ++row)
            {
                for (int col = 0; col < input.Count; ++col)
                {
                    if (input[row][col] == "X")
                    {
                        for (int i = -1; i <= 1; ++i)
                        {
                            for (int j = -1; j <= 1; ++j)
                            {
                                totalSum += Search(input, row, col, "X", i, j);
                            }
                        }
                    }
                }
            }

            return totalSum;
        }

        public int Day4Part2Solver(string filename)
        {
            var input = this.parser.ParseInputAsArrayOfStrings(filename);

            int totalSum = 0;

            for (int row = 1; row < input.Count() - 1; ++row)
            {
                for (int col = 1; col < input[0].Count() - 1; ++col)
                {
                    string diagonal1 = input[row - 1][col - 1] + input[row][col] + input[row + 1][col + 1];
                    string diagonal2 = input[row - 1][col + 1] + input[row][col] + input[row + 1][col - 1];

                    if ((diagonal1 == "MAS" || diagonal1 == "SAM") && (diagonal2 == "MAS" || diagonal2 == "SAM"))
                    {
                        totalSum += 1;
                    }
                }
            }
            return totalSum;
        }

        public int Search(List<List<string>> input, int row, int col, String lastChar, int xdir, int ydir)
        {
            String charToSearch = "";
            switch(lastChar)
            {
                case "X":
                    charToSearch = "M";
                    break;
                case "M":
                    charToSearch = "A";
                    break;
                case "A":
                    charToSearch = "S";
                    break;
                case "S":
                    return 1;
                default:
                    return 0;
            }

            int nextRow = Math.Max(0, Math.Min(row + xdir, input.Count() - 1));
            int nextCol = Math.Max(0, Math.Min(col + ydir, input.ElementAt(0).Count() - 1));

            if (nextRow == row && xdir != 0 || nextCol == col && ydir != 0)
            {
                return 0;
            }

            else if (input.ElementAt(nextRow).ElementAt(nextCol) == charToSearch)
            {
                return Search(input, nextRow, nextCol, charToSearch, xdir, ydir);
            }
            else
            {
                return 0;
            }
        }


    }
}
