using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2024
{
    public class AdventOfCode2024Parser
    {

        public List<List<string>> ParseInput(string filename)
        {
            List<List<string>> returnedInput = new List<List<string>>();
            StreamReader reader = File.OpenText(filename);
            string curLine;
            while ((curLine = reader.ReadLine()) != null)
            {
                List<string> curStringList = curLine.Split(" ").Where(e => e != "").ToList();
                returnedInput.Add(curStringList);
            }
            reader.Close();
            return returnedInput;
        }

        public List<List<char>> ParseInputNoRemovedWhitespace(string filename)
        {
            List<List<char>> returnedInput = new List<List<char>>();
            StreamReader reader = File.OpenText(filename);
            string curLine;
            while ((curLine = reader.ReadLine()) != null)
            {
                List<char> curStringList = curLine.ToCharArray().ToList();
                returnedInput.Add(curStringList);
            }
            reader.Close();
            return returnedInput;
        }

        public List<List<int>> ParseInputAsInts(string filename)
        {
            List<List<int>> returnedInput = new List<List<int>>();
            StreamReader reader = File.OpenText(filename);
            string curLine;
            while((curLine = reader.ReadLine()) != null)
            {
                List<string> curStringList = curLine.Split(" ").Where(e => e != "").ToList();
                List<int> curStringListAsInts = curStringList.Select(e => Int32.Parse(e)).ToList();
                returnedInput.Add(curStringListAsInts);
            }
            return returnedInput;
        }

        public List<List<string>> ParseInputAsArrayOfStrings(string filename)
        {
            List<List<string>> returnedInput = new List<List<string>>();
            StreamReader reader = File.OpenText(filename);
            string curLine;
            while ((curLine = reader.ReadLine()) != null)
            {
                List<string> curStringList = curLine.Select(e => e.ToString()).ToList();
                returnedInput.Add(curStringList);
            }
            return returnedInput;
        }

        public List<string> ParseInputAsSingleArrayOfStrings(string filename)
        {
            List<string> returnedInput = new List<string>();
            StreamReader reader = File.OpenText(filename);
            string curLine;
            while ((curLine = reader.ReadLine()) != null)
            {
                returnedInput.Add(curLine);
            }
            return returnedInput;
        }

        public List<int> ParseInputAsArrayOfIntsFromSingleLine(string filename)
        {
            List<int> ints = new List<int>();
            StreamReader reader = File.OpenText(filename);
            string curLine;
            while ((curLine = reader.ReadLine()) != null)
            {
                char[] curLineArr = curLine.ToCharArray();
                foreach (var item in curLineArr)
                {
                    ints.Add(int.Parse(item.ToString()));
                }
            }
            return ints;
        }
    }
}
