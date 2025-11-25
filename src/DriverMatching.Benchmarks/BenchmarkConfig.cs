using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;

namespace DriverMatching.Benchmarks
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
        
            AddJob(Job.Default.WithId(".NET 10.0"));
            
            AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
            AddExporter(DefaultConfig.Instance.GetExporters().ToArray());
            AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
            
            WithOptions(ConfigOptions.DisableOptimizationsValidator | ConfigOptions.JoinSummary);
        }
    }
}