using fftbench.Benchmark;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fftbench
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!GetCommandLine(args, out int size, out int repeat))
            {
                Console.WriteLine("fftbench-cli [size] [repeat]");
                Console.WriteLine();
                Console.WriteLine("Parameters:");
                Console.WriteLine("   size    FFT size (6-14, default: 12)");
                Console.WriteLine("   repeat  Number of repeat cycles (20-200, default: 80)");
                return;
            }

            try
            {
                var task = Run(size, repeat);

                task.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task Run(int size, int repeat)
        {
            var tests = Util.LoadTests();
            var cp = new ConsoleProgress(50);

            int total = tests.Count;

            var progress = new Progress<Tuple<int, string>>(t =>
            {
                if (t.Item2 != null)
                {
                    cp.Update(t.Item1, total, t.Item2);
                }
            });

            Console.WriteLine("FFT size: 2^{0} ({1})", size, Util.Pow(2, size));
            Console.WriteLine("  Repeat: {0}", repeat);
            Console.WriteLine();

            var results = await RunBenchmark(size, repeat, tests, progress);

            Console.WriteLine();
            Console.WriteLine(Report.CreateText(results, "Exocortex (real)"));
            Console.WriteLine();

            //Console.WriteLine(EnvironmentHelper.GetCurrentInfo().ToFormattedString());
        }

        static async Task<List<BenchmarkResult>> RunBenchmark(int i, int repeat,
            List<ITest> tests, IProgress<Tuple<int, string>> progress)
        {
            return await Task.Run(() =>
            {
                var results = new List<BenchmarkResult>();

                var benchmark = new BenchmarkRunner();

                int k = 0;

                int size = Util.Pow(2, i);

                foreach (var test in tests)
                {
                    if (!test.Enabled)
                    {
                        continue;
                    }

                    progress.Report(new Tuple<int, string>(++k, "Testing " + test.Name + " ..."));

                    var data = SignalGenerator.Sawtooth(size);

                    var result = benchmark.Run(test, data, repeat);

                    results.Add(result);
                }

                progress.Report(new Tuple<int, string>(k, "Done"));

                return results;
            });
        }

        static bool GetCommandLine(string[] args, out int size, out int repeat)
        {
            size = 12;
            repeat = 80;

            if (args.Length > 0)
            {
                if (!int.TryParse(args[0], out size))
                {
                    return false;
                }
            }

            size = Math.Max(Math.Min(size, 14), 6);

            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out repeat))
                {
                    return false;
                }
            }

            repeat = Math.Max(Math.Min(repeat, 200), 20);

            return true;
        }
    }
}
