using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverMatching.Core.Models
{
    public record Location(int X, int Y)
    {
        public Location() : this(0, 0) { }

        public double DistanceTo(Location other)
        {
            int deltaX = X - other.X;
            int deltaY = Y - other.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}