using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerOnJordanFinci
{
    public class Map
    {
        #region Properties

        private Dictionary<char, TrainStop> Cities;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of Map;
        /// </summary>
        public Map()
        {
            this.Cities = new Dictionary<char, TrainStop>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the route to the map.  If either <paramref name="startName"/> or <paramref name="endName"/>
        /// do not yet exist in the map, they are created and added to the map.
        /// </summary>
        /// <param name="startName"></param>
        /// <param name="endName"></param>
        /// <param name="distance"></param>
        public void AddRoute(char startName, char endName, int distance)
        {
            this.AddCity(startName).Routes.Add(this.AddCity(endName), distance);
        }

        /// <summary>
        /// Finds the total route distance between the cities passed as a parameter <paramref name="route"/>.
        /// Routes must contain more than 1 
        /// </summary>
        /// <param name="route"></param>
        /// <returns>Distance in integers</returns>
        /// <exception cref="ArgumentException">Thrown when the distance is null </exception>
        public int FindDirectDistance(LinkedList<char> route)
        {
            LinkedList<char> routeCopy;
            TrainStop startCity;

            if (route.Count() < 2)
            {
                throw new ArgumentException("To calulate a route, there must be at least two cities.");
            }

            routeCopy = new LinkedList<char>(route);
            startCity = this.Cities[route.First.Value];
            route.RemoveFirst();

            return FindDistance(startCity, route);
        }

        private int FindDistance(TrainStop startCity, LinkedList<char> route, int distance = 0)
        {
            TrainStop nextCity;
            int nextCityDistance;

            if (route.Count() == 0)
            {
                return distance;
            }
            else
            {
                if (!this.Cities.TryGetValue(route.First.Value, out nextCity))
                {
                    throw new ArgumentOutOfRangeException();
                }
                
                if (!startCity.Routes.TryGetValue(nextCity, out nextCityDistance))
                {
                    throw new ArgumentOutOfRangeException();
                }

                route.RemoveFirst();
                distance += nextCityDistance;

                return this.FindDistance(nextCity, route, distance);
            }
        }

        public bool HasStop(char cityName)
        {
            return this.Cities.ContainsKey(cityName);
        }

        /// <summary>
        /// Finds the number of trips between <paramref name="startCity"/> and <paramref name="endCity"/>
        /// with a maximum of <paramref name="nStops"/> stops.
        /// </summary>
        /// <param name="nStops"></param>
        /// <param name="startCity"></param>
        /// <param name="endCity"></param>
        /// <returns></returns>
        public int NumTripsWithMaxNStops(int nStops, char startCity, char endCity)
        {
            TrainStop currentCity;

            currentCity = this.Cities[startCity];

            return this.FindNumTripsWithMaxNStops(0, nStops, 0, currentCity, endCity);
        }

        private int FindNumTripsWithMaxNStops(int stops, int maxStops, int count, TrainStop currentCity, char endCity)
        {
            Dictionary<TrainStop, int> adjacentCities;
            adjacentCities = currentCity.Routes;

            if (stops <= maxStops)
            {
                foreach (TrainStop stop in adjacentCities.Keys)
                {
                    if (stop.Name == endCity)
                    {
                        count++;
                    }

                    count = this.FindNumTripsWithMaxNStops(stops+1, maxStops, count, stop, endCity);
                }
            }

            return count;
        }

        /// <summary>
        /// Finds the number of trips between <paramref name="startCity"/> and <paramref name="endCity"/>
        /// with exactly <paramref name="nStops"/> stops.
        /// </summary>
        /// <param name="nStops"></param>
        /// <param name="startCity"></param>
        /// <param name="endCity"></param>
        /// <returns></returns>
        public int NumTripsWithExactlyNStops(int nStops, char startCity, char endCity)
        {
            TrainStop currentCity;

            if (!this.Cities.TryGetValue(startCity, out currentCity))
            {
                throw new ArgumentOutOfRangeException("startCity");
            }

            return this.findNumTripsWithExactlyNStops(0, nStops, 0, currentCity, endCity);
        }

        private int findNumTripsWithExactlyNStops(int stops, int nStops, int count, TrainStop currentCity, char endCity)
        {
            Dictionary<TrainStop, int> adjacentCities;
            adjacentCities = currentCity.Routes;

            if (stops <= nStops)
            {
                foreach (TrainStop nextStop in adjacentCities.Keys)
                {
                    if (nStops == stops && nextStop.Name == endCity)
                    {
                        count++;
                    }
                    else
                    {
                        count = this.findNumTripsWithExactlyNStops(stops+1, nStops, count, nextStop, endCity);
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Finds the number of trips between <paramref name="startCity"/> and <paramref name="endCity"/>
        /// with a minnmum distance of <paramref name="distance"/>
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="startCity"></param>
        /// <param name="endCity"></param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the start city does not exist
        /// on the map.</exception>
        public int NumTripsWithDistanceLessThanN(int distance, char startCity, char endCity)
        {
            TrainStop currentCity;

            if (!this.Cities.TryGetValue(startCity, out currentCity))
            {
                throw new ArgumentOutOfRangeException("startCity");
            }

            return this.FindNumTripsWithDistanceLessThanN(distance, 0, 0, currentCity, endCity);
        }

        private int FindNumTripsWithDistanceLessThanN(int maxDistance, int currentDistance, int count, TrainStop currentCity, char endCity)
        {
            Dictionary<TrainStop, int> adjacentCities;
            adjacentCities = currentCity.Routes;
            
            foreach (TrainStop nextStop in adjacentCities.Keys)
            {
                int nextDistance;
                
                nextDistance = currentDistance + adjacentCities[nextStop];
                if (nextDistance < maxDistance)
                {
                    if (nextStop.Name == endCity)
                    {
                        count++;
                    }

                    count = this.FindNumTripsWithDistanceLessThanN(maxDistance, nextDistance, count, nextStop, endCity);
                }
            }

            return count;
        }

        /// <summary>
        /// Finds the shortest route between stops.  
        /// 
        /// This is just a basic implementation of Dijkstra's, referenced:
        /// https://en.wikipedia.org/wiki/Dijkstra's_algorithm
        /// </summary>
        /// <param name="stopA"></param>
        /// <param name="stopB"></param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when 
        /// <paramref name="stopA"/> does not exist in the Map.</exception>
        public int ShortestRoute(char stopA, char stopB)
        {
            Dictionary<char, int> distances;
            Dictionary<char, TrainStop> previous;
            List<char> stops;
            TrainStop startCity;

            if (!this.Cities.TryGetValue(stopA, out startCity))
            {
                throw new ArgumentOutOfRangeException("stopA");
            }

            distances = new Dictionary<char, int>();
            previous = new Dictionary<char, TrainStop>();
            stops = new List<char>();

            foreach (char stop in this.Cities.Keys)
            {
                distances.Add(stop, int.MaxValue);
                previous.Add(stop, null);
                stops.Add(stop);
            }

            distances[stopA] = 0;

            while (stops.Count() > 0)
            {
                char currStop;
                int minDistance;

                minDistance = int.MaxValue;
                currStop = stopA; //Compiler?

                //Find the node with the least distance first.
                foreach (char stop in stops)
                {
                    if (minDistance > distances[stop])
                    {
                        minDistance = distances[stop];
                        currStop = stop;
                    }
                }

                if (currStop == stopB)
                {
                    return this.findShortestPathDistance(currStop, previous, distances);
                }

                stops.Remove(currStop);

                foreach (TrainStop neighbor in this.Cities[currStop].Routes.Keys)
                {
                    int dist;

                    dist = distances[currStop] + this.Cities[currStop].Routes[neighbor];
                    if (dist < distances[neighbor.Name])
                    {
                        distances[neighbor.Name] = dist;
                        previous[neighbor.Name] = this.Cities[currStop];
                    }
                }
            }

            throw new Exception("Not found.");
        }

        private int findShortestPathDistance(char lastStop, Dictionary<char, TrainStop> previous, Dictionary<char, int> distances)
        {
            char currStop;
            LinkedList<char> pathFound;

            currStop = lastStop;
            pathFound = new LinkedList<char>();
            while (previous[currStop] != null)
            {
                pathFound.AddFirst(currStop);
                currStop = previous[currStop].Name;
            }

            pathFound.AddFirst(currStop);

            return this.FindDirectDistance(pathFound);
        }

        #endregion

        #region Private

        /// <summary>
        /// Adds the city with name <paramref name="cityName"/> to the map if 
        /// it does not already exist.
        /// </summary>
        /// <param name="cityName"></param>
        private TrainStop AddCity(char cityName)
        {
            TrainStop newCity;
            if (!this.Cities.TryGetValue(cityName, out newCity))
            {
                newCity = new TrainStop(cityName);
                this.Cities.Add(cityName, newCity);
            }
            return newCity;
        }

        #endregion
    }
}
