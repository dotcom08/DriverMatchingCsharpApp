
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

             var config = new BenchmarkConfig();

            if (args.Length > 0)
            {
                // Run specific benchmark from command line
                var benchmarkType = args[0];
                RunSpecificBenchmark(benchmarkType, config);
            }
            else
            {
                // Interactive mode
                Console.Write("Enter benchmark number (1-4): ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        RunDriverMatchingBenchmarks(config);
                        break;
                    case "2":
                        RunAlgorithmComparisonBenchmarks(config);
                        break;
                    case "3":
                        RunMemoryUsageBenchmarks(config);
                        break;
                    case "4":
                    default:
                        RunAllBenchmarks(config);
                        break;
                }
            }

            Console.WriteLine("Benchmarks completed!");
        }

        private static void RunSpecificBenchmark(string benchmarkType, IConfig config)
        {
            switch (benchmarkType.ToLower())
            {
                case "basic":
                case "1":
                    RunDriverMatchingBenchmarks(config);
                    break;
                case "comparison":
                case "2":
                    RunAlgorithmComparisonBenchmarks(config);
                    break;
                case "memory":
                case "3":
                    RunMemoryUsageBenchmarks(config);
                    break;
                default:
                    RunAllBenchmarks(config);
                    break;
            }
        }

        private static void RunDriverMatchingBenchmarks(IConfig config)
        {
            Console.WriteLine("Running DriverMatchingBenchmarks...");
            BenchmarkRunner.Run<DriverMatchingBenchmarks>();
        }

        private static void RunAlgorithmComparisonBenchmarks(IConfig config)
        {
            Console.WriteLine("Running AlgorithmComparisonBenchmarks...");
            BenchmarkRunner.Run<AlgorithmComparisonBenchmarks>();
        }

        private static void RunMemoryUsageBenchmarks(IConfig config)
        {
            Console.WriteLine("Running MemoryUsageBenchmarks...");
            BenchmarkRunner.Run<MemoryUsageBenchmarks>();
        }

        private static void RunAllBenchmarks(IConfig config)
        {
            Console.WriteLine("Running all benchmarks...");
            
            
            BenchmarkRunner.Run<DriverMatchingBenchmarks>(config);
            BenchmarkRunner.Run<AlgorithmComparisonBenchmarks>(config);
            BenchmarkRunner.Run<MemoryUsageBenchmarks>(config);
        }
    }
}