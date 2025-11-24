using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverMatching.Core.Models
{
    public class Order
    {
        public string Id { get; set; } = string.Empty;
        public Location PickupLocation { get; set; } = new Location();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}