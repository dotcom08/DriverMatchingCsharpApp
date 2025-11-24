using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverMatching.Core.Models;

namespace DriverMatching.Core.Algorithms
{
    public class LinearSearchMatcher : IDriverMatcher
    {
        public string AlgorithmName => "Linear Search";

        public List<Driver> FindNearestDrivers(Order order, List<Driver> drivers, int count = 5)
        {
            //parameters validation
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (drivers == null) throw new ArgumentNullException(nameof(drivers));
            if (count <= 0) throw new ArgumentException("Count must be positive", nameof(count));

            var availableDrivers = drivers.Where(d => d.IsAvailable).ToList();

            if (availableDrivers.Count <= count)
                return availableDrivers;

            // Сортируем по расстоянию и берем ближайших
            return availableDrivers
                .OrderBy(d => d.Location.DistanceTo(order.PickupLocation))
                .Take(count)
                .ToList();
        }
    }
}