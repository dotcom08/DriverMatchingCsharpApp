using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverMatching.Core.Models;

namespace DriverMatching.Core.Algorithms
{
    public class GridSearchMatcher : IDriverMatcher
    {
        private readonly int _gridSize;

        public string AlgorithmName => "Grid Search";

        public GridSearchMatcher(int gridSize = 10)
        {
            _gridSize = gridSize;
        }

        public List<Driver> FindNearestDrivers(Order order, List<Driver> drivers, int count = 5)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (drivers == null) throw new ArgumentNullException(nameof(drivers));
            if (count <= 0) throw new ArgumentException("Count must be positive", nameof(count));

            var availableDrivers = drivers.Where(d => d.IsAvailable).ToList();

            if (availableDrivers.Count <= count)
                return availableDrivers;

            // Поиск в расширяющейся сетке
            var nearestDrivers = new List<Driver>();
            var searchRadius = 0;

            while (nearestDrivers.Count < count && searchRadius < 100) // ограничиваем радиус поиска
            {
                var candidates = SearchInGrid(order.PickupLocation, availableDrivers, searchRadius);
                nearestDrivers.AddRange(candidates.Take(count - nearestDrivers.Count));
                searchRadius++;
            }

            return nearestDrivers.Take(count).ToList();
        }

        private IEnumerable<Driver> SearchInGrid(Location center, List<Driver> drivers, int radius)
        {
            var minX = center.X - radius * _gridSize;
            var maxX = center.X + radius * _gridSize;
            var minY = center.Y - radius * _gridSize;
            var maxY = center.Y + radius * _gridSize;

            return drivers
                .Where(d => d.Location.X >= minX && d.Location.X <= maxX &&
                           d.Location.Y >= minY && d.Location.Y <= maxY)
                .OrderBy(d => d.Location.DistanceTo(center));
        }
    }
}