using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DriverMatching.Core.Models
{
    public class Driver
    {
        public string Id {get; set;} = string.Empty;
        public Location Location {get; set;}= new Location();
        public bool IsAvailable { get; set; } = true;

        public Driver() { }

        public Driver(string id, int x, int y)
        {
            Id = id;
            Location = new Location(x, y);
        }
    }
}