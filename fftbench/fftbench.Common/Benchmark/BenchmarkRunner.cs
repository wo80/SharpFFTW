using System;
using System.Diagnostics;

namespace fftbench.Benchmark
{
    public class BenchmarkRunner
    {
        private readonly int innerIterations;

        public BenchmarkRunner(int innerIterations = 50)
        {
            this.innerIterations = innerIterations;
        }

        public BenchmarkResult Run(ITest test, double[] data, int repeat)
        {
            var timer = new Stopwatch();

            test.Initialize(data);

            // Warmup.
            test.FFT(true);

            var result = new BenchmarkResult(test);

            var ts = new double[repeat];

            long min = long.MaxValue;
            long max = 0;
            long total = 0;

            for (int i = 0; i < repeat; i++)
            {
                timer.Restart();

                for (int j = 0; j < innerIterations; j++)
                {
                    test.FFT(true);
                }

                timer.Stop();

                var t = timer.ElapsedTicks;

                min = Math.Min(min, t);
                max = Math.Max(max, t);
                total += t;

                ts[i] = TimeSpan.FromTicks(t).TotalMilliseconds;
            }

            result.Minimum = TimeSpan.FromTicks(min).TotalMilliseconds / innerIterations;
            result.Maximum = TimeSpan.FromTicks(max).TotalMilliseconds / innerIterations;

            result.Total = TimeSpan.FromTicks(total).TotalMilliseconds;

            double stddev = 0.0;
            double mean = result.Total / repeat;

            for (int i = 0; i < repeat; i++)
            {
                stddev += (ts[i] - mean) * (ts[i] - mean);
            }

            result.Mean = mean / innerIterations;
            result.StdDev = Math.Sqrt(stddev / repeat) / innerIterations;

            return result;
        }
    }
}
