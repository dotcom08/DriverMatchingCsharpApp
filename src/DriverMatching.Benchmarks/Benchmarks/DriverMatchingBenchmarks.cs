using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Models;


namespace DriverMatching.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net60)]
    [RankColumn]
    public class DriverMatchingBenchmarks
    {
        private List<Driver> _smallDrivers = null!;
        private List<Driver> _mediumDrivers = null!;
        private List<Driver> _largeDrivers = null!;
        private List<Driver> _veryLargeDrivers = null!;
        
        private Order _testOrder = null!;
        
        private LinearSearchMatcher _linearMatcher = null!;
        private GridSearchMatcher _gridMatcher = null!;
        private KDTreeMatcher _kdTreeMatcher = null!;
        private PriorityQueueMatcher _priorityQueueMatcher = null!;

        [GlobalSetup]
        public void GlobalSetup()
        {
            // Initialize matchers
            _linearMatcher = new LinearSearchMatcher();
            _gridMatcher = new GridSearchMatcher();
            _kdTreeMatcher = new KDTreeMatcher();
            _priorityQueueMatcher = new PriorityQueueMatcher();

            // Create test order
            _testOrder = new Order { PickupLocation = new Location(50, 50) };

            // Generate test data
            _smallDrivers = GenerateDrivers(100, 0, 100);
            _mediumDrivers = GenerateDrivers(1_000, 0, 200);
            _largeDrivers = GenerateDrivers(10_000, 0, 500);
            _veryLargeDrivers = GenerateDrivers(100_000, 0, 1000);
        }

        private List<Driver> GenerateDrivers(int count, int minCoord, int maxCoord)
        {
            var random = new Random(42); // Fixed seed for consistent results
            var drivers = new List<Driver>();

            for (int i = 0; i < count; i++)
            {
                drivers.Add(new Driver(
                    $"driver_{i}",
                    random.Next(minCoord, maxCoord),
                    random.Next(minCoord, maxCoord)
                ));
            }

            return drivers;
        }

        // Small dataset benchmarks (100 drivers)
        [Benchmark]
        public void LinearSearch_Small() 
            => _linearMatcher.FindNearestDrivers(_testOrder, _smallDrivers, 5);

        [Benchmark]
        public void GridSearch_Small() 
            => _gridMatcher.FindNearestDrivers(_testOrder, _smallDrivers, 5);

        [Benchmark]
        public void KDTree_Small() 
            => _kdTreeMatcher.FindNearestDrivers(_testOrder, _smallDrivers, 5);

        [Benchmark]
        public void PriorityQueue_Small() 
            => _priorityQueueMatcher.FindNearestDrivers(_testOrder, _smallDrivers, 5);

        // Medium dataset benchmarks (1,000 drivers)
        [Benchmark]
        public void LinearSearch_Medium() 
            => _linearMatcher.FindNearestDrivers(_testOrder, _mediumDrivers, 5);

        [Benchmark]
        public void GridSearch_Medium() 
            => _gridMatcher.FindNearestDrivers(_testOrder, _mediumDrivers, 5);

        [Benchmark]
        public void KDTree_Medium() 
            => _kdTreeMatcher.FindNearestDrivers(_testOrder, _mediumDrivers, 5);

        [Benchmark]
        public void PriorityQueue_Medium() 
            => _priorityQueueMatcher.FindNearestDrivers(_testOrder, _mediumDrivers, 5);

        // Large dataset benchmarks (10,000 drivers)
        [Benchmark]
        public void LinearSearch_Large() 
            => _linearMatcher.FindNearestDrivers(_testOrder, _largeDrivers, 5);

        [Benchmark]
        public void GridSearch_Large() 
            => _gridMatcher.FindNearestDrivers(_testOrder, _largeDrivers, 5);

        [Benchmark]
        public void KDTree_Large() 
            => _kdTreeMatcher.FindNearestDrivers(_testOrder, _largeDrivers, 5);

        [Benchmark]
        public void PriorityQueue_Large() 
            => _priorityQueueMatcher.FindNearestDrivers(_testOrder, _largeDrivers, 5);

        // Very large dataset benchmarks (100,000 drivers)
        [Benchmark]
        public void LinearSearch_VeryLarge() 
            => _linearMatcher.FindNearestDrivers(_testOrder, _veryLargeDrivers, 5);

        [Benchmark]
        public void GridSearch_VeryLarge() 
            => _gridMatcher.FindNearestDrivers(_testOrder, _veryLargeDrivers, 5);

        [Benchmark]
        public void KDTree_VeryLarge() 
            => _kdTreeMatcher.FindNearestDrivers(_testOrder, _veryLargeDrivers, 5);

        [Benchmark]
        public void PriorityQueue_VeryLarge() 
            => _priorityQueueMatcher.FindNearestDrivers(_testOrder, _veryLargeDrivers, 5);
    
    }
}