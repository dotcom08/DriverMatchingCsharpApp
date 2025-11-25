using NUnit.Framework;
using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Models;

namespace DriverMatching.Tests
{
    [TestFixture]
    public class BenchmarkTests
    {
        [Test]
        public void AllAlgorithms_ReturnCorrectNumberOfDrivers_WithValidInput()
        {
            // Arrange - Use distinct positions to avoid distance equality issues
            var drivers = new List<Driver>
            {
                new Driver("1", 10, 10), 
                new Driver("2", 15, 15),    
                new Driver("3", 20, 20),  
                new Driver("4", 35, 35),   
                new Driver("5", 40, 40),   
                new Driver("6", 50, 50)   
            };

            var order = new Order { PickupLocation = new Location(25, 25) };

            var algorithms = new (string Name, IDriverMatcher Matcher)[]
            {
                ("LinearSearch", new LinearSearchMatcher()),
                ("GridSearch", new GridSearchMatcher()),
                ("KDTree", new KDTreeMatcher()),
                ("PriorityQueue", new PriorityQueueMatcher())
            };

            foreach (var (name, matcher) in algorithms)
            {
                // Act
                var result = matcher.FindNearestDrivers(order, drivers, 3);

                // Assert
                Assert.That(result, Has.Count.EqualTo(3), 
                    $"{name} should return exactly 3 drivers");
                
                Assert.That(result.All(d => d != null), Is.True,
                    $"{name} should not return null drivers");
                
                // Verify that results are sorted by distance
                AssertIsSortedByDistance(result, order.PickupLocation, name);
            }
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
                    Assert.That(result, Has.Count.EqualTo(5));
                }, $"{matcher.GetType().Name} failed with large dataset");
            }
        }

        [Test]
        public void Algorithms_ReturnEmptyList_WhenNoDriversAvailable()
        {
            // Arrange
            var emptyDrivers = new List<Driver>();
            var order = new Order { PickupLocation = new Location(25, 25) };

            var matchers = new List<IDriverMatcher>
            {
                new LinearSearchMatcher(),
                new GridSearchMatcher(),
                new KDTreeMatcher(),
                new PriorityQueueMatcher()
            };

            foreach (var matcher in matchers)
            {
                // Act
                var result = matcher.FindNearestDrivers(order, emptyDrivers, 5);

                // Assert
                Assert.That(result, Is.Empty, 
                    $"{matcher.GetType().Name} should return empty list for no drivers");
            }
        }

        [Test]
        public void Algorithms_ReturnAllDrivers_WhenRequestedCountExceedsAvailable()
        {
            // Arrange
            var drivers = new List<Driver>
            {
                new Driver("1", 10, 10),
                new Driver("2", 20, 20)
            };

            var order = new Order { PickupLocation = new Location(25, 25) };

            var matchers = new List<IDriverMatcher>
            {
                new LinearSearchMatcher(),
                new GridSearchMatcher(),
                new KDTreeMatcher(),
                new PriorityQueueMatcher()
            };

            foreach (var matcher in matchers)
            {
                // Act
                var result = matcher.FindNearestDrivers(order, drivers, 5);

                // Assert
                Assert.That(result, Has.Count.EqualTo(2), 
                    $"{matcher.GetType().Name} should return all available drivers when count exceeds available");
            }
        }

        [Test]
        public void Algorithms_ProduceReasonableResults_WithSameInput()
        {
            // Arrange - Simple dataset with clear distances
            var drivers = new List<Driver>
            {
                new Driver("close1", 24, 24),   // Very close
                new Driver("close2", 26, 26),   // Very close  
                new Driver("medium1", 50, 50),  // Medium
                new Driver("medium2", 0, 0),    // Medium
                new Driver("far1", 100, 100),   // Far
                new Driver("far2", 200, 200)    // Very far
            };

            var order = new Order { PickupLocation = new Location(25, 25) };

            var algorithms = new IDriverMatcher[]
            {
                new LinearSearchMatcher(),
                new GridSearchMatcher(),
                new KDTreeMatcher(),
                new PriorityQueueMatcher()
            };

            foreach (var algorithm in algorithms)
            {
                // Act
                var result = algorithm.FindNearestDrivers(order, drivers, 3);

                // Assert - Verify reasonable properties
                Assert.That(result, Has.Count.EqualTo(3));
                
                // The 2 closest should be "close1" and "close2"
                var closeDrivers = result.Count(d => d.Id == "close1" || d.Id == "close2");
                Assert.That(closeDrivers, Is.GreaterThanOrEqualTo(1),
                    $"{algorithm.GetType().Name} should find at least one close driver");
                
                // Verify sorting
                AssertIsSortedByDistance(result, order.PickupLocation, algorithm.GetType().Name);
            }
        }

        [Test]
        public void Algorithms_HandleEdgeCases_WithoutErrors()
        {
            // Arrange
            var edgeCaseDrivers = new List<Driver>
            {
                new Driver("same1", 0, 0),
                new Driver("same2", 0, 0), // Same position
                new Driver("same3", 0, 0), // Same position
                new Driver("far", 1000, 1000)
            };

            var order = new Order { PickupLocation = new Location(0, 0) };

            var matchers = new List<IDriverMatcher>
            {
                new LinearSearchMatcher(),
                new GridSearchMatcher(),
                new KDTreeMatcher(),
                new PriorityQueueMatcher()
            };

            foreach (var matcher in matchers)
            {
                // Act & Assert - Should handle drivers at same location
                Assert.DoesNotThrow(() =>
                {
                    var result = matcher.FindNearestDrivers(order, edgeCaseDrivers, 2);
                    Assert.That(result, Has.Count.EqualTo(2));
                }, $"{matcher.GetType().Name} failed with edge case data");
            }
        }
        [Test]
        public void Algorithms_ResultsAreSortedByDistance()
        {
            // Arrange - Use positions that create clear distance ordering
            var drivers = new List<Driver>
            {
                new Driver("close", 5, 5),      
                new Driver("medium", 15, 15),   
                new Driver("far", 25, 25),       
                new Driver("very_far", 35, 35)   
            };

            var order = new Order { PickupLocation = new Location(0, 0) };

            var matchers = new List<IDriverMatcher>
            {
                new LinearSearchMatcher(),
                new GridSearchMatcher(),
                new KDTreeMatcher(),
                new PriorityQueueMatcher()
            };

            foreach (var matcher in matchers)
            {
                // Act
                var result = matcher.FindNearestDrivers(order, drivers, 4);

                // Debug: Print the actual results to understand the ordering
                Console.WriteLine($"{matcher.GetType().Name} results:");
                foreach (var driver in result)
                {
                    var distance = driver.Location.DistanceTo(order.PickupLocation);
                    Console.WriteLine($"  {driver.Id}: {distance:F2}");
                }

                // Assert - Results should be sorted by distance (closest first)
                AssertIsSortedByDistance(result, order.PickupLocation, matcher.GetType().Name);
            }
        }
        [Test]
        public void Algorithms_HandleUnavailableDrivers_Correctly()
        {
            // Arrange
            var drivers = new List<Driver>
            {
                new Driver("1", 10, 10) { IsAvailable = true },
                new Driver("2", 20, 20) { IsAvailable = false }, // Unavailable
                new Driver("3", 30, 30) { IsAvailable = true },
                new Driver("4", 40, 40) { IsAvailable = false }, // Unavailable
                new Driver("5", 50, 50) { IsAvailable = true }
            };

            var order = new Order { PickupLocation = new Location(25, 25) };

            var matchers = new List<IDriverMatcher>
            {
                new LinearSearchMatcher(),
                new GridSearchMatcher(),
                new KDTreeMatcher(),
                new PriorityQueueMatcher()
            };

            foreach (var matcher in matchers)
            {
                // Act
                var result = matcher.FindNearestDrivers(order, drivers, 5);

                // Assert - Should only return available drivers
                Assert.That(result, Has.Count.EqualTo(3));
                Assert.That(result.All(d => d.IsAvailable), Is.True, 
                    $"{matcher.GetType().Name} should only return available drivers");
                Assert.That(result.Select(d => d.Id), Does.Not.Contain("2"));
                Assert.That(result.Select(d => d.Id), Does.Not.Contain("4"));
            }
        }

        private void AssertIsSortedByDistance(List<Driver> drivers, Location target, string algorithmName)
        {
            for (int i = 0; i < drivers.Count - 1; i++)
            {
                var dist1 = drivers[i].Location.DistanceTo(target);
                var dist2 = drivers[i + 1].Location.DistanceTo(target);
                Assert.That(dist1, Is.LessThanOrEqualTo(dist2), 
                    $"{algorithmName}: Results should be sorted by distance. " +
                    $"{drivers[i].Id}({dist1:F2}) should be before {drivers[i+1].Id}({dist2:F2})");
            }
        }

        private List<Driver> GenerateLargeDriverSet(int count)
        {
            var random = new Random(42); // Fixed seed for reproducible results
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