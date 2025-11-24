using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Models;

namespace DriverMatching.Tests
{
    [TestFixture]
    public class PriorityQueueMatcherTests
    {
        private PriorityQueueMatcher _matcher = null!;
        private List<Driver> _drivers = null!;
        private Order _order = null!;

        [SetUp]
        public void Setup()
        {
            _matcher = new PriorityQueueMatcher();
            _drivers = new List<Driver>();
            _order = new Order { PickupLocation = new Location(5, 5) };
        }
        

        [Test]
        public void FindNearestDrivers_WithPriorityQueue_ReturnsSortedResults()
        {
            // Arrange
            _drivers.Add(new Driver("d1", 10, 10)); // distance ~7.07
            _drivers.Add(new Driver("d2", 3, 3));   // distance ~2.83
            _drivers.Add(new Driver("d3", 6, 6));   // distance ~1.41
            _drivers.Add(new Driver("d4", 1, 1));   // distance ~5.66

            // Act
            var result = _matcher.FindNearestDrivers(_order, _drivers, 3);

            // Assert - FIXED
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[0].Id, Is.EqualTo("d3")); // Closest
            Assert.That(result[1].Id, Is.EqualTo("d2")); // Second
            Assert.That(result[2].Id, Is.EqualTo("d4")); // Third
        }
    }
}