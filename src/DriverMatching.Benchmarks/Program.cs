
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using DriverMatching.Benchmarks.Benchmarks;

namespace DriverMatching.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Driver Matching Benchmarks...");
            Console.WriteLine("Available benchmarks:");
            Console.WriteLine("1. DriverMatchingBenchmarks - Basic performance tests");
            Console.WriteLine("2. AlgorithmComparisonBenchmarks - Parameterized comparisons");
            Console.WriteLine("3. MemoryUsageBenchmarks - Memory usage analysis");
            Console.WriteLine("4. All benchmarks");
            Console.WriteLine();

            if (args.Length > 0)
            {
                // Run specific benchmark from command line
                var benchmarkType = args[0];
                RunSpecificBenchmark(benchmarkType);
            }
            else
            {
                // Interactive mode
                Console.Write("Enter benchmark number (1-4): ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        RunDriverMatchingBenchmarks();
                        break;
                    case "2":
                        RunAlgorithmComparisonBenchmarks();
                        break;
                    case "3":
                        RunMemoryUsageBenchmarks();
                        break;
                    case "4":
                    default:
                        RunAllBenchmarks();
                        break;
                }
            }

            Console.WriteLine("Benchmarks completed!");
        }

        private static void RunSpecificBenchmark(string benchmarkType)
        {
            switch (benchmarkType.ToLower())
            {
                case "basic":
                case "1":
                    RunDriverMatchingBenchmarks();
                    break;
                case "comparison":
                case "2":
                    RunAlgorithmComparisonBenchmarks();
                    break;
                case "memory":
                case "3":
                    RunMemoryUsageBenchmarks();
                    break;
                default:
                    RunAllBenchmarks();
                    break;
            }
        }

        private static void RunDriverMatchingBenchmarks()
        {
            Console.WriteLine("Running DriverMatchingBenchmarks...");
            BenchmarkRunner.Run<DriverMatchingBenchmarks>();
        }

        private static void RunAlgorithmComparisonBenchmarks()
        {
            Console.WriteLine("Running AlgorithmComparisonBenchmarks...");
            BenchmarkRunner.Run<AlgorithmComparisonBenchmarks>();
        }

        private static void RunMemoryUsageBenchmarks()
        {
            Console.WriteLine("Running MemoryUsageBenchmarks...");
            BenchmarkRunner.Run<MemoryUsageBenchmarks>();
        }

        private static void RunAllBenchmarks()
        {
            Console.WriteLine("Running all benchmarks...");
            
            var config = DefaultConfig.Instance;
            
            BenchmarkRunner.Run<DriverMatchingBenchmarks>(config);
            BenchmarkRunner.Run<AlgorithmComparisonBenchmarks>(config);
            BenchmarkRunner.Run<MemoryUsageBenchmarks>(config);
        }
    }
}