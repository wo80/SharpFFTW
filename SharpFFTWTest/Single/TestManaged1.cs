
namespace SharpFFTWTest.Single
{
    using SharpFFTW;
    using SharpFFTW.Single;
    using System;

    /// <summary>
    /// Test managed FFTW interface (1D).
    /// </summary>
    public class TestManaged1
    {
        /// <summary>
        /// Run examples.
        /// </summary>
        /// <param name="n">Logical size of the transform.</param>
        public static void Run(int length)
        {
            Console.WriteLine("Starting managed test with FFT size = " + length + " (Type: single)");
            Console.WriteLine();

            Example1(length);
            Example2(length);
            Example3(length);

            Console.WriteLine();
        }

        /// <summary>
        /// Complex to complex transform.
        /// </summary>
        static void Example1(int length)
        {
            Console.Write("Test 1: complex transform ... ");

            // Size is 2 * n because we are dealing with complex numbers.
            int size = 2 * length;

            // Create two managed arrays, possibly misalinged.
            var data = Util.GenerateSignal(size);

            // Copy to native memory.
            var input = new ComplexArray(data);
            var output = new ComplexArray(size);

            // Create a managed plan as well.
            var plan1 = Plan.Create1(length, input, output, Direction.Forward, Options.Estimate);

            plan1.Execute();

            var plan2 = Plan.Create1(length, output, input, Direction.Backward, Options.Estimate);

            plan2.Execute();

            Array.Clear(data, 0, data.Length);

            // Copy unmanaged output of back-tranform to managed array.
            input.CopyTo(data);

            // Check and see how we did.
            bool ok = Util.CheckResults(length, length, data);

            Util.WriteResult(ok);
        }

        /// <summary>
        /// Real to complex transform.
        /// </summary>
        static void Example2(int length)
        {
            Console.Write("Test 2: real to complex transform ... ");

            int n = length;

            // Create two managed arrays, possibly misalinged.
            var data = Util.GenerateSignal(n);

            // Copy to native memory.
            var input = new RealArray(data);
            var output = new ComplexArray(n / 2 + 1);

            // Create a managed plan.
            var plan1 = Plan.Create1(n, input, output, Options.Estimate);

            plan1.Execute();

            var plan2 = Plan.Create1(n, output, input, Options.Estimate | Options.PreserveInput);

            plan2.Execute();

            Array.Clear(data, 0, n);

            // Copy unmanaged output of back-tranform to managed array.
            input.CopyTo(data);

            // Check and see how we did.
            bool ok = Util.CheckResults(n, n, data);

            Util.WriteResult(ok);
        }

        /// <summary>
        /// Real to half-complex transform.
        /// </summary>
        static void Example3(int length)
        {
            Console.Write("Test 3: real to half-complex transform ... ");

            int n = length;

            // Create two managed arrays, possibly misalinged.
            var data = Util.GenerateSignal(n);

            // Copy to native memory.
            var input = new RealArray(data);
            var output = new RealArray(n);

            // Create a managed plan.
            var plan1 = Plan.Create1(n, input, output, Transform.R2HC, Options.Estimate);

            plan1.Execute();

            var plan2 = Plan.Create1(n, output, input, Transform.HC2R, Options.Estimate);

            plan2.Execute();

            Array.Clear(data, 0, n);

            // Copy unmanaged output of back-tranform to managed array.
            input.CopyTo(data);

            // Check and see how we did.
            bool ok = Util.CheckResults(n, n, data);

            Util.WriteResult(ok);
        }
    }
}