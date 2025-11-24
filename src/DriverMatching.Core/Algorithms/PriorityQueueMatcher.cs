using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverMatching.Core.Models;

namespace DriverMatching.Core.Algorithms
{
    public class PriorityQueueMatcher : IDriverMatcher
    {
        public string AlgorithmName => "Priority Queue";

        public List<Driver> FindNearestDrivers(Order order, List<Driver> drivers, int count = 5)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (drivers == null) throw new ArgumentNullException(nameof(drivers));
            if (count <= 0) throw new ArgumentException("Count must be positive", nameof(count));

            var availableDrivers = drivers.Where(d => d.IsAvailable).ToList();

            if (availableDrivers.Count <= count)
                return availableDrivers;

            // Используем PriorityQueue для эффективного поиска ближайших
            var queue = new PriorityQueue<Driver, double>();

            foreach (var driver in availableDrivers)
            {
                var distance = driver.Location.DistanceTo(order.PickupLocation);
                queue.Enqueue(driver, distance);
            }

            var result = new List<Driver>();
            for (int i = 0; i < count && queue.Count > 0; i++)
            {
                result.Add(queue.Dequeue());
            }

            return result;
        }
    }
}