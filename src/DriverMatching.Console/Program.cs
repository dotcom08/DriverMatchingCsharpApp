using DriverMatching.Core.Models;
using DriverMatching.Core.Services;

namespace DriverMatching
{
    class Program
    {
        static void Main(string[] args)
        {
            var driverService = new DriverService();
            
            // Добавляем тестовых водителей
            for (int i = 0; i < 20; i++)
            {
                driverService.AddDriver(new Driver($"driver_{i}", i * 2, i * 3));
            }

            // Создаем заказ
            var order = new Order
            {
                Id = "order_1",
                PickupLocation = new Location(10, 15)
            };

            // Ищем ближайших водителей
            var nearestDrivers = driverService.FindNearestDrivers(order);

            Console.WriteLine("Ближайшие водители к заказу:");
            foreach (var driver in nearestDrivers)
            {
                var distance = driver.Location.DistanceTo(order.PickupLocation);
                Console.WriteLine($"Водитель {driver.Id} на расстоянии {distance:F2}");
            }

            Console.WriteLine("\nДоступные алгоритмы:");
            foreach (var algorithm in driverService.GetAvailableAlgorithms())
            {
                Console.WriteLine($"- {algorithm}");
            }
        }
    }
}