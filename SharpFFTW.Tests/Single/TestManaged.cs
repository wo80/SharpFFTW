
namespace SharpFFTW.Tests.Single
{
    using NUnit.Framework;
    using SharpFFTW;
    using SharpFFTW.Single;
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Test managed FFTW interface (1D).
    /// </summary>
    public class TestManaged
    {
        [Test]
        public void Test()
        {
            const int SIZE = 8192;

            Assert.True(Example1(SIZE));
            Assert.True(Example2(SIZE));
            Assert.True(Example3(SIZE));
            Assert.True(Example4(2000, false));
        }

        /// <summary>
        /// Complex to complex transform.
        /// </summary>
        bool Example1(int length)
        {
            Console.Write("Test 1: complex transform ... ");

            // Size is 2 * n because we are dealing with complex numbers.
            int size = 2 * length;

            // Create two managed arrays, possibly misaligned.
            var data = Util.GenerateSignal(size);

            // Copy to native memory.
            using var input = new ComplexArray(data);
            using var output = new ComplexArray(size);

            // Create a managed plan as well.
            using var plan1 = Plan.Create1(length, input, output, Direction.Forward, Options.Estimate);

            plan1.Execute();

            using var plan2 = Plan.Create1(length, output, input, Direction.Backward, Options.Estimate);

            plan2.Execute();

            Array.Clear(data, 0, data.Length);

            // Copy unmanaged output of back-transform to managed array.
            input.CopyTo(data);

            // Check and see how we did.
            return Util.CheckResults(length, length, data);
        }

        /// <summary>
        /// Real to complex transform.
        /// </summary>
        bool Example2(int length)
        {
            Console.Write("Test 2: real to complex transform ... ");

            int n = length;

            // Create two managed arrays, possibly misaligned.
            var data = Util.GenerateSignal(n);

            // Copy to native memory.
            using var input = new RealArray(data);
            using var output = new ComplexArray(n / 2 + 1);

            // Create a managed plan.
            using var plan1 = Plan.Create1(n, input, output, Options.Estimate);

            plan1.Execute();

            using var plan2 = Plan.Create1(n, output, input, Options.Estimate | Options.PreserveInput);

            plan2.Execute();

            Array.Clear(data, 0, n);

            // Copy unmanaged output of back-transform to managed array.
            input.CopyTo(data);

            // Check and see how we did.
            return Util.CheckResults(n, n, data);
        }

        /// <summary>
        /// Real to half-complex transform.
        /// </summary>
        bool Example3(int length)
        {
            Console.Write("Test 3: real to half-complex transform ... ");

            int n = length;

            // Create two managed arrays, possibly misaligned.
            var data = Util.GenerateSignal(n);

            // Copy to native memory.
            using var input = new RealArray(data);
            using var output = new RealArray(n);

            // Create a managed plan.
            using var plan1 = Plan.Create1(n, input, output, Transform.R2HC, Options.Estimate);

            plan1.Execute();

            using var plan2 = Plan.Create1(n, output, input, Transform.HC2R, Options.Estimate);

            plan2.Execute();

            Array.Clear(data, 0, n);

            // Copy unmanaged output of back-transform to managed array.
            input.CopyTo(data);

            // Check and see how we did.
            return Util.CheckResults(n, n, data);
        }

        /// <summary>
        /// Parallel execution.
        /// </summary>
        bool Example4(int tasks, bool print)
        {
            Console.Write("Test 4: parallel real to complex transform ... ");

            var plans = new ConcurrentDictionary<int, Tuple<RealArray, ComplexArray, Plan, Plan>>();

            const int size = 4096;

            var result = Parallel.For(0, tasks, (i, state) =>
            {
                int thread = Thread.CurrentThread.ManagedThreadId;

                var (input, output, plan1, plan2) = plans.GetOrAdd(thread, (i) =>
                {
                    var input = new RealArray(size);
                    var output = new ComplexArray(size / 2 + 1);

                    var plan1 = Plan.Create1(size, input, output, Options.Estimate);
                    var plan2 = Plan.Create1(size, output, input, Options.Estimate);

                    return new Tuple<RealArray, ComplexArray, Plan, Plan>(input, output, plan1, plan2);
                });

                var data = Util.GenerateSignal(size);

                input.Set(data);

                plan1.Execute();
                plan2.Execute();

                Array.Clear(data, 0, size);

                input.CopyTo(data);

                var success = Util.CheckResults(size, size, data);

                if (print)
                {
                    Console.WriteLine($"{i,5}: current thread = {thread}, success = {success}");
                }

                if (!success)
                {
                    state.Break();
                }
            });

            foreach (var (input, output, plan1, plan2) in plans.Values)
            {
                plan1.Dispose();
                plan2.Dispose();
                input.Dispose();
                output.Dispose();
            }

            return result.IsCompleted;
        }
    }
}