using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DealerOnJordanFinci
{
    public class Program
    {
        private const string noPath = "NO SUCH ROUTE";
        private const string graphRegex = @"((?<start>[A-D]{1})(?<end>[A-D]{1})(?<distance>\d+)(,\s)*)";

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Westworld Express!");
            Console.WriteLine("Enter a graph to retrieve graph statistics or Q to quit.");

            Regex regEngine = new Regex(graphRegex);

            string input = Console.ReadLine();
            while (input != "Q")
            {
                Map map = new Map();
                MatchCollection matches = regEngine.Matches(input);

                foreach (Match match in matches)
                {
                    char startName, endName;
                    int distance;

                    startName = match.Groups["start"].Value[0];
                    endName = match.Groups["end"].Value[0];
                    distance = Convert.ToInt32(match.Groups["distance"].Value);

                    map.AddRoute(startName, endName, distance);
                }

                //GenerateOutput(map);

                input = Console.ReadLine();
            }

            Console.WriteLine("Thanks for riding!");
        }

        //public static void GenerateOutput(Map map)
        //{
        //    Console.WriteLine("Not implemented");
        //}

        private string findDirectDistance(Map map, LinkedList<char> route)
        {
            try
            {
                return map.FindDirectDistance(route).ToString();
            }
            catch (ArgumentOutOfRangeException)
            {
                return noPath;
            }
        }
    }
}
