
namespace fftbench.Benchmark
{
    public class BenchmarkResult
    {
        /// <summary>
        /// The test context.
        /// </summary>
        public ITest Test;

        /// <summary>
        /// The minimum runtime in milliseconds.
        /// </summary>
        public double Minimum;

        /// <summary>
        /// The maximum runtime in milliseconds.
        /// </summary>
        public double Maximum;

        /// <summary>
        /// The mean runtime in milliseconds.
        /// </summary>
        public double Mean;

        /// <summary>
        /// The standard deviation.
        /// </summary>
        public double StdDev;

        /// <summary>
        /// The total runtime (with repeats) in milliseconds.
        /// </summary>
        public double Total;

        public BenchmarkResult(ITest test)
        {
            Test = test;
            Minimum = long.MaxValue;
        }
    }
}
