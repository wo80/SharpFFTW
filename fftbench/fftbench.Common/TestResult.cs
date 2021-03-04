using fftbench.Benchmark;
using System.Collections.Generic;

namespace fftbench
{
    public class TestResult
    {
        public string Name { get; private set; }

        public Dictionary<string, BenchmarkResult> Results { get; private set; }

        public TestResult(string name, BenchmarkResult result)
        {
            this.Name = name;
            this.Results = new Dictionary<string, BenchmarkResult>();

            Add(name, result);
        }

        public void Add(string name, BenchmarkResult result)
        {
            Results[name] = result;
        }
    }
}
