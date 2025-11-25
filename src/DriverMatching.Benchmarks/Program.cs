
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

<<<<<<< HEAD
=======
             var config = new BenchmarkConfig();

>>>>>>> feature/benchmark-tests
            if (args.Length > 0)
            {
                // Run specific benchmark from command line
                var benchmarkType = args[0];
<<<<<<< HEAD
                RunSpecificBenchmark(benchmarkType);
=======
                RunSpecificBenchmark(benchmarkType, config);
>>>>>>> feature/benchmark-tests
            }
            else
            {
                // Interactive mode
                Console.Write("Enter benchmark number (1-4): ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
<<<<<<< HEAD
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
=======
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
>>>>>>> feature/benchmark-tests
                        break;
                }
            }

            Console.WriteLine("Benchmarks completed!");
        }

<<<<<<< HEAD
        private static void RunSpecificBenchmark(string benchmarkType)
=======
        private static void RunSpecificBenchmark(string benchmarkType, IConfig config)
>>>>>>> feature/benchmark-tests
        {
            switch (benchmarkType.ToLower())
            {
                case "basic":
                case "1":
<<<<<<< HEAD
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
=======
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
>>>>>>> feature/benchmark-tests
                    break;
            }
        }

<<<<<<< HEAD
        private static void RunDriverMatchingBenchmarks()
=======
        private static void RunDriverMatchingBenchmarks(IConfig config)
>>>>>>> feature/benchmark-tests
        {
            Console.WriteLine("Running DriverMatchingBenchmarks...");
            BenchmarkRunner.Run<DriverMatchingBenchmarks>();
        }

<<<<<<< HEAD
        private static void RunAlgorithmComparisonBenchmarks()
=======
        private static void RunAlgorithmComparisonBenchmarks(IConfig config)
>>>>>>> feature/benchmark-tests
        {
            Console.WriteLine("Running AlgorithmComparisonBenchmarks...");
            BenchmarkRunner.Run<AlgorithmComparisonBenchmarks>();
        }

<<<<<<< HEAD
        private static void RunMemoryUsageBenchmarks()
=======
        private static void RunMemoryUsageBenchmarks(IConfig config)
>>>>>>> feature/benchmark-tests
        {
            Console.WriteLine("Running MemoryUsageBenchmarks...");
            BenchmarkRunner.Run<MemoryUsageBenchmarks>();
        }

<<<<<<< HEAD
        private static void RunAllBenchmarks()
        {
            Console.WriteLine("Running all benchmarks...");
            
            var config = DefaultConfig.Instance;
=======
        private static void RunAllBenchmarks(IConfig config)
        {
            Console.WriteLine("Running all benchmarks...");
            
>>>>>>> feature/benchmark-tests
            
            BenchmarkRunner.Run<DriverMatchingBenchmarks>(config);
            BenchmarkRunner.Run<AlgorithmComparisonBenchmarks>(config);
            BenchmarkRunner.Run<MemoryUsageBenchmarks>(config);
        }
    }
}