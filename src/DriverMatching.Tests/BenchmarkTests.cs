using NUnit.Framework;
using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Models;

namespace DriverMatching.Tests
{
[TestFixture]
    public class BenchmarkTests
    {
        [Test]
        public void AllAlgorithms_ProduceSameResults_WithSameInput()
        {
            // Arrange
            var drivers = new List<Driver>
            {
                new Driver("1", 10, 10),
                new Driver("2", 20, 20),
                new Driver("3", 30, 30),
                new Driver("4", 40, 40),
                new Driver("5", 50, 50),
                new Driver("6", 60, 60)
            };

            var order = new Order { PickupLocation = new Location(25, 25) };

            var linearMatcher = new LinearSearchMatcher();
            var gridMatcher = new GridSearchMatcher();
            var kdTreeMatcher = new KDTreeMatcher();
            var priorityQueueMatcher = new PriorityQueueMatcher();

            // Act
            var linearResult = linearMatcher.FindNearestDrivers(order, drivers, 3);
            var gridResult = gridMatcher.FindNearestDrivers(order, drivers, 3);
            var kdTreeResult = kdTreeMatcher.FindNearestDrivers(order, drivers, 3);
            var priorityQueueResult = priorityQueueMatcher.FindNearestDrivers(order, drivers, 3);

            // Assert - All algorithms should return the same drivers (order might differ for equal distances)
            Assert.That(linearResult.Count, Is.EqualTo(3));
            Assert.That(gridResult.Count, Is.EqualTo(3));
            Assert.That(kdTreeResult.Count, Is.EqualTo(3));
            Assert.That(priorityQueueResult.Count, Is.EqualTo(3));

            // Check that all algorithms found the same set of drivers
            var linearIds = linearResult.Select(d => d.Id).ToHashSet();
            var gridIds = gridResult.Select(d => d.Id).ToHashSet();
            var kdTreeIds = kdTreeResult.Select(d => d.Id).ToHashSet();
            var priorityQueueIds = priorityQueueResult.Select(d => d.Id).ToHashSet();

            Assert.That(linearIds.SetEquals(gridIds));
            Assert.That(linearIds.SetEquals(kdTreeIds));
            Assert.That(linearIds.SetEquals(priorityQueueIds));
        }

        [Test]
        public void Algorithms_HandleLargeDatasets_WithoutErrors()
        {
            // Arrange
            var largeDrivers = GenerateLargeDriverSet(1000);
            var order = new Order { PickupLocation = new Location(500, 500) };

            var matchers = new List<IDriverMatcher>
            {
                new LinearSearchMatcher(),
                new GridSearchMatcher(),
                new KDTreeMatcher(),
                new PriorityQueueMatcher()
            };

            foreach (var matcher in matchers)
            {
                // Act & Assert - Should not throw exceptions
                Assert.DoesNotThrow(() =>
                {
                    var result = matcher.FindNearestDrivers(order, largeDrivers, 5);
                    Assert.That(result.Count, Is.EqualTo(5));
                });
            }
        }

        private List<Driver> GenerateLargeDriverSet(int count)
        {
            var random = new Random(42);
            var drivers = new List<Driver>();

            for (int i = 0; i < count; i++)
            {
                drivers.Add(new Driver(
                    $"driver_{i}",
                    random.Next(0, 1000),
                    random.Next(0, 1000)
                ));
            }

            return drivers;
        }
    }
}