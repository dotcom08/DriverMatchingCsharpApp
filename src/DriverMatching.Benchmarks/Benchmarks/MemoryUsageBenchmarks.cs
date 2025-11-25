using BenchmarkDotNet.Attributes;
<<<<<<< HEAD
using BenchmarkDotNet.Jobs;
=======
>>>>>>> feature/benchmark-tests
using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Models;

namespace DriverMatching.Benchmarks.Benchmarks
{
<<<<<<< HEAD
[MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net60)]
=======
    [MemoryDiagnoser]
    [Config(typeof(BenchmarkConfig))]
>>>>>>> feature/benchmark-tests
    public class MemoryUsageBenchmarks
    {
        private List<Driver> _drivers = null!;
        private Order _testOrder = null!;

        [Params(1000, 10000)]
        public int DriverCount { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _testOrder = new Order { PickupLocation = new Location(500, 500) };
            _drivers = GenerateDrivers(DriverCount, 0, 1000);
        }

        private List<Driver> GenerateDrivers(int count, int minCoord, int maxCoord)
        {
            var random = new Random(42);
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

        [Benchmark(Baseline = true)]
        public void LinearSearch_Memory() 
            => new LinearSearchMatcher().FindNearestDrivers(_testOrder, _drivers, 5);

        [Benchmark]
        public void GridSearch_Memory() 
            => new GridSearchMatcher().FindNearestDrivers(_testOrder, _drivers, 5);

        [Benchmark]
        public void KDTree_Memory() 
            => new KDTreeMatcher().FindNearestDrivers(_testOrder, _drivers, 5);

        [Benchmark]
        public void PriorityQueue_Memory() 
            => new PriorityQueueMatcher().FindNearestDrivers(_testOrder, _drivers, 5);
    }
}