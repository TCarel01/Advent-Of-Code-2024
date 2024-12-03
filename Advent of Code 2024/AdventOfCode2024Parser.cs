using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
