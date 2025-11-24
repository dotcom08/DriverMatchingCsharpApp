using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Models;

namespace DriverMatching.Tests
{
    [TestFixture]
    public class GridSearchMatcherTests
    {
        private GridSearchMatcher _matcher = null!;
        private List<Driver> _drivers = null!;
        private Order _order = null!;

        [SetUp]
        public void Setup()
        {
            _matcher = new GridSearchMatcher(5);
            _drivers = new List<Driver>();
            _order = new Order { PickupLocation = new Location(10, 10) };
        }

        [Test]
        public void FindNearestDrivers_WithDriversInGrid_ReturnsCorrectDrivers()
        {
            // Arrange
            _drivers.Add(new Driver("in_grid1", 12, 12));  // In grid
            _drivers.Add(new Driver("in_grid2", 8, 8));    // In grid
            _drivers.Add(new Driver("far_away", 50, 50));  // Outside grid

            // Act
            var result = _matcher.FindNearestDrivers(_order, _drivers, 2);

            // Assert - FIXED
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.None.Matches<Driver>(d => d.Id == "far_away"));
        }

        [Test]
        public void FindNearestDrivers_WithMultipleSearchRounds_FindsAllRequired()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
            {
                _drivers.Add(new Driver($"driver_{i}", i * 10, i * 10));
            }

            // Act
            var result = _matcher.FindNearestDrivers(_order, _drivers, 5);

            // Assert
            Assert.That(result, Has.Count.EqualTo(5));
        }
    }
}