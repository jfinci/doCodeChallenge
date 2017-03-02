using Microsoft.VisualStudio.TestTools.UnitTesting;
using DealerOnJordanFinci;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerOnJordanFinci.Tests
{
    [TestClass()]
    public class MapTests
    {
        [TestMethod()]
        public void addRouteTest()
        {
            char startCity, endCity;
            Map map;

            startCity = 'A';
            endCity = 'B';
            map = new Map();

            map.AddRoute(startCity, endCity, 5);

            Assert.IsTrue(map.HasCity(startCity) && map.HasCity(endCity));
        }

        [TestMethod()]
        public void findDirectDistanceTest()
        {
            char cityA, cityB, cityC, cityD;
            Map map;
            LinkedList<char> route;

            cityA = 'A';
            cityB = 'B';
            cityC = 'C';
            cityD = 'D';

            route = new LinkedList<char>(new char[] { cityA, cityB, cityC, cityD });

            map = new Map();

            map.AddRoute(cityA, cityB, 5);
            route = new LinkedList<char> (new char[] { cityA, cityB });
            Assert.AreEqual(map.FindDirectDistance(route), 5);

            map.AddRoute(cityB, cityC, 2);
            route = new LinkedList<char>(new char[] { cityA, cityB, cityC });
            Assert.AreEqual(map.FindDirectDistance(route), 7);

            map.AddRoute(cityC, cityD, 10);
            route = new LinkedList<char>(new char[] { cityA, cityB, cityC, cityD });
            Assert.AreEqual(map.FindDirectDistance(route), 17);
        }

        [TestMethod()]
        public void numTripsWithMaxNStopsTest()
        {
            char cityA, cityB, cityC, cityD;
            Map map;
            LinkedList<char> route;

            cityA = 'A';
            cityB = 'B';
            cityC = 'C';
            cityD = 'D';

            route = new LinkedList<char>(new char[] { cityA, cityB, cityC, cityD });

            map = new Map();

            map.AddRoute(cityA, cityB, 5);
            Assert.AreEqual(map.NumTripsWithMaxNStops(1, cityA, cityB), 1);
            Assert.AreEqual(map.NumTripsWithMaxNStops(1, cityB, cityA), 0);

            map.AddRoute(cityB, cityC, 5);
            Assert.AreEqual(map.NumTripsWithMaxNStops(1, cityA, cityC), 1);

            map.AddRoute(cityC, cityD, 5);
            Assert.AreEqual(map.NumTripsWithMaxNStops(1, cityA, cityD), 0);
            Assert.AreEqual(map.NumTripsWithMaxNStops(2, cityA, cityD), 1);

            map.AddRoute(cityA, cityD, 5);
            Assert.AreEqual(map.NumTripsWithMaxNStops(1, cityA, cityD), 1);
            Assert.AreEqual(map.NumTripsWithMaxNStops(2, cityA, cityD), 2);
            Assert.AreEqual(map.NumTripsWithMaxNStops(10, cityA, cityD), 2);

            map.AddRoute(cityB, cityD, 5);
            Assert.AreEqual(map.NumTripsWithMaxNStops(2, cityA, cityD), 3);

            map.AddRoute(cityC, cityB, 5);
            Assert.AreEqual(map.NumTripsWithMaxNStops(4, cityA, cityD), 5);
        }

        //[TestMethod()]
        //public void numTripsWithExactlyNStopsTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void numTripsWithDistanceLessThanTest()
        //{
        //    Assert.Fail();
        //}
    }
}