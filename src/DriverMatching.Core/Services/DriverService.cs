using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Models;

namespace DriverMatching.Core.Services
{
    public class DriverService
    {
        private readonly List<Driver> _drivers = new();
        private readonly Dictionary<string, IDriverMatcher> _matchers = new();

        public DriverService()
        {
            // Регистрируем алгоритмы
            RegisterMatcher(new LinearSearchMatcher());
        }

        public void RegisterMatcher(IDriverMatcher matcher)
        {
            _matchers[matcher.AlgorithmName] = matcher;
        }

        public void AddDriver(Driver driver)
        {
            if (_drivers.Any(d => d.Id == driver.Id))
                throw new ArgumentException($"Driver with ID {driver.Id} already exists");

            _drivers.Add(driver);
        }

        public void UpdateDriverLocation(string driverId, Location newLocation)
        {
            var driver = _drivers.FirstOrDefault(d => d.Id == driverId);
            if (driver == null)
                throw new ArgumentException($"Driver with ID {driverId} not found");

            driver.Location = newLocation;
        }

        public List<Driver> FindNearestDrivers(Order order, string algorithmName = "Linear Search")
        {
            if (!_matchers.ContainsKey(algorithmName))
                throw new ArgumentException($"Algorithm {algorithmName} not found");

            var matcher = _matchers[algorithmName];
            return matcher.FindNearestDrivers(order, _drivers);
        }

        public List<string> GetAvailableAlgorithms()
        {
            return _matchers.Keys.ToList();
        }
    }
}