using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Models;

namespace DriverMatching.Tests
{
    [TestFixture]
    public class KDTreeMatcherTests
    {
        private KDTreeMatcher _matcher = null!;
        private List<Driver> _drivers = null!;
        private Order _order = null!;

        [SetUp]
        public void Setup()
        {
            _matcher = new KDTreeMatcher();
            _drivers = new List<Driver>();
            _order = new Order { PickupLocation = new Location(5, 5) };
        }

        [Test]
        public void FindNearestDrivers_WithKDTree_ReturnsCorrectNearestDrivers()
        {
            // Arrange
            _drivers.Add(new Driver("far", 20, 20));
            _drivers.Add(new Driver("near1", 4, 4));
            _drivers.Add(new Driver("near2", 6, 6));
            _drivers.Add(new Driver("medium", 10, 10));

            // Act
            var result = _matcher.FindNearestDrivers(_order, _drivers, 2);

            // Assert - FIXED: Use Contains and correct assertion syntax
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Some.Matches<Driver>(d => d.Id == "near1"));
            Assert.That(result, Has.Some.Matches<Driver>(d => d.Id == "near2"));
        }

        [Test]
        public void FindNearestDrivers_WithLargeDataset_ReturnsCorrectResults()
        {
            // Arrange
            var random = new Random(42); // Fixed seed for reproducibility
            for (int i = 0; i < 100; i++)
            {
                _drivers.Add(new Driver($"driver_{i}", random.Next(0, 100), random.Next(0, 100)));
            }

            // Act
            var result = _matcher.FindNearestDrivers(_order, _drivers, 5);

            // Assert
            Assert.That(result, Has.Count.EqualTo(5));
            
            // Check that results are sorted by distance
            var distances = result.Select(d => d.Location.DistanceTo(_order.PickupLocation)).ToList();
            for (int i = 0; i < distances.Count - 1; i++)
            {
                Assert.That(distances[i], Is.LessThanOrEqualTo(distances[i + 1]));
            }
        }
    }
}