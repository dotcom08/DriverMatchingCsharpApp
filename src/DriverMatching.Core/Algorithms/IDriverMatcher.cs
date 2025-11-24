using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverMatching.Core.Models;

namespace DriverMatching.Core.Algorithms
{
    public interface IDriverMatcher
    {
        List<Driver> FindNearestDrivers(Order order, List<Driver> drivers, int count = 5);
        string AlgorithmName {get;}
    }
}