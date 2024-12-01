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
    }
}
