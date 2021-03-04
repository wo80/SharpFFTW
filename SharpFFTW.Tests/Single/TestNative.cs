
namespace SharpFFTW.Tests.Single
{
    using SharpFFTW;
    using SharpFFTW.Single;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Test native FFTW interface (1D).
    /// </summary>
    public class TestNative
    {
        /// <summary>
        /// Run examples.
        /// </summary>
        /// <param name="length">Logical size of the transform.</param>
        public static void Run(int length)
        {
            Console.WriteLine("Starting native test with FFT size = " + length + " (Type: single)");
            Console.WriteLine();

            try
            {
                Example1(length);
                Example2(length);
            }
            catch (BadImageFormatException)
            {
                Util.Write("Couldn't load native FFTW image (Type: single)", false);
            }
            catch (Exception e)
            {
                Util.Write(e.Message, false);
            }

            Console.WriteLine();
        }

        static void Example1(int length)
        {
            Console.Write("Test 1: complex transform ... ");

            // This example will show how to work with FFTW using unmanaged memory for
            // input and output. Since we don't have direct access to the native memory,
            // we need to allocate at least one more managed array to copy data between
            // managed and unmanaged memory.

            // Size is 2 * n because we are dealing with complex numbers.
            int size = 2 * length;

            // Create two unmanaged arrays, properly aligned.
            IntPtr pin = NativeMethods.fftwf_malloc(size * sizeof(float));
            IntPtr pout = NativeMethods.fftwf_malloc(size * sizeof(float));

            // Create managed input arrays, possibly misalinged.
            var fin = Util.GenerateSignal(size);

            // Copy managed input array to unmanaged input array.
            Marshal.Copy(fin, 0, pin, size);

            // Create test transforms (forward and backward).
            IntPtr plan1 = NativeMethods.fftwf_plan_dft_1d(length, pin, pout, Direction.Forward, Options.Estimate);
            IntPtr plan2 = NativeMethods.fftwf_plan_dft_1d(length, pout, pin, Direction.Backward, Options.Estimate);

            NativeMethods.fftwf_execute(plan1); // Forward.
            NativeMethods.fftwf_execute(plan2); // Backward.

            // Clear input array (technically not necessary).
            Array.Clear(fin, 0, fin.Length);

            // Copy unmanaged output of back-tranform to managed array (overwriting input array).
            Marshal.Copy(pin, fin, 0, size);

            // Check and see how we did.
            Util.CheckResults(length, length, fin);

            // Don't forget to free the memory after finishing.
            NativeMethods.fftwf_free(pin);
            NativeMethods.fftwf_free(pout);
            NativeMethods.fftwf_destroy_plan(plan1);
            NativeMethods.fftwf_destroy_plan(plan2);
        }

        static void Example2(int length)
        {
            Console.Write("Test 2: complex transform ... ");

            // This example will show how to work with FFTW using only managed memory for
            // input and output.

            // Size is 2 * n because we are dealing with complex numbers.
            int size = 2 * length;

            // Create two managed arrays, possibly misalinged.
            var fin = Util.GenerateSignal(size);
            var fout = new float[size];

            // Get handles and pin arrays so the GC doesn't move them
            var hin = GCHandle.Alloc(fin, GCHandleType.Pinned);
            var hout = GCHandle.Alloc(fout, GCHandleType.Pinned);

            // Get pointers to pinned array.
            IntPtr min = hin.AddrOfPinnedObject();
            IntPtr mout = hout.AddrOfPinnedObject();

            // Create test transforms (forward and backward).
            IntPtr plan1 = NativeMethods.fftwf_plan_dft_1d(length, min, mout, Direction.Forward, Options.Estimate);
            IntPtr plan2 = NativeMethods.fftwf_plan_dft_1d(length, mout, min, Direction.Backward, Options.Estimate);

            NativeMethods.fftwf_execute(plan1);

            // Clear input array and try to refill it from a backwards FFT.
            Array.Clear(fin, 0, size);

            NativeMethods.fftwf_execute(plan2);

            // Check and see how we did.
            Util.CheckResults(length, length, fin);

            // Don't forget to free the memory after finishing.
            NativeMethods.fftwf_destroy_plan(plan1);
            NativeMethods.fftwf_destroy_plan(plan2);

            hin.Free();
            hout.Free();
        }
    }
}