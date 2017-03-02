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

        private Dictionary<char, City> cities;

        #endregion

        #region Constructor

        public Map()
        {
            this.cities = new Dictionary<char, City>();
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
            this.addCity(startName).routes.Add(this.addCity(endName), distance);
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
            City startCity;

            if (route.Count() < 2)
            {
                throw new ArgumentException("To calulate a route, there must be at least two cities.");
            }

            routeCopy = new LinkedList<char>(route);
            startCity = this.cities[route.First.Value];
            route.RemoveFirst();

            return FindDistance(startCity, route);
        }

        private int FindDistance(City startCity, LinkedList<char> route, int distance = 0)
        {
            City nextCity;
            int nextCityDistance;

            if (route.Count() == 0)
            {
                return distance;
            }
            else
            {
                if (!this.cities.TryGetValue(route.First.Value, out nextCity))
                {
                    throw new ArgumentOutOfRangeException();
                }
                
                if (!startCity.routes.TryGetValue(nextCity, out nextCityDistance))
                {
                    throw new ArgumentOutOfRangeException();
                }

                route.RemoveFirst();
                distance += nextCityDistance;

                return this.FindDistance(nextCity, route, distance);
            }
        }

        public bool HasCity(char cityName)
        {
            return this.cities.ContainsKey(cityName);
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
            City currentCity;

            currentCity = this.cities[startCity];

            return this.FindNumTripsWithMaxNStops(0, nStops, 0, currentCity, endCity);
        }

        private int FindNumTripsWithMaxNStops(int stops, int maxStops, int count, City currentCity, char endCity)
        {
            Dictionary<City, int> adjacentCities;
            adjacentCities = currentCity.routes;

            if (stops <= maxStops)
            {
                foreach (City city in adjacentCities.Keys)
                {
                    if (city.name == endCity)
                    {
                        count++;
                    }
                    else
                    {
                        count = this.FindNumTripsWithMaxNStops(++stops, maxStops, count, city, endCity);
                    }
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
        public int numTripsWithExactlyNStops(int nStops, char startCity, char endCity)
        {
            City currentCity;
            currentCity = this.cities[startCity];

            return this.FindNumTripsWithMaxNStops(0, nStops, 0, currentCity, endCity);
        }

        private int findNumTripsWithExactlyNStops(int stops, int nStops, int count, City currentCity, char endCity)
        {
            Dictionary<City, int> adjacentCities;
            adjacentCities = currentCity.routes;

            if (stops <= nStops)
            {
                foreach (City city in adjacentCities.Keys)
                {
                    if (nStops == stops && city.name == endCity)
                    {
                        count++;
                    }
                    else
                    {
                        count = this.findNumTripsWithExactlyNStops(++stops, nStops, count, city, endCity);
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
        /// <returns></returns>
        public int numTripsWithDistanceLessThan(int distance, char startCity, char endCity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private

        /// <summary>
        /// Adds the city with name <paramref name="cityName"/> to the map if 
        /// it does not already exist.
        /// </summary>
        /// <param name="cityName"></param>
        private City addCity(char cityName)
        {
            City newCity;
            if (!this.cities.TryGetValue(cityName, out newCity))
            {
                newCity = new City(cityName);
                this.cities.Add(cityName, newCity);
            }
            return newCity;
        }

        #endregion
    }
}
