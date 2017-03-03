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
        #region Constants

        private const string commandNotRecognized = "Command not recognized or in wrong format.  Enter 'help' to view a list of commands.";
        private const string commandRegex = @"^(?<command>[A-Za-z]*)";
        private const string directPathRegex = @"^([A-Z])*$";
        private const string graphRegex = @"^((?<start>[A-Z]{1})(?<end>[A-Z]{1})(?<distance>\d+)(,\s)*)*((?<start>[A-Z]{1})(?<end>[A-Z]{1})(?<distance>\d+))$";
        private const string help = @"
            dp [PATH]                               -- Finds direct path.  EX: ABC will find the direct path throuh A, B then C.
            maxstops [STOPS] [START] [END]          -- Finds the number of trips with less than or equal to [MAXSTOPS] stops. EX: maxstops 1 A B
            exactstops [STOPS] [START] [END]        -- Finds the number of trips with exactly [STOPS] stops. EX: exactstops 1 A B
            shortest [START] [END]                  -- Finds the shortest path. EX: shortest A B
            shorterthan [DISTANCE] [START] [END]    -- Finds the number of paths with a distance shorter than [DISTANCE]. EX: shorterthan 30 A B
            help                                    -- Shows this help menu.
            q                                       -- exits the application.
        ";
        private const string stopsAndDistanceRegex = @"^(?<stops>\d+)\s(?<start>[A-Z])\s(?<end>[A-Z])$";
        private const string noPath = "NO SUCH ROUTE";
        private const string shortestRegex = @"^(?<start>[A-Z]*)\s(?<end>[A-Z]*)$";

        #endregion

        public static void Main(string[] args)
        {
            TrainNetwork map;
            Match commandMatch;
            Regex regEngine;
            string command, input, parameters;
            
            map = new TrainNetwork();
            regEngine = new Regex(graphRegex);

            do
            {
                Console.WriteLine("Enter a graph to begin.");
                input = Console.ReadLine();
                commandMatch = regEngine.Match(input);

                if (!commandMatch.Success)
                    Console.WriteLine(commandNotRecognized);
            }
            while (!commandMatch.Success);
            
            for (int i = 0; i < commandMatch.Groups["start"].Captures.Count; i++)
            {
                char startName, endName;
                int distance;

                startName = commandMatch.Groups["start"].Captures[i].Value[0];
                endName = commandMatch.Groups["end"].Captures[i].Value[0];
                distance = Convert.ToInt32(commandMatch.Groups["distance"].Captures[i].Value);

                map.AddRoute(startName, endName, distance);
            }

            Console.WriteLine(help);
            input = Console.ReadLine();

            while (input != "Q")
            {
                if (input == "help")
                {
                    Console.WriteLine(help);
                }
                else
                {
                    regEngine = new Regex(commandRegex);
                    commandMatch = regEngine.Match(input);
                    command = commandMatch.Value;
                    parameters = input.Replace(command + " ", "");

                    try
                    {
                        switch (command.ToLower())
                        {
                            case "dp":
                                commandMatch = MatchCommandRegex(regEngine, directPathRegex, parameters);
                                FindDirectPath(new LinkedList<char>(commandMatch.Value.ToCharArray()), map);
                                break;

                            case "maxstops":
                                commandMatch = MatchCommandRegex(regEngine, stopsAndDistanceRegex, parameters);
                                FindMaxStops(Convert.ToInt32(commandMatch.Groups["stops"].Value), 
                                    commandMatch.Groups["start"].Value, commandMatch.Groups["end"].Value, map);
                                break;

                            case "exactstops":
                                commandMatch = MatchCommandRegex(regEngine, stopsAndDistanceRegex, parameters);
                                FindExactStops(Convert.ToInt32(commandMatch.Groups["stops"].Value),
                                    commandMatch.Groups["start"].Value, commandMatch.Groups["end"].Value, map);
                                break;

                            case "shortest":
                                commandMatch = MatchCommandRegex(regEngine, shortestRegex, parameters);
                                FindShortestPath(commandMatch.Groups["start"].Value, commandMatch.Groups["end"].Value, map);
                                break;

                            case "shorterthan":
                                commandMatch = MatchCommandRegex(regEngine, stopsAndDistanceRegex, parameters);
                                FindPathShorterThan(Convert.ToInt32(commandMatch.Groups["stops"].Value),
                                    commandMatch.Groups["start"].Value, commandMatch.Groups["end"].Value, map);
                                break;

                            default:
                                Console.WriteLine(commandNotRecognized);
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(commandNotRecognized);
                    }
                }
                
                input = Console.ReadLine();
            }
        }

        #region Methods and Helpers

        private static Match MatchCommandRegex(Regex regEngine, string regex, string parameters)
        {
            Match commandMatch;

            regEngine = new Regex(regex);
            commandMatch = regEngine.Match(parameters);

            if (!commandMatch.Success)
                throw new Exception();

            return commandMatch;
        }

        private static void FindPathShorterThan(int distance, string start, string end, TrainNetwork map)
        {
            try
            {
                Console.WriteLine(map.NumTripsWithDistanceLessThanN(distance, start[0], end[0]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void FindShortestPath(string start, string end, TrainNetwork map)
        {
            try
            {
                Console.WriteLine(map.ShortestRoute(start[0], end[0]));
            }
            catch (Exception)
            {
                Console.WriteLine(noPath);
            }
        }

        private static void FindExactStops(int stops, string start, string end, TrainNetwork map)
        {
            try
            {
                Console.WriteLine(map.NumTripsWithExactlyNStops(stops, start[0], end[0]));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void FindMaxStops(int stops, string start, string end, TrainNetwork map)
        {
            try
            {
                Console.WriteLine(map.NumTripsWithMaxNStops(stops, start[0], end[0]));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void FindDirectPath(LinkedList<char> list, TrainNetwork map)
        {
            try
            {
                Console.WriteLine(map.FindDirectDistance(list));
            }
            catch (PathNotFound)
            {
                Console.WriteLine(noPath);
            }
        }

        #endregion

    }
}
