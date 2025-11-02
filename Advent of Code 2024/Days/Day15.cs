using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day15
    {

        private AdventOfCode2024Parser dayFifteenParser;

        public Day15()
        {
            dayFifteenParser = new AdventOfCode2024Parser();
        }

        public List<List<string>> ParseInput(string filename)
        {
            var input = dayFifteenParser.ParseInputAsArrayOfStrings(filename);

            return input;
        }

        public int Day15Part1Solver(string filename)
        {
            List<List<string>> input = ParseInput(filename);

            List<List<string>> warehouse = input.Where(e => e.Where(f => f == "#").ToList().Count != 0).ToList();

            string directions = String.Join("", input.Where(e => e.Where(f => f == "^" || f == "v" || f == "<" || f == ">").ToList().Count != 0).ToList().SelectMany(e => e));

            int count = 0;
            foreach(var direction in directions.ToCharArray().Select(e => e.ToString()).ToList())
            {
                ++count;
                ExecuteBoardMove(warehouse, direction);
                //Console.WriteLine($"Move {count}:");
                //PrintWarehouse(warehouse);
            }

            return ComputeWarehouseScore(warehouse);
        }

        public int Day15Part2Solver(string filename)
        {
            List<List<string>> input = ParseInput(filename);

            List<List<string>> warehouse = ResizeWarehouse(input.Where(e => e.Where(f => f == "#").ToList().Count != 0).ToList());

            string directions = String.Join("", input.Where(e => e.Where(f => f == "^" || f == "v" || f == "<" || f == ">").ToList().Count != 0).ToList().SelectMany(e => e));

            foreach (var direction in directions.ToCharArray().Select(e => e.ToString()).ToList())
            {
                ExecuteExpandedBoardMove(warehouse, direction);
            }

            return ComputeWarehouseScoreExpanded(warehouse);
        }

        public void ExecuteBoardMove(List<List<string>> warehouse, string curDirection)
        {
            List<int> robotCoords = FindRobotCoords(warehouse);

            List<int> direction = curDirection == "^" ? [0, -1] : curDirection == ">" ? [1, 0] :
                curDirection == "v" ? [0, 1] : [-1, 0];

            List<int> startBoxCoords = [robotCoords[0] + direction[0], robotCoords[1] + direction[1]];

            bool canMoveRobot = warehouse[startBoxCoords[1]][startBoxCoords[0]] != "#";

            if (warehouse[startBoxCoords[1]][startBoxCoords[0]] == "O")
            {
                canMoveRobot = MoveSingularBox(warehouse, startBoxCoords, direction);
            }

            if (canMoveRobot)
            {
                warehouse[startBoxCoords[1]][startBoxCoords[0]] = "@";
                warehouse[robotCoords[1]][robotCoords[0]] = ".";
            }
        }

        public void ExecuteExpandedBoardMove(List<List<string>> warehouse, string curDirection)
        {
            List<int> robotCoords = FindRobotCoords(warehouse);

            List<int> direction = curDirection == "^" ? [0, -1] : curDirection == ">" ? [1, 0] :
                curDirection == "v" ? [0, 1] : [-1, 0];

            List<int> startRobotCoords = [robotCoords[0] + direction[0], robotCoords[1] + direction[1]];

            bool canMoveRobot = warehouse[startRobotCoords[1]][startRobotCoords[0]] != "#";

            if (canMoveRobot && warehouse[startRobotCoords[1]][startRobotCoords[0]] == "[")
            {
                canMoveRobot = MoveExpandedWarehouseBox(warehouse, startRobotCoords, direction, false);
            }
            else if (canMoveRobot && warehouse[startRobotCoords[1]][startRobotCoords[0]] == "]")
            {
                canMoveRobot = MoveExpandedWarehouseBox(warehouse, [startRobotCoords[0] - 1, startRobotCoords[1]], direction, false);
            }

            if (canMoveRobot)
            {
                if (warehouse[startRobotCoords[1]][startRobotCoords[0]] == "[")
                {
                    canMoveRobot = MoveExpandedWarehouseBox(warehouse, startRobotCoords, direction, true);
                }
                else if (warehouse[startRobotCoords[1]][startRobotCoords[0]] == "]")
                {
                    canMoveRobot = MoveExpandedWarehouseBox(warehouse, [startRobotCoords[0] - 1, startRobotCoords[1]], direction, true);
                }
                warehouse[startRobotCoords[1]][startRobotCoords[0]] = "@";
                warehouse[robotCoords[1]][robotCoords[0]] = ".";
            }
        }

        public bool MoveSingularBox(List<List<string>> warehouse, List<int> curBoxCoords, List<int> direction)
        {
            List<int> moveToCoords = [curBoxCoords[0] + direction[0], curBoxCoords[1] + direction[1]];

            while (warehouse[moveToCoords[1]][moveToCoords[0]] != "." && warehouse[moveToCoords[1]][moveToCoords[0]] != "#")
            {
                moveToCoords = [moveToCoords[0] + direction[0], moveToCoords[1] + direction[1]];
            }

            if (warehouse[moveToCoords[1]][moveToCoords[0]] == ".")
            {
                warehouse[moveToCoords[1]][moveToCoords[0]] = "O";
                warehouse[curBoxCoords[1]][curBoxCoords[0]] = ".";
                return true;
            }

            return false;
        }

        public bool MoveExpandedWarehouseBox(List<List<string>> warehouse, List<int> curBoxCoords, List<int> direction, bool moveBox)
        {
            // curBoxCoords is the coordinates of [ for the current box

            if (direction[0] != 0)
            {
                List<int> newBoxPos = [curBoxCoords[0] + direction[0], curBoxCoords[1]];

                bool canMoveBox;

                if (warehouse[newBoxPos[1]][newBoxPos[0]] == "#" || 
                    warehouse[newBoxPos[1]][newBoxPos[0] + 1] == "#")
                {
                    canMoveBox = false;
                }
                else if (warehouse[newBoxPos[1]][newBoxPos[0]] == "]" && direction[0] == -1)
                {
                    canMoveBox = MoveExpandedWarehouseBox(warehouse, [newBoxPos[0] - 1, newBoxPos[1]], direction, moveBox);
                }
                else if (warehouse[newBoxPos[1]][newBoxPos[0] + 1] == "[" && direction[0] == 1)
                {
                    canMoveBox = MoveExpandedWarehouseBox(warehouse, [newBoxPos[0] + 1, newBoxPos[1]], direction, moveBox);

                }
                else
                {
                    canMoveBox = true;
                }

                if (canMoveBox && moveBox)
                {
                    warehouse[curBoxCoords[1]][curBoxCoords[0]] = ".";
                    warehouse[curBoxCoords[1]][curBoxCoords[0] + 1] = ".";
                    warehouse[newBoxPos[1]][newBoxPos[0]] = "[";
                    warehouse[newBoxPos[1]][newBoxPos[0] + 1] = "]";
                }
                return canMoveBox;
            }

            else
            {
                List<int> newBoxPos = [curBoxCoords[0], curBoxCoords[1] + direction[1]];

                bool canMoveBox;

                if (warehouse[newBoxPos[1]][newBoxPos[0]] == "#" || warehouse[newBoxPos[1]][newBoxPos[0] + 1] == "#")
                {
                    canMoveBox = false;
                }
                else if (warehouse[newBoxPos[1]][newBoxPos[0]] == "[")
                {
                    canMoveBox = MoveExpandedWarehouseBox(warehouse, newBoxPos, direction, moveBox);
                }

                else if (warehouse[newBoxPos[1]][newBoxPos[0]] == "]" && warehouse[newBoxPos[1]][newBoxPos[0] + 1] == "[")
                {
                    canMoveBox = MoveExpandedWarehouseBox(warehouse, [newBoxPos[0] - 1, newBoxPos[1]], direction, moveBox)
                        && MoveExpandedWarehouseBox(warehouse, [newBoxPos[0] + 1, newBoxPos[1]], direction, moveBox);
                }
                else if (warehouse[newBoxPos[1]][newBoxPos[0]] == "]")
                {
                    canMoveBox = MoveExpandedWarehouseBox(warehouse, [newBoxPos[0] - 1, newBoxPos[1]], direction, moveBox);
                }
                else if (warehouse[newBoxPos[1]][newBoxPos[0] + 1] == "[")
                {
                    canMoveBox = MoveExpandedWarehouseBox(warehouse, [newBoxPos[0] + 1, newBoxPos[1]], direction, moveBox);
                }
                else
                {
                    canMoveBox = true;
                }

                if (canMoveBox && moveBox)
                {
                    warehouse[curBoxCoords[1]][curBoxCoords[0]] = ".";
                    warehouse[curBoxCoords[1]][curBoxCoords[0] + 1] = ".";
                    warehouse[newBoxPos[1]][newBoxPos[0]] = "[";
                    warehouse[newBoxPos[1]][newBoxPos[0] + 1] = "]";
                }
                return canMoveBox;
            }
        }


        public List<int> FindRobotCoords(List<List<string>> input)
        {
            for (int i = 0; i < input.Count; ++i)
            {
                for (int j = 0; j < input[i].Count; ++j)
                {
                    if (input[i][j] == "@")
                    {
                        return [j, i];
                    }
                }
            }
            return [-1, -1];
        }

        public int ComputeWarehouseScore(List<List<string>> warehouse)
        {
            int score = 0;
            for (int i = 0; i < warehouse.Count; ++i)
            {
                for (int j = 0; j < warehouse[i].Count; ++j)
                {
                    if (warehouse[i][j] == "O")
                    {
                        score += i * 100 + j;
                    }
                }
            }
            return score;
        }

        public int ComputeWarehouseScoreExpanded(List<List<string>> warehouse)
        {
            int score = 0;
            for (int i = 0; i < warehouse.Count; ++i)
            {
                for (int j = 0; j < warehouse[i].Count; ++j)
                {
                    if (warehouse[i][j] == "[")
                    {
                        score += i * 100 + j;
                    }
                }
            }
            return score;
        }

        public List<List<string>> ResizeWarehouse(List<List<string>> warehouse)
        {
            List<List<string>> newWarehouse = new List<List<string>>();

            for (int i = 0; i < warehouse.Count; ++i)
            {
                List<string> curWarehouseRow = new List<string>();

                for (int j = 0; j < warehouse[0].Count; ++j)
                {
                    if (warehouse[i][j] == "#")
                    {
                        curWarehouseRow.Add("#");
                        curWarehouseRow.Add("#");
                    }
                    else if (warehouse[i][j] == ".")
                    {
                        curWarehouseRow.Add(".");
                        curWarehouseRow.Add(".");
                    }
                    else if (warehouse[i][j] == "O")
                    {
                        curWarehouseRow.Add("[");
                        curWarehouseRow.Add("]");
                    }
                    else
                    {
                        curWarehouseRow.Add("@");
                        curWarehouseRow.Add(".");
                    }
                }
                newWarehouse.Add(curWarehouseRow);
            }

            return newWarehouse;
        }

        public void PrintWarehouse(List<List<string>> warehouse)
        {
            for (int i = 0; i < warehouse.Count; ++i)
            {
                String curLineString = String.Join("", warehouse[i]);
                Console.WriteLine(curLineString);
            }
        }
    }
}
