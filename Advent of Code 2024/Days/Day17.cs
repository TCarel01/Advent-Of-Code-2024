using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024.Days
{
    public class Day17
    {

        private AdventOfCode2024Parser daySeventeenParser;

        public Day17()
        {
            daySeventeenParser = new AdventOfCode2024Parser();
        }

        public string Day17Part1Solver(string filename)
        {
            List<string> input = daySeventeenParser.ParseInputAsSingleArrayOfStrings(filename);

            int registerA = int.Parse(input[0].Substring(input[0].IndexOf(": ") + 2));

            int registerB = int.Parse(input[1].Substring(input[0].IndexOf(": ") + 2));

            int registerC = int.Parse(input[2].Substring(input[0].IndexOf(": ") + 2));

            List<int> program = input[4].Substring(input[4].IndexOf(": ") + 2).Split(",").Select(e => int.Parse(e)).ToList();

            OpcodeComputer computer = new OpcodeComputer(registerA, registerB, registerC);

            return computer.ExecuteProgram(program);
        }


        public long Day17Part2Solver(string filename)
        {
            List<string> input = daySeventeenParser.ParseInputAsSingleArrayOfStrings(filename);

            List<int> program = input[4].Substring(input[4].IndexOf(": ") + 2).Split(",").Select(e => int.Parse(e)).ToList();

            List<long> outputs = ReverseComputationStep(0, program, program.Count - 1);

            return outputs.Aggregate(long.MaxValue, (acc, e) => e < acc ? e : acc);
        }


        // (2, 4) take A modulo 8, store it in B
        // (1, 1) B XOR 1 (flip the last bit of B (if B is even, adds one to B, otherwise subtracts one from B)
        // (7, 5) A / 2 ^ B, store result in C (1, 10, 100, 1000, 10000, 100000, 1000000, 10000000)
        // (1, 5) B XOR 5 (101), flip first and third bits of B, stored in B
        // (4, 0) B XOR C, stored in B
        // Output B modulo 8
        // A /= 8
        // Jump to beginning of the program until the register A is zeroed out

        // TO GET THIS OUTPUT:

        // A must end at 0
        // B mod 8 must first be 0
        // most of C doesn't matter at any point in time, only the first three bits matter

        // start with A = 0, in order for the computation to end, A must have 16 bits. 
        // For an arbitrary iteration
        // guess the three least significant bits of A
        // derive B
        // Figure out what C must be
        // check if we have determined what C must be according to the bits in the string, if we have not, set them to what C must be. If we have, check that they match. If they do, continue, otherwise fail. We can do this by having an array of integers initialized to 0, and setting them to 1 if they're already been initialized
        // Pass the current A up, repeat until we have all 16 digits. Run the computation to confirm it matches

        public List<long> ReverseComputationStep(long startingA, List<int> output, int curOutputIdx)
        {
            if (curOutputIdx < 0)
            {
                return [startingA];
            }

            List<int> possibleLeastSignificantDigits = [0, 1, 2, 3, 4, 5, 6, 7];

            List<long> possibleA = new List<long>();

            for (int i = 0; i < possibleLeastSignificantDigits.Count; ++i)
            {
                int curLeastSignificantBits = possibleLeastSignificantDigits[i];

                long currentA = (startingA << 3) + curLeastSignificantBits;

                int curB = (curLeastSignificantBits % 8) ^ 1;

                long curBXOR5 = curB ^ 5;

                long curOutput = output[curOutputIdx];

                long curC = curBXOR5 ^ curOutput;

                long test = (currentA >> curB) % 8; 

                if (curC == test)
                {
                    possibleA = possibleA.Concat(ReverseComputationStep(currentA, output, curOutputIdx - 1)).ToList();
                }
            }

            return possibleA;
        }
    }

    public class OpcodeComputer
    {
        private int RegisterA { get; set; }
        private int RegisterB { get; set; }
        private int RegisterC { get; set; }
        private int InstructionPointer { get; set; }

        public OpcodeComputer(int registerA, int registerB, int registerC)
        {
            RegisterA = registerA;
            RegisterB = registerB;
            RegisterC = registerC;
            InstructionPointer = 0;
        }


        public string ExecuteProgram(List<int> program)
        {
            List<string> output = new List<string>();
            InstructionPointer = 0;
            while (InstructionPointer < program.Count)
            {
                int opcode = program[InstructionPointer];
                int operand = program[InstructionPointer + 1];

                switch (opcode)
                {
                    case 0:
                        ADV(operand);
                        InstructionPointer += 2;
                        break;
                    case 1:
                        BXL(operand);
                        InstructionPointer += 2;
                        break;
                    case 2:
                        BST(operand);
                        InstructionPointer += 2;
                        break;
                    case 3:
                        bool jumpSuccessful = JNZ(operand);
                        InstructionPointer = !jumpSuccessful ? InstructionPointer + 2 : InstructionPointer;
                        break;
                    case 4:
                        BXC();
                        InstructionPointer += 2;
                        break;
                    case 5:
                        int curOutput = OUT(operand);
                        output.Add(string.Join(',', curOutput.ToString().Split("")));
                        InstructionPointer += 2;
                        break;
                    case 6:
                        BDV(operand);
                        InstructionPointer += 2;
                        break;
                    case 7:
                        CDV(operand);
                        InstructionPointer += 2;
                        break;
                    default:
                        break;
                }
            }
            return string.Join(',', output);
        }



        private int GetComboOperandResult(int comboOperand)
        {
            switch (comboOperand)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 3;
                case 4:
                    return RegisterA;
                case 5:
                    return RegisterB;
                case 6:
                    return RegisterC;
                default:
                    return -1;
            }
        }

        private int ADV(int operand)
        {
            int properOperand = GetComboOperandResult(operand);

            RegisterA = (int)Math.Floor(1.0 * RegisterA / Math.Pow(2, properOperand));

            return RegisterA;
        }
        private int BXL(int operand)
        {
            RegisterB = RegisterB ^ operand;
            return RegisterB;
        }

        private int BST(int operand)
        {
            int properOperand = GetComboOperandResult(operand);

            RegisterB = properOperand % 8;

            return RegisterB;
        }

        private bool JNZ(int operand)
        {
            if (RegisterA == 0)
            {
                return false;
            }
            InstructionPointer = operand;
            return true;
        }

        private int BXC()
        {
            RegisterB = RegisterB ^ RegisterC;
            return RegisterB;
        }

        private int OUT(int operand)
        {
            return GetComboOperandResult(operand) % 8;
        }

        private int BDV(int operand)
        {
            int properOperand = GetComboOperandResult(operand);

            RegisterB = (int)Math.Floor(1.0 * RegisterA / Math.Pow(2, properOperand));

            return RegisterB;
        }

        private int CDV(int operand)
        {
            int properOperand = GetComboOperandResult(operand);

            RegisterC = (int)Math.Floor(1.0 * RegisterA / Math.Pow(2, properOperand));

            return RegisterC;
        }
    }
}
