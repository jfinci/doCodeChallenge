using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerOnJordanFinci
{
    /// <summary>
    /// A convienent exception thats a bit more
    /// descriptive than the standard System exceptions.
    /// </summary>
    public class PathNotFound : Exception
    {
        public PathNotFound()
            : base()
        { }

        public PathNotFound(char nextStop)
            : base(string.Format("Next stop {0} not found.", nextStop))
        { }

        public PathNotFound(char start, char end)
            : base(string.Format("Path between {0} and {1} does not exist.", start, end))
        { }
    }

    /// <summary>
    /// A directed graph representing a netowrk of trains.
    /// This could easily be made generic and serve as a 
    /// generic directed graph for similar applications!
    /// </summary>
    public class TrainNetwork
    {
        #region Properties

        private Dictionary<char, TrainStop> graph;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of Map;
        /// </summary>
        public TrainNetwork()
        {
            this.graph = new Dictionary<char, TrainStop>();
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
        public void AddRoute(char source, char destination, int distance)
        {
            TrainStop sourceNode = this.AddTrainRoute(source);
            TrainStop destinationNode = this.AddTrainRoute(destination);

            sourceNode.AdjacentStops.Add(destinationNode, distance);
        }

        /// <summary>
        /// Adds the city with name <paramref name="cityName"/> to the map if 
        /// it does not already exist.
        /// </summary>
        /// <param name="cityName"></param>
        private TrainStop AddTrainRoute(char cityName)
        {
            TrainStop newCity;
            if (!this.graph.TryGetValue(cityName, out newCity))
            {
                newCity = new TrainStop(cityName);
                this.graph.Add(cityName, newCity);
            }
            return newCity;
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
                throw new ArgumentException("Must contain ","route");

            routeCopy = new LinkedList<char>(route);

            if (!this.HasStop(route.First.Value))
                throw new PathNotFound();

            startCity = this.graph[route.First.Value];
            route.RemoveFirst();

            return FindDistance(startCity, route);
        }

        public bool HasStop(char cityName)
        {
            return this.graph.ContainsKey(cityName);
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
            if (!this.graph.TryGetValue(startCity, out currentCity))
                throw new ArgumentException("startCity");

            return this.FindNumTripsWithMaxNStops(1, nStops, 0, currentCity, endCity);
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

            if (!this.graph.TryGetValue(startCity, out currentCity))
                throw new ArgumentException("startCity");

            return this.FindNumTripsWithExactlyNStops(0, nStops, 0, currentCity, endCity);
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
            if (!this.graph.TryGetValue(startCity, out currentCity))
                throw new ArgumentException("startCity");
            

            return this.FindNumTripsWithDistanceLessThanN(distance, 0, 0, currentCity, endCity);
        }

        /// <summary>
        /// Finds the shortest route between stops.  
        /// 
        /// This is just a basic implementation of Dijkstra's, to implement,
        /// I referenced: https://en.wikipedia.org/wiki/Dijkstra's_algorithm
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
            bool ignoreIteration;

            if (!this.graph.TryGetValue(stopA, out startCity))
                throw new KeyNotFoundException("stopA");

            this.InitShortestRoute(out stops, out distances, out previous);
            distances[stopA] = 0;
            ignoreIteration = stopA == stopB;

            while (stops.Count() > 0)
            {
                char currStop;
                int leastDistance;

                currStop = this.FindNextStopWithLeastDistance(stops, distances);
                leastDistance = distances[currStop];

                if (!ignoreIteration && currStop == stopB)
                    return this.FindShortestPathDistance(stopB, previous, distances);

                stops.Remove(currStop);

                foreach (TrainStop neighbor in this.graph[currStop].AdjacentStops.Keys)
                {
                    int dist = distances[currStop] + this.graph[currStop].AdjacentStops[neighbor];
                    if (dist < distances[neighbor.Name] && distances[currStop] != int.MaxValue)
                    {
                        distances[neighbor.Name] = dist;
                        previous[neighbor.Name] = this.graph[currStop];
                    }
                }
                if (ignoreIteration)
                {
                    distances[stopA] = int.MaxValue;
                    ignoreIteration = false;
                }
            }
            
            return this.FindShortestPathDistance(stopB, previous, distances);
        }

        #endregion

        #region Private

        private int FindDistance(TrainStop startCity, LinkedList<char> route, int distance = 0)
        {
            TrainStop nextCity;
            int nextCityDistance;

            if (route.Count() == 0)
                return distance;
            else
            {
                if (!this.graph.TryGetValue(route.First.Value, out nextCity))
                    throw new PathNotFound(route.First.Value);

                if (!startCity.AdjacentStops.TryGetValue(nextCity, out nextCityDistance))
                    throw new PathNotFound(route.First.Value);
                
                route.RemoveFirst();
                distance += nextCityDistance;

                return this.FindDistance(nextCity, route, distance);
            }
        }
        
        private char FindNextStopWithLeastDistance(List<char> stops, Dictionary<char, int> distances)
        {
            char stopName;
            int minDistance;

            stopName = ' ';
            minDistance = int.MaxValue;
            foreach (char stop in stops)
            {
                if (minDistance >= distances[stop])
                {
                    minDistance = distances[stop];
                    stopName = stop;
                }
            }

            return stopName;
        }

        private int FindNumTripsWithDistanceLessThanN(int maxDistance, int currentDistance, int count, TrainStop currentCity, char endCity)
        {
            Dictionary<TrainStop, int> adjacentCities;
            adjacentCities = currentCity.AdjacentStops;

            foreach (TrainStop nextStop in adjacentCities.Keys)
            {
                int nextDistance;

                nextDistance = currentDistance + adjacentCities[nextStop];
                if (nextDistance < maxDistance)
                {
                    if (nextStop.Name == endCity)
                        count++;

                    count = this.FindNumTripsWithDistanceLessThanN(maxDistance, nextDistance, count, nextStop, endCity);
                }
            }

            return count;
        }

        private int FindNumTripsWithExactlyNStops(int stops, int nStops, int count, TrainStop currentCity, char endCity)
        {
            Dictionary<TrainStop, int> adjacentCities;
            adjacentCities = currentCity.AdjacentStops;

            if (stops <= nStops)
            {
                foreach (TrainStop nextStop in adjacentCities.Keys)
                {
                    if (nStops == stops && nextStop.Name == endCity)
                        count++;
                    else
                        count = this.FindNumTripsWithExactlyNStops(stops + 1, nStops, count, nextStop, endCity);
                }
            }

            return count;
        }

        private int FindNumTripsWithMaxNStops(int stops, int maxStops, int count, TrainStop currentCity, char endCity)
        {
            Dictionary<TrainStop, int> adjacentCities;
            adjacentCities = currentCity.AdjacentStops;

            if (stops <= maxStops)
            {
                foreach (TrainStop stop in adjacentCities.Keys)
                {
                    if (stop.Name == endCity)
                        count++;

                    count = this.FindNumTripsWithMaxNStops(stops + 1, maxStops, count, stop, endCity);
                }
            }

            return count;
        }

        private int FindShortestPathDistance(char lastStop, Dictionary<char, TrainStop> previous, Dictionary<char, int> distances)
        {
            char currStop;
            LinkedList<char> pathFound;

            currStop = lastStop;
            pathFound = new LinkedList<char>();

            while (previous[currStop] != null)
            {
                pathFound.AddFirst(currStop);
                currStop = previous[currStop].Name;

                if (currStop == lastStop)
                    break;
            }

            pathFound.AddFirst(currStop);

            return this.FindDirectDistance(pathFound);
        }
        
        private void InitShortestRoute(out List<char> stops, out Dictionary<char, int> distances, out Dictionary<char, TrainStop> previous)
        {
            distances = new Dictionary<char, int>();
            previous = new Dictionary<char, TrainStop>();
            stops = new List<char>();

            foreach (char stop in this.graph.Keys)
            {
                distances.Add(stop, int.MaxValue);
                previous.Add(stop, null);
                stops.Add(stop);
            }
        }

        #endregion
    }
}
