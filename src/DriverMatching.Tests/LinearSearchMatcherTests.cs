using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Models;



namespace DriverMatching.Tests
{
    [TestFixture]
    public class LinearSearchMatcherTests
    {
        private LinearSearchMatcher _matcher = null!;
        private List<Driver> _drivers = null!;
        private Order _order = null!;

        [SetUp]
        public void Setup()
        {
            _matcher = new LinearSearchMatcher();
            _drivers = new List<Driver>();
            _order = new Order { PickupLocation = new Location(5, 5) };
        }

        
        [Test]
        public void FindNearestDrivers_WithEmptyList_ReturnsEmptyList()
        {
            // Act
            var result = _matcher.FindNearestDrivers(_order, _drivers);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void FindNearestDrivers_WithLessDriversThanRequested_ReturnsAllDrivers()
        {
            // Arrange
            _drivers.Add(new Driver("1", 1, 1));
            _drivers.Add(new Driver("2", 2, 2));

            // Act
            var result = _matcher.FindNearestDrivers(_order, _drivers, 5);

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public void FindNearestDrivers_WithMultipleDrivers_ReturnsNearestOnes()
        {
            // Arrange
            _drivers.Add(new Driver("far", 10, 10));      // Дальний
            _drivers.Add(new Driver("near", 4, 4));       // Близкий
            _drivers.Add(new Driver("medium", 6, 6));     // Средний
            _drivers.Add(new Driver("far2", 15, 15));     // Дальний
            _drivers.Add(new Driver("near2", 5, 5));      // Близкий
            _drivers.Add(new Driver("medium2", 7, 7));    // Средний

            // Act
            var result = _matcher.FindNearestDrivers(_order, _drivers, 3);

            // Assert
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[0].Id, Is.EqualTo("near2")); // Самый близкий
            Assert.That(result[1].Id, Is.EqualTo("near"));  // Второй по близости
            Assert.That(result[2].Id, Is.EqualTo("medium")); // Третий
        }

        [Test]
        public void FindNearestDrivers_WithUnavailableDrivers_IgnoresThem()
        {
            // Arrange
            var availableDriver = new Driver("available", 1, 1) { IsAvailable = true };
            var unavailableDriver = new Driver("unavailable", 2, 2) { IsAvailable = false };

            _drivers.Add(availableDriver);
            _drivers.Add(unavailableDriver);

            // Act
            var result = _matcher.FindNearestDrivers(_order, _drivers);

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo("available"));
        }

        [Test]
        public void FindNearestDrivers_WithNullOrder_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                _matcher.FindNearestDrivers(null!, _drivers));
        }

        [Test]
        public void FindNearestDrivers_WithNullDrivers_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                _matcher.FindNearestDrivers(_order, null!));
        }

        [Test]
        public void FindNearestDrivers_WithInvalidCount_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                _matcher.FindNearestDrivers(_order, _drivers, 0));
        }
    }
}