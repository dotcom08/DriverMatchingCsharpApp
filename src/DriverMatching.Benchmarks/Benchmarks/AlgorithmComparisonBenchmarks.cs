using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Models;


namespace DriverMatching.Benchmarks.Benchmarks
{

    [MemoryDiagnoser]
    [RankColumn]
    [Config(typeof(BenchmarkConfig))]
    public class AlgorithmComparisonBenchmarks
    {
        private List<Driver> _drivers = null!;
        private List<Order> _orders = null!;
        
        private LinearSearchMatcher _linearMatcher = null!;
        private GridSearchMatcher _gridMatcher = null!;
        private KDTreeMatcher _kdTreeMatcher = null!;
        private PriorityQueueMatcher _priorityQueueMatcher = null!;

        [Params(100, 1000, 10000)]
        public int DriverCount { get; set; }

        [Params(1, 5, 10)]
        public int NearestDriversCount { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _linearMatcher = new LinearSearchMatcher();
            _gridMatcher = new GridSearchMatcher();
            _kdTreeMatcher = new KDTreeMatcher();
            _priorityQueueMatcher = new PriorityQueueMatcher();

            _drivers = GenerateDrivers(DriverCount, 0, 1000);
            _orders = GenerateOrders(10);
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

        private List<Order> GenerateOrders(int count)
        {
            var random = new Random(24);
            var orders = new List<Order>();

            for (int i = 0; i < count; i++)
            {
                orders.Add(new Order
                {
                    Id = $"order_{i}",
                    PickupLocation = new Location(random.Next(0, 1000), random.Next(0, 1000))
                });
            }

            return orders;
        }

        [Benchmark]
        public void LinearSearch_MultipleOrders()
        {
            foreach (var order in _orders)
            {
                _linearMatcher.FindNearestDrivers(order, _drivers, NearestDriversCount);
            }
        }

        [Benchmark]
        public void GridSearch_MultipleOrders()
        {
            foreach (var order in _orders)
            {
                _gridMatcher.FindNearestDrivers(order, _drivers, NearestDriversCount);
            }
        }

        [Benchmark]
        public void KDTree_MultipleOrders()
        {
            foreach (var order in _orders)
            {
                _kdTreeMatcher.FindNearestDrivers(order, _drivers, NearestDriversCount);
            }
        }

        [Benchmark]
        public void PriorityQueue_MultipleOrders()
        {
            foreach (var order in _orders)
            {
                _priorityQueueMatcher.FindNearestDrivers(order, _drivers, NearestDriversCount);
            }
        }
    }
}