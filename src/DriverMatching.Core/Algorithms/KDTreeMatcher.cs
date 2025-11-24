using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverMatching.Core.Models;

namespace DriverMatching.Core.Algorithms
{
    public class KDTreeMatcher : IDriverMatcher

    {
        public string AlgorithmName => "KD-Tree";

        public List<Driver> FindNearestDrivers(Order order, List<Driver> drivers, int count = 5)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (drivers == null) throw new ArgumentNullException(nameof(drivers));
            if (count <= 0) throw new ArgumentException("Count must be positive", nameof(count));

            var availableDrivers = drivers.Where(d => d.IsAvailable).ToList();

            if (availableDrivers.Count <= count)
                return availableDrivers;

            // Строим KD-дерево из доступных водителей
            var tree = new KDTree(availableDrivers);
            
            // Ищем k ближайших соседей
            return tree.FindNearestNeighbors(order.PickupLocation, count);
        }
    }

    internal class KDTreeNode
    {
        public Driver Driver { get; set; } = null!;
        public KDTreeNode? Left { get; set; }
        public KDTreeNode? Right { get; set; }
        public int Depth { get; set; }

        public KDTreeNode(Driver driver, int depth)
        {
            Driver = driver;
            Depth = depth;
        }
    }

    internal class KDTree
    {
        private KDTreeNode? _root;

        public KDTree(List<Driver> drivers)
        {
            _root = BuildTree(drivers, 0);
        }

        private KDTreeNode? BuildTree(List<Driver> drivers, int depth)
        {
            if (drivers == null || drivers.Count == 0)
                return null;

            var axis = depth % 2; // 0 для X, 1 для Y

            // Сортируем по текущей оси и берем медиану
            var sorted = axis == 0 
                ? drivers.OrderBy(d => d.Location.X).ToList()
                : drivers.OrderBy(d => d.Location.Y).ToList();

            var medianIndex = sorted.Count / 2;
            var medianNode = new KDTreeNode(sorted[medianIndex], depth);

            // Рекурсивно строим левое и правое поддеревья
            medianNode.Left = BuildTree(sorted.Take(medianIndex).ToList(), depth + 1);
            medianNode.Right = BuildTree(sorted.Skip(medianIndex + 1).ToList(), depth + 1);

            return medianNode;
        }

        public List<Driver> FindNearestNeighbors(Location target, int k)
        {
            var neighbors = new List<(Driver driver, double distance)>();
            FindNearestNeighbors(_root, target, k, neighbors, 0);
            return neighbors.OrderBy(n => n.distance).Select(n => n.driver).Take(k).ToList();
        }

        private void FindNearestNeighbors(KDTreeNode? node, Location target, int k, 
            List<(Driver driver, double distance)> neighbors, int depth)
        {
            if (node == null) return;

            var distance = node.Driver.Location.DistanceTo(target);
            neighbors.Add((node.Driver, distance));

            // Сортируем и оставляем только k ближайших
            neighbors.Sort((a, b) => a.distance.CompareTo(b.distance));
            if (neighbors.Count > k)
                neighbors.RemoveAt(neighbors.Count - 1);

            var axis = depth % 2;
            var targetValue = axis == 0 ? target.X : target.Y;
            var nodeValue = axis == 0 ? node.Driver.Location.X : node.Driver.Location.Y;

            // Определяем, в каком поддереве искать сначала
            KDTreeNode? firstChild, secondChild;
            if (targetValue < nodeValue)
            {
                firstChild = node.Left;
                secondChild = node.Right;
            }
            else
            {
                firstChild = node.Right;
                secondChild = node.Left;
            }

            // Рекурсивно ищем в первом поддереве
            FindNearestNeighbors(firstChild, target, k, neighbors, depth + 1);

            // Проверяем, нужно ли искать во втором поддереве
            var worstDistance = neighbors.Count == k ? neighbors.Last().distance : double.MaxValue;
            var axisDistance = Math.Abs(targetValue - nodeValue);

            if (axisDistance < worstDistance)
                FindNearestNeighbors(secondChild, target, k, neighbors, depth + 1);
        }
    }
}