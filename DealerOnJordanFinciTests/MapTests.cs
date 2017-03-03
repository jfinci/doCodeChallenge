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
        public void AddRouteTest()
        {
            char startstop, endstop;
            Map map;

            startstop = 'A';
            endstop = 'B';
            map = new Map();

            map.AddRoute(startstop, endstop, 5);

            Assert.IsTrue(map.HasStop(startstop) && map.HasStop(endstop));
        }

        [TestMethod()]
        public void FindDirectDistanceTest()
        {
            char stopA, stopB, stopC, stopD;
            Map map;
            LinkedList<char> route;

            stopA = 'A';
            stopB = 'B';
            stopC = 'C';
            stopD = 'D';

            route = new LinkedList<char>(new char[] { stopA, stopB, stopC, stopD });

            map = new Map();

            map.AddRoute(stopA, stopB, 5);
            route = new LinkedList<char> (new char[] { stopA, stopB });
            Assert.AreEqual(map.FindDirectDistance(route), 5);

            map.AddRoute(stopB, stopC, 2);
            route = new LinkedList<char>(new char[] { stopA, stopB, stopC });
            Assert.AreEqual(map.FindDirectDistance(route), 7);

            map.AddRoute(stopC, stopD, 10);
            route = new LinkedList<char>(new char[] { stopA, stopB, stopC, stopD });
            Assert.AreEqual(map.FindDirectDistance(route), 17);
        }

        [TestMethod()]
        public void NumTripsWithMaxNStopsTest()
        {
            char stopA, stopB, stopC, stopD;
            Map map;

            stopA = 'A';
            stopB = 'B';
            stopC = 'C';
            stopD = 'D';

            map = new Map();

            map.AddRoute(stopA, stopB, 5);
            Assert.AreEqual(1, map.NumTripsWithMaxNStops(1, stopA, stopB));

            map.AddRoute(stopB, stopC, 5);
            Assert.AreEqual(1, map.NumTripsWithMaxNStops(1, stopA, stopC));

            map.AddRoute(stopC, stopD, 5);
            Assert.AreEqual(0, map.NumTripsWithMaxNStops(1, stopA, stopD));
            Assert.AreEqual(1, map.NumTripsWithMaxNStops(2, stopA, stopD));

            map.AddRoute(stopA, stopD, 5);
            Assert.AreEqual(1, map.NumTripsWithMaxNStops(1, stopA, stopD));
            Assert.AreEqual(2, map.NumTripsWithMaxNStops(2, stopA, stopD));
            Assert.AreEqual(2, map.NumTripsWithMaxNStops(10, stopA, stopD));

            map.AddRoute(stopB, stopD, 5);
            Assert.AreEqual(3, map.NumTripsWithMaxNStops(2, stopA, stopD));

            map.AddRoute(stopC, stopB, 5);
            Assert.AreEqual(5, map.NumTripsWithMaxNStops(4, stopA, stopD));

            map = new Map();

            map.AddRoute(stopA, stopB, 1);
            map.AddRoute(stopB, stopC, 1);
            map.AddRoute(stopC, stopA, 1);
            Assert.AreEqual(2, map.NumTripsWithMaxNStops(3, stopA, stopB));
        }

        [TestMethod()]
        public void NumTripsWithExactlyNStopsTest()
        {
            char stopA, stopB, stopC, stopD, stopE;
            Map map;

            stopA = 'A';
            stopB = 'B';
            stopC = 'C';
            stopD = 'D';
            stopE = 'E';

            map = new Map();

            map.AddRoute(stopA, stopB, 5);
            Assert.AreEqual(1, map.NumTripsWithExactlyNStops(0, stopA, stopB));
            Assert.AreEqual(0, map.NumTripsWithExactlyNStops(1, stopA, stopB));

            map.AddRoute(stopB, stopC, 5);
            Assert.AreEqual(1, map.NumTripsWithExactlyNStops(1, stopA, stopC));
            Assert.AreEqual(1, map.NumTripsWithExactlyNStops(0, stopB, stopC));
            Assert.AreEqual(0, map.NumTripsWithExactlyNStops(2, stopA, stopC));

            map.AddRoute(stopC, stopD, 5);
            Assert.AreEqual(0, map.NumTripsWithExactlyNStops(1, stopA, stopD));
            Assert.AreEqual(1, map.NumTripsWithExactlyNStops(2, stopA, stopD));
            Assert.AreEqual(0, map.NumTripsWithExactlyNStops(3, stopA, stopD));

            map = new Map();

            map.AddRoute(stopA, stopB, 1);
            map.AddRoute(stopB, stopD, 1);
            map.AddRoute(stopA, stopC, 1);
            map.AddRoute(stopC, stopD, 1);
            map.AddRoute(stopA, stopE, 1);
            map.AddRoute(stopC, stopE, 1);
            map.AddRoute(stopD, stopE, 1);
            map.AddRoute(stopE, stopD, 1);

            Assert.AreEqual(3, map.NumTripsWithExactlyNStops(1, stopA, stopD));
            Assert.AreEqual(1, map.NumTripsWithExactlyNStops(2, stopA, stopD));

            map.AddRoute(stopE, stopB, 1);
            Assert.AreEqual(2, map.NumTripsWithExactlyNStops(2, stopA, stopD));
        }

        [TestMethod()]
        public void NumTripsWithDistanceLessThanTest()
        {
            char stopA, stopB, stopC, stopD, stopE;
            Map map;

            stopA = 'A';
            stopB = 'B';
            stopC = 'C';
            stopD = 'D';
            stopE = 'E';

            map = new Map();

            map.AddRoute(stopA, stopB, 5);
            Assert.AreEqual(0, map.NumTripsWithDistanceLessThanN(5, stopA, stopB));
            Assert.AreEqual(1, map.NumTripsWithDistanceLessThanN(6, stopA, stopB));

            map.AddRoute(stopB, stopC, 1);
            map.AddRoute(stopC, stopA, 1);
            Assert.AreEqual(2, map.NumTripsWithDistanceLessThanN(13, stopA, stopB));
        }

        [TestMethod()]
        public void shortestRoute()
        {
            char stopA, stopB, stopC, stopD, stopE;
            Map map;

            stopA = 'A';
            stopB = 'B';
            stopC = 'C';
            stopD = 'D';
            stopE = 'E';

            map = new Map();

            map.AddRoute(stopA, stopB, 5);
            Assert.AreEqual(5, map.ShortestRoute(stopA, stopB));

            map.AddRoute(stopA, stopC, 15);
            map.AddRoute(stopB, stopC, 1);
            Assert.AreEqual(6, map.ShortestRoute(stopA, stopC));
        }
    }
}